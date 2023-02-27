using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Category
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class CreateModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;
        public SelectList selectCategory;
        public CreateModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;

        }
        
        public IActionResult OnGet()
        {
            
            selectCategory = new SelectList(_context.Categorys.AsEnumerable()
                                                                            .Select(g => new SelectListItem
                                                                            {
                                                                                Text = g.Name,
                                                                                Value = g.CategoryId.ToString()

                                                                            })
                                                                            );
            return Page();
        }

        [BindProperty]
        public CoreSite1.Models.Category Category { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categorys.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}