using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.MyStore
{
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.Product> Product { get;set; }
        //public IList<CoreSite1.Models.Category> Category { get; set; }
        public IList<CoreSite1.Models.Page> Pages { get;set; }
        public async Task OnGetAsync()
        {
            Product = await _context.Products
                .Include(p => p.Variantlist)
                .Include(p => p.Category).Where(p => p.Category.Name == "NEW PRODUCT" || p.Category.Name == "FEATURE" || p.Category.Name == "BEST SELLER" || p.Category.Name == "HOT TREND").ToListAsync();

            //Category = await _context.Categorys.ToListAsync();

            Pages = await _context.Pages.Where(e => e.CategoryId == 1074).ToListAsync();
            //IQueryable<CoreSite1.Models.Product> productIQ = from p in _context.Products.Include(p => p.Category)
            //                                                        join v in _context.Variants on p.ProductId equals v.ProductId
            //                                                        where p.Category.Name == "New"
            //                                                        //where v.IsDefaulProduct == true //&& p.Title.Contains(searchString) || v.IsDefaulProduct == true && p.Brand.Contains(searchString)
            //                                                        select new CoreSite1.Models.Product
            //                                                        {
            //                                                            ProductId = p.ProductId,
            //                                                            CategoryId = p.CategoryId,
            //                                                            AddedDate = p.AddedDate,
            //                                                            AddedBy = p.AddedBy,
            //                                                            Title = p.Title,
            //                                                            Price = p.Price,
            //                                                            Brand = p.Brand,
            //                                                            Discount = p.Discount,
            //                                                            ProductArtUrl = p.ProductArtUrl,
            //                                                            Description = p.Description,
            //                                                            StockOfNonVariant = p.StockOfNonVariant,
            //                                                            RowVersion = p.RowVersion,

            //                                                            UnitInStock = v.UnitInStock,
            //                                                            Colour = v.color,
            //                                                            Size = v.Size,
            //                                                            VariantId = v.VariantId,
            //                                                            OptionalImageURL = v.OptionalImageURL,

            //                                                            chcekboxAnswer = false
            //                                                        };//v.VariantId, v.UnitInStock, v.IsDefaulProduct };
            //Product = productIQ.ToList();

        }
    }
}
