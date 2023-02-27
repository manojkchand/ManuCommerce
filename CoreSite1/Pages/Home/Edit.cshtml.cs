using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Http;
//using Page = CoreSite1.Models.Page;

namespace CoreSite1.Pages.Home
{
    public class EditModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public EditModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CoreSite1.Models.Page Page { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Page = await _context.Pages.FirstOrDefaultAsync(m => m.PageId == id);

            if (Page == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection values)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
             
            Page.URL = CreateURL(values);
            // Page.Content = values["editor1"].ToString();
            _context.Attach(Page).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(Page.PageId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //var v = values["Page.CategoryId"].ToString();
            //return Redirect("./List/?cid="+ v);//RedirectToPage
            return Redirect("./?id=" + Page.PageId);
        }

        private bool PageExists(int id)
        {
            return _context.Pages.Any(e => e.PageId == id);
        }

        string CreateURL(IFormCollection values)
        {
            string s = "";
            int cid = int.Parse(values["Page.CategoryId"]);
            s = "/" + values["Page.PageName"].ToString();// + "?id=" + values["Page.PageId"].ToString();

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
