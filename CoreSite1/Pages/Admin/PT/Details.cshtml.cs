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

namespace CoreSite1.Pages.Admin.PageTemplate
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class DetailsModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public DetailsModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public CoreSite1.Models.PageTemplate PageTemplate { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PageTemplate = await _context.PTemplate.FirstOrDefaultAsync(m => m.PageTemplateId == id);

            if (PageTemplate == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
