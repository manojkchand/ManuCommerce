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

namespace CoreSite1.Pages.Admin.Status
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class VariantStockStatusModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public VariantStockStatusModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.Variant> Variant { get;set; }

        public async Task OnGetAsync()
        {
            Variant = await _context.Variants
                .Include(v => v.Product).OrderBy(e => e.UnitInStock).ToListAsync();
        }
    }
}
