using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LendLoopAPI.Models;

namespace LendLoopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAppsController : ControllerBase
    {
        private readonly LendLoopContext _context;

        public UserAppsController(LendLoopContext context)
        {
            _context = context;
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
    }
}
