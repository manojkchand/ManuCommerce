using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.Store
{
    public class BrowseVariantModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public BrowseVariantModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.Variant> Variant { get;set; }
        IQueryable<CoreSite1.Models.Product> SortedProductModel { get; set; }

        public async Task OnGetAsync()
        {
            //var stock = (from p in storeDB.Products
            //                               join v in storeDB.Variants on p.ProductId equals v.ProductId
            //                               where v.IsDefaulProduct == true && p.Title.Contains(searchString) || v.IsDefaulProduct == true && p.Brand.Contains(searchString)
            //                               select new { p.ProductId, v.VariantId, v.UnitInStock, v.IsDefaulProduct }).ToList(); ;

            Variant = await _context.Variants
                .Include(v => v.Product).ToListAsync();
        }
    }
}
