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
    public class BrowseModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public BrowseModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.Product> Product { get;set; }
        public IQueryable<CoreSite1.Models.Product> SortedProductModel { get; set; }

        public async Task OnGetAsync()
        {
            SortedProductModel = from p in _context.Products
                                 join v in _context.Variants on p.ProductId equals v.ProductId
                                 where v.IsDefaulProduct == true //&& p.Title.Contains(searchString) || v.IsDefaulProduct == true && p.Brand.Contains(searchString)
                                 select new CoreSite1.Models.Product{ ProductId = p.ProductId, CategoryId = p.CategoryId, AddedDate = p.AddedDate,
                                     AddedBy = p.AddedBy,
                                     Title = p.Title,
                                     Price = p.Price,
                                     Brand = p.Brand,
                                     Discount = p.Discount,
                                     ProductArtUrl = p.ProductArtUrl,
                                     Description = p.Description,
                                     StockOfNonVariant = p.StockOfNonVariant,
                                     RowVersion = p.RowVersion,
                                     UnitInStock = v.UnitInStock,
                                     Colour = v.color,
                                     Size = v.Size

                                 };//v.VariantId, v.UnitInStock, v.IsDefaulProduct };

            Product = await _context.Products
                .Include(p => p.Category).ToListAsync();
        }
    }
}
