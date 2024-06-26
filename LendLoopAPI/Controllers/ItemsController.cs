﻿using System;
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
    public class ItemsController : ControllerBase
    {
        private readonly LendLoopContext _context;

        public ItemsController(LendLoopContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }
        [HttpGet("searchItem/{name}")]
        public async Task<ActionResult<List<Item>>> GetItemByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return NotFound();
            }
            var items = await _context.Items.Where(w => w.Title.Contains(name.ToLower())).ToListAsync();
     
            if (items.Count == 0)
            {
                return NotFound();
            }

            return Ok(items);
        }
        [HttpGet("searchItemByUserId/{userId}")]
        public async Task<ActionResult<List<Item>>> GetItemByByUserId(int userId)
        {
            var items = await _context.Items.Where(item => item.UserId == userId).ToListAsync();
            if (items.Count == 0)
            {
                return NotFound();
            }

            return Ok(items);
        }
        [HttpGet("searchByCategory/{idCategory}")]
        public async Task<ActionResult<List<Item>>> GetItemByCategory(int idCategory)
        {
            var subcategories =await _context.Subcategories.Where(x => x.CategoryId == idCategory).Select(x=>x.SubcategoryId).ToListAsync();
            if(!subcategories.Any())
            {
                return NotFound();  
            }
            var items = await _context.Items.Where(x => subcategories.Contains(x.SubcategoryId)).ToListAsync();
            if (!items.Any() )
            {
                return NotFound();
            }

            return Ok(items);

        }

        [HttpGet("searchByKeyWord/{keyWord}")]
        public async Task<ActionResult<List<Item>>> GetItemByKeyWord(string keyWord)
        {
            if(keyWord == null)
            {
                return NotFound();
            }
            var items = await _context.Items.Where(x=> x.Description.Contains(keyWord) || x.Title.Contains(keyWord)).ToListAsync();
            if (!items.Any())
            {
                return NotFound(); 
            }
            return Ok(items);

        }
        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.ItemId }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
