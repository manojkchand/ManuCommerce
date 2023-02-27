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

namespace CoreSite1.Pages.ShippingMethod
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class DetailsModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public DetailsModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public CoreSite1.Models.ShippingMethod ShippingMethod { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ShippingMethod = await _context.ShippingMethods.FirstOrDefaultAsync(m => m.ShippingMethodID == id);

            if (ShippingMethod == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
