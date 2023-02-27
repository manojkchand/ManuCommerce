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
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin.PageTemplate
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class EditModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public EditModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            CoreSite1.Models.PageTemplate DBpageT = _context.PTemplate.Where(e => e.PageTemplateId == PageTemplate.PageTemplateId).FirstOrDefault();
            DBpageT.Header = PageTemplate.Header;
            DBpageT.Footer = PageTemplate.Footer;
            DBpageT.SideBar = PageTemplate.SideBar;
            _context.Attach(DBpageT).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageTemplateExists(PageTemplate.PageTemplateId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PageTemplateExists(int id)
        {
            return _context.PTemplate.Any(e => e.PageTemplateId == id);
        }
    }
}
