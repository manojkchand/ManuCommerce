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

namespace CoreSite1.Pages.OrderItem
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class CreateModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public CreateModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
        ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Brand");
        ViewData["VariantId"] = new SelectList(_context.Variants, "VariantId", "VariantId");
            return Page();
        }

        [BindProperty]
        public CoreSite1.Models.OrderItem OrderItem { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.OrderItem.Add(OrderItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}