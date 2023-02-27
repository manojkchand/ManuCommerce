using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.Admin.Store.Map
{
    public class CreateModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public CreateModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public SelectList MapShapeList;

        public IActionResult OnGet()
        {
            //showind shipping method
            var v = new List<string>();
            v.Add("circle");
            v.Add("poly");
            v.Add("rect");
            MapShapeList = new SelectList(v);

            return Page();
        }

        [BindProperty]
        public MapImage MapImage { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.MapImage.Add(MapImage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
