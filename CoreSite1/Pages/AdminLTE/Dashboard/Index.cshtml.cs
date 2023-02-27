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

namespace CoreSite1.Pages.Admin.Dashboard
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;
        public IQueryable<CoreSite1.Models.Product> SortedProductModel { get; set; }

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.OrderItem> OrderItem { get;set; }
        public IList<CoreSite1.Models.Product> Product { get; set; }

        public async Task OnGetAsync()
        {
            //OrderItem = await _context.OrderItem
            //    .Include(o => o.Order)
            //    .Include(o => o.Product)
            //    .Include(o => o.Variant).ToListAsync();


            int PageCount = 5;//_context.OrderItem.Count() / 5;

            OrderItem = await PaginatedList<CoreSite1.Models.OrderItem>.CreateAsync((_context.OrderItem
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Include(o => o.Variant)).AsNoTracking(), PageCount, 5);



            //return View(await PaginatedList<Product>.CreateAsync(SortedProductModel.AsNoTracking(), page ?? 1, pageSize));


            
       
            SortedProductModel = from p in _context.Products
                                 join v in _context.Variants on p.ProductId equals v.ProductId
                                 //where p.CategoryId == cid
                                 //where v.IsDefaulProduct == true //&& p.Title.Contains(searchString) || v.IsDefaulProduct == true && p.Brand.Contains(searchString)
                                 select new CoreSite1.Models.Product
                                 {
                                     ProductId = p.ProductId,
                                     CategoryId = p.CategoryId,
                                     AddedDate = p.AddedDate,
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
                                     Size = v.Size,
                                     VariantId = v.VariantId,
                                     OptionalImageURL = v.OptionalImageURL
                                 };

            //get total stock for each prooduct from - (Items in stock + sold Items + Items in cart)
            //Array[][] a;
            //foreach (var item in SortedProductModel)
            //{
            //    var v = _context.OrderItem.Where(p => p.ProductId == item.ProductId).Select(m => m.Quantity).Sum();
            //    var j= _context.Carts.Where(p => p.ProductId == item.ProductId).Select(m => m.Count).Sum();
                
            //}
            //Product = await _context.Products.Take(5)
            //  .ToListAsync();
            int pageSize = 5;
            int count = _context.Products.Count()/5;
            Product = await PaginatedList<CoreSite1.Models.Product>.CreateAsync(SortedProductModel.AsNoTracking(), count, pageSize);
        }
    }
}
