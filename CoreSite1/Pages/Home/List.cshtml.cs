using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
//using Page = CoreSite1.Models.Page;

namespace CoreSite1.Pages.Home
{
    public class ListModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public ListModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.Page> Pages { get;set; }
        public string Layout { get; set; }
        public IList<CoreSite1.Models.Category> Category { get; set; }

        public async Task OnGetAsync(int? cid)
        {

           

            if (cid == null)
            {
                Layout = null;
                Pages = await _context.Pages.ToListAsync();
            }
            else
            {
                Layout = _context.PCategorys.Where(e => e.PageCategoryId == cid).FirstOrDefault().LayoutPage;
               Pages = await _context.Pages.Where(e=>e.CategoryId == cid).ToListAsync();
               
            }

            if (Layout == "~/Pages/Shared/_LayoutMyStore.cshtml")
            {
                Category = await _context.Categorys.ToListAsync();
            }

        }
    }
}
