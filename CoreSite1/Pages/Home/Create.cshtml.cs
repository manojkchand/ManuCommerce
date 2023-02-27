using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
//using Page = CoreSite1.Models.Page;CoreSite1CoreSite1

namespace CoreSite1.Pages.Home
{
    public class CreateModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public CreateModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CoreSite1.Models.Page Page { get; set; }

        public async Task<IActionResult> OnPostAsync(IFormCollection values)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Page.URL = CreateURL(values);

            _context.Pages.Add(Page);
            await _context.SaveChangesAsync();

            //Page=
            //Page.URL = CreateURL(values,Page);
            //_context.Attach(Page).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            
            //return RedirectToPage("./List");
            var v = values["Page.CategoryId"].ToString();
            return Redirect("./List/?cid=" + v);//RedirectToPage
        }

        string CreateURL(IFormCollection values)
        {
            string s = "";
            int cid = int.Parse(values["Page.CategoryId"]);
            s = "/" + values["Page.PageName"].ToString();// +"?id="+ Page.PageId;

            var categorys= _context.PCategorys.ToList();

            while(cid != 1)
            { 
            s = "/" + categorys.Where(e => e.PageCategoryId == cid).FirstOrDefault().Name + s;
                cid = categorys.Where(e => e.PageCategoryId == cid).FirstOrDefault().ParentCategoryId;
            }

            return s;
        }
    }
}