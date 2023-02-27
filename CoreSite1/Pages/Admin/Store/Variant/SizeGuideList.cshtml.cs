using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin.Store.Variant
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class SizeGuideListModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public SizeGuideListModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SizeGuide> SizeGuide { get;set; }

        public async Task OnGetAsync()
        {
            SizeGuide = await _context.SizeGuide.ToListAsync();
        }
    }
}
