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

namespace CoreSite1.Pages.Admin.Store.Map
{
    public class EditModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public EditModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MapImage MapImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MapImage = await _context.MapImage.FirstOrDefaultAsync(m => m.MapImageID == id);

            if (MapImage == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MapImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapImageExists(MapImage.MapImageID))
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

        private bool MapImageExists(int id)
        {
            return _context.MapImage.Any(e => e.MapImageID == id);
        }
    }
}
