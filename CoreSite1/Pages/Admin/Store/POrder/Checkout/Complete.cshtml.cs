using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoreSite1.Pages.MyStore.POCheckout
{//     /MyStore/Checkout/Complete?id=9
    public class CompleteModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public CompleteModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.Category> Category { get; set; }
        public CoreSite1.Models.Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int? id=  HttpContext.Session.GetInt32("NewOrderId");
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.Address).Include(o => o.OrderDetails).FirstOrDefaultAsync(m => m.OrderId == id);

            if (Order == null)
            {
                return NotFound();
            }
            
            Category = await _context.Categorys.ToListAsync();
            return Page();
        }
    }
}