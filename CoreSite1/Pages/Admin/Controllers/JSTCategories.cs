using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin.Service
{
    
    
    [Route("api/[controller]")]
    [ApiController]
    public class JSTCategories : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JSTCategories(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JSTCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoreSite1.Models.Category>>> GetCategorys()
        {
            return await _context.Categorys.ToListAsync();
        }

        // GET: api/JSTCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoreSite1.Models.Category>> GetCategory(int id)
        {
            var category = await _context.Categorys.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/JSTCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CoreSite1.Models.Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }
            CoreSite1.Models.Category DBCategory = _context.Categorys.Where(e => e.CategoryId == category.CategoryId).FirstOrDefault();
            DBCategory.Name = category.Name;
            //DBpage.URL = CreateURL(DBpage);

            _context.Entry(DBCategory).State = EntityState.Modified;
           // _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/JSTCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpPost]
        public async Task<ActionResult<CoreSite1.Models.Category>> PostCategory(CoreSite1.Models.Category category)
        {
            category.AddedDate = DateTime.Now;
            _context.Categorys.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        // DELETE: api/JSTCategories/5
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CoreSite1.Models.Category>> DeleteCategory(int id)
        {
            var category = await _context.Categorys.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categorys.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        private bool CategoryExists(int id)
        {
            return _context.Categorys.Any(e => e.CategoryId == id);
        }
    }
}
