using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.Admin.Controllers.Map
{
    public class SearchModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public SearchModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MapStock> MapStock { get;set; }

        public async Task OnGetAsync()
        {
            MapStock = await _context.MapStock.ToListAsync();
        }


        public JsonResult OnGetMapImage(string searchString)
        {
            //var mapImage = await _context.MapImage.FindAsync(id);

            //if (mapImage == null)
            //{
            //    return NotFound();
            //}

            //return mapImage;

            IQueryable<CoreSite1.Models.Product> productIQ = from p in _context.Products
                                                             join v in _context.Variants on p.ProductId equals v.ProductId
                                                             //where p.Title == CurrentFilter || p.Brand == currentFilter
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
                                                                 OptionalImageURL = v.OptionalImageURL,

                                                                 chcekboxAnswer = false
                                                             };//v.VariantId, v.UnitInStock, v.IsDefaulProduct };


            productIQ = productIQ.Where(s => s.Title.Contains(searchString)
                  || s.Brand.Contains(searchString));

            var g = _context.MapStock.Where(a => a.ProductId == productIQ.FirstOrDefault().ProductId).FirstOrDefault();

            return new JsonResult(g);


        }

    }
}
