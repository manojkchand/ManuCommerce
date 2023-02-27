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
    public class DeleteModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public DeleteModel(CoreSite1.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Page = await _context.Pages.FindAsync(id);

            if (Page != null)
            {
                _context.Pages.Remove(Page);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
