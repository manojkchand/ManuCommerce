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

namespace CoreSite1.Pages.OrderItem
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class DeleteModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public DeleteModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CoreSite1.Models.OrderItem OrderItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            OrderItem = await _context.OrderItem
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Include(o => o.Variant).FirstOrDefaultAsync(m => m.OrderItemId == id);

            if (OrderItem == null)
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

            OrderItem = await _context.OrderItem.FindAsync(id);

            if (OrderItem != null)
            {
                _context.OrderItem.Remove(OrderItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
