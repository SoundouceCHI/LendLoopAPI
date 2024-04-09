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
    public class QRCodeImagesController : ControllerBase
    {
        private readonly LendLoopContext _context;

        public QRCodeImagesController(LendLoopContext context)
        {
            _context = context;
        }

        // GET: api/QRCodeImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QRCodeImage>>> GetQRCodeImages()
        {
            return await _context.QRCodeImages.ToListAsync();
        }

        // GET: api/QRCodeImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QRCodeImage>> GetQRCodeImage(int id)
        {
            var qRCodeImage = await _context.QRCodeImages.FindAsync(id);

            if (qRCodeImage == null)
            {
                return NotFound();
            }

            return qRCodeImage;
        }

        // PUT: api/QRCodeImages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQRCodeImage(int id, QRCodeImage qRCodeImage)
        {
            if (id != qRCodeImage.QRCodeImageId)
            {
                return BadRequest();
            }

            _context.Entry(qRCodeImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QRCodeImageExists(id))
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

        // POST: api/QRCodeImages
        [HttpPost]
        public async Task<ActionResult<QRCodeImage>> PostQRCodeImage(QRCodeImage qRCodeImage)
        {
            _context.QRCodeImages.Add(qRCodeImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQRCodeImage", new { id = qRCodeImage.QRCodeImageId }, qRCodeImage);
        }

        // DELETE: api/QRCodeImages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQRCodeImage(int id)
        {
            var qRCodeImage = await _context.QRCodeImages.FindAsync(id);
            if (qRCodeImage == null)
            {
                return NotFound();
            }

            _context.QRCodeImages.Remove(qRCodeImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QRCodeImageExists(int id)
        {
            return _context.QRCodeImages.Any(e => e.QRCodeImageId == id);
        }
    }
}
