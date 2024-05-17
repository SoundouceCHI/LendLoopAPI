using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LendLoopAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
using LendLoopAPI.Services;
using LendLoopAPI.ModelDto;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.Extensions.Options;

namespace LendLoopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAppsController : ControllerBase
    {
        private readonly LendLoopContext _context;
        private readonly string _jwtKey; 

        public UserAppsController(LendLoopContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtKey = jwtSettings.Value.Key;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserAppLogin user)
        {
            var userDB = _context.Users.FirstOrDefault(x=> x.Email == user.Email);
            if (PasswordService.CheckAuth(user, userDB)) 
            {
                var token = GenerateJwtToken(userDB.UserId, userDB.UserName);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserAppRegistration user)
        {
            var userDB = _context.Users.FirstOrDefault(x => x.Email == user.Email || x.UserName == user.UserName);
            if (userDB!=null) 
            {
                throw new ArgumentException($"User with email {user.Email} or username {user.UserName} already exists.");
            }
            if (!PasswordService.IsValidEmail(user.Email)){
                throw new ArgumentException("Email invalid"); 
            }
            string pwd = PasswordService.HashPassword(user.Password); 
            var userApp = new UserApp
            {
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = pwd,
                Adress = user.Adress,
                ProfilePicUrl = null
            }; 

            _context.Users.Add(userApp);
            _context.SaveChangesAsync(); 

            var token = GenerateJwtToken(userApp.UserId, user.UserName);
            return Ok(new { Token = token , Message = "Registration successful and logged in." });
        }

        // GET: api/UserApps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserApp>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/UserApps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserApp>> GetUserApp(int id)
        {
            var userApp = await _context.Users.FindAsync(id);

            if (userApp == null)
            {
                return NotFound();
            }

            return userApp;
        }

        // PUT: api/UserApps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserApp(int id, UserApp userApp)
        {
            if (id != userApp.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userApp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAppExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserApps
        [HttpPost]
        public async Task<ActionResult<UserApp>> PostUserApp(UserApp userApp)
        {
            _context.Users.Add(userApp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserApp", new { id = userApp.UserId }, userApp);
        }

        // DELETE: api/UserApps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserApp(int id)
        {
            var userApp = await _context.Users.FindAsync(id);
            if (userApp == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userApp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserAppExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private string GenerateJwtToken(int userId, string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),  
                new Claim(ClaimTypes.Name, username)           
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
