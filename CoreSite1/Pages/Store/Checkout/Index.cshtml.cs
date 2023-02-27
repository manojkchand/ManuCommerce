using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.Store.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            //ERROR CHECK IS 
            //CHECK IF THERE ARE NO ITEMS IN BASKET. IF NO ITEM THEN REDIRECT TO HOME.


        //ViewData["AddressID"] = new SelectList(_context.Addresses, "AddressID", "City");
            return Page();
        }

        [BindProperty]
        public CoreSite1.Models.Order Order { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}