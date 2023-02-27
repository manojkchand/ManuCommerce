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
    public class JSTreeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JSTreeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JSTree
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Page>>> GetPages()
        {
            return await _context.Pages.ToListAsync();
        }

        // GET: api/JSTree/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Page>> GetPage(int id)
        {
            var page = await _context.Pages.FindAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return page;
        }

        // PUT: api/JSTree/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPage(int id, Page page)
        {
            if (id != page.PageId)
            {
                return BadRequest();
            }

            Page DBpage = _context.Pages.Where(e => e.PageId == page.PageId).FirstOrDefault();
            DBpage.PageName = page.PageName;
            DBpage.URL = CreateURL(DBpage);

            _context.Entry(DBpage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(id))
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

        // PUT: api/JSTree/5
        //[HttpPut("{id}")]
        //[HttpPost]
        //public async Task<IActionResult> OnPostPageName(int id, string pagename)
        //{
        //    if (pagename == "")
        //    {
        //        return BadRequest();
        //    }

        //    Page page = _context.Pages.Where(e => e.PageId == id).FirstOrDefault();
        //    page.PageName = pagename;

        //    _context.Entry(page).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PageExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/JSTree
        [HttpPost]
        public async Task<ActionResult<Page>> PostPage(Page page)
        {
            //if (!PageExists(page.PageId))
            //{
            //    Page DBpage = _context.Pages.Where(e => e.PageId == page.PageId).FirstOrDefault();
            //    DBpage.PageName = page.PageName;
            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //            throw;
            //    }

            //    return NoContent();

            //}
            //else
            //{
            //need to all Page URL field dynamically here.
            page.URL = CreateURL(page);

            _context.Pages.Add(page);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPage", new { id = page.PageId }, page);
            //}
           
        }

        // DELETE: api/JSTree/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Page>> DeletePage(int id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();

            return page;
        }

        private bool PageExists(int id)
        {
            return _context.Pages.Any(e => e.PageId == id);
        }

        string CreateURL(CoreSite1.Models.Page values)
        {
            string s = "";
            int cid = values.CategoryId;
            s = "/" + values.PageName.ToString();// +"?id="+ Page.PageId;

            var categorys = _context.PCategorys.ToList();

            while (cid != 1)
            {
                s = "/" + categorys.Where(e => e.PageCategoryId == cid).FirstOrDefault().Name + s;
                cid = categorys.Where(e => e.PageCategoryId == cid).FirstOrDefault().ParentCategoryId;
            }

            return s;
        }
    }
}
