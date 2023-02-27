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

namespace CoreSite1.Pages.Admin
{
    [Authorize(Roles = "ThisSiteAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class JSTreeNodeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JSTreeNodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JSTreeNode
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PageCategory>>> GetPCategorys()
        {
            return await _context.PCategorys.ToListAsync();
        }

        // GET: api/JSTreeNode/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PageCategory>> GetPageCategory(int id)
        {
            var pageCategory = await _context.PCategorys.FindAsync(id);

            if (pageCategory == null)
            {
                return NotFound();
            }

            return pageCategory;
        }

        // PUT: api/JSTreeNode/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPageCategory(int id, PageCategory pageCategory)
        {
            if (id != pageCategory.PageCategoryId)
            {
                return BadRequest();
            }
            PageCategory DBpageCategory = _context.PCategorys.Where(e => e.PageCategoryId == pageCategory.PageCategoryId).FirstOrDefault();
            DBpageCategory.Name = pageCategory.Name;

            _context.Entry(DBpageCategory).State = EntityState.Modified;
            //_context.Entry(pageCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageCategoryExists(id))
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

        // POST: api/JSTreeNode
        [HttpPost]
        public async Task<ActionResult<PageCategory>> PostPageCategory(PageCategory pageCategory)
        {
            pageCategory.AddedDate = DateTime.Now;
            _context.PCategorys.Add(pageCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPageCategory", new { id = pageCategory.PageCategoryId }, pageCategory);
        }

        // DELETE: api/JSTreeNode/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PageCategory>> DeletePageCategory(int id)
        {
            var pageCategory = await _context.PCategorys.FindAsync(id);
            if (pageCategory == null)
            {
                return NotFound();
            }

            _context.PCategorys.Remove(pageCategory);
            await _context.SaveChangesAsync();

            return pageCategory;
        }

        private bool PageCategoryExists(int id)
        {
            return _context.PCategorys.Any(e => e.PageCategoryId == id);
        }
    }
}
