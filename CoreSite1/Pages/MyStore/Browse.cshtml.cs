using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.Extensions.Configuration;

namespace CoreSite1.Pages.MyStore
{
    public class BrowseModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public BrowseModel(CoreSite1.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //public IList<CoreSite1.Models.Product> Product { get;set; }

        //public async Task OnGetAsync()
        //{
        //    Product = await _context.Products
        //        .Include(p => p.Category).ToListAsync();
        //}
        public PaginatedList<CoreSite1.Models.Product> Product { get; set; }
        //public IQueryable<CoreSite1.Models.Product> SortedProductModel { get; set; }
       // public IList<CoreSite1.Models.Category> Category { get; set; }
        public string CurrentCategory { get; set; }

        
        public string CurrentFilter { get; set; }
        public string PriceFilter { get; set; }
        public string DiscountFilter { get; set; }
        //used to show list of brands avalible
        public List<CoreSite1.Models.Product> BrandFilterList { get; set; }

        public string CurrentSort { get; set; }

        // public IList<CoreSite1.Models.Product> Product { get;set; }

        //public PaginatedList<CoreSite1.Models.Product> Product { get; set; }

        public async Task OnGetAsync(string CategoryName,string currentFilter, string price, string discount, string searchString, string sortOrder, int? pageIndex)
        {
            //for Menu
            //Category = await _context.Categorys.ToListAsync();
            //requrird for paging URL
            CurrentCategory = CategoryName;
            //filter fields
            //check if user input something in search textbox in any page come back to page1.
            //if (searchString != null)
            //{
            //    pageIndex = 1;
            //}
            //else
            //{
            //    searchString = String.IsNullOrEmpty(currentFilter) ? "" : currentFilter;
            //}
            CurrentFilter = String.IsNullOrEmpty(searchString) ? "" : searchString;//searchString=brandfilter
            PriceFilter = String.IsNullOrEmpty(price) ?  "": price;
            DiscountFilter = String.IsNullOrEmpty(discount) ? "" : discount;
           

            //GET PRODUCT'S
            var cid = _context.Categorys.Where(e => e.Name == CategoryName).First().CategoryId;
            IQueryable<CoreSite1.Models.Product> productIQ = from p in _context.Products
                                                             where p.CategoryId == cid

                                                             join v in _context.Variants 
                                                             on p.ProductId equals v.ProductId
                                                             where v.IsDefaulProduct == true
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




            //IF PARENT CATEGORY GET SUB CATEGORY PRODUCTS
            //if category is base than get SUBSUB category product
            //bool parentcategory = false;
            if (CategoryName == "Mens" || CategoryName == "Womens" || CategoryName == "Kids")
            {
                //parentcategory = true;
                //get category id
                int id = _context.Categorys.Where(e=>e.Name == CategoryName).First().CategoryId;

                //get all sub categorys
                IQueryable<CoreSite1.Models.Category> SubCategory = _context.Categorys.Where(e => e.ParentCategoryId == id).AsQueryable();
                   
                // get all subsub category
                IQueryable<CoreSite1.Models.Category> SubsubCategory = _context.Categorys.Where(s => SubCategory.Any(y => y.CategoryId == s.ParentCategoryId)).AsQueryable(); 
                //foreach (var cat in SubCategory)
                //{
                //    SubsubCategory = SubsubCategory.Union(Category.Where(e => e.ParentCategoryId == cat.CategoryId).AsQueryable());
                //}
                
                //get all products from subsubcategory.
                //foreach (var cate in SubsubCategory)
                //{
                //int sid = Category.Where(e => e.Name == cate.Name.ToString()).First().CategoryId;

                IQueryable<CoreSite1.Models.Product> AllProducts = from p in _context.Products
                                                                                         join v in _context.Variants on p.ProductId equals v.ProductId
                                                                                         //where p.CategoryId == cate.CategoryId
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

                   // productIQ = productIQ.Union(SortedProductModelTemp);
                    productIQ = AllProducts.Where(s => SubsubCategory.Any(y => y.CategoryId == s.CategoryId));

                //}
            }


            BrandFilterList =  productIQ.ToList()
                               .GroupBy(y => y.Brand)
                               .Select(y => y.FirstOrDefault())
                               .ToList();
            
           




            //END GET PRODUCTS



            //FILTER
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                searchString = searchString.Remove(searchString.Length - 1, 1);//removes the last extra comma

                string[] words = searchString.Split(',');
                //int i = 0;//check first item
                //IQueryable<CoreSite1.Models.Product> SortedProductModeltemp;
                //SortedProductModeltemp = productIQ;
                productIQ = productIQ.Where(s => words.Any(y => y == s.Brand));
                //productIQ = SortedProductModeltemp;
            }

            if (!String.IsNullOrEmpty(price))
            {
                if (price.Contains("₹"))
                {
                    decimal d = decimal.Parse(price.Replace("₹", ""));
                    if (d == 500) { productIQ = productIQ.Where(s => s.Price <= d); }
                    if (d == 1000) { productIQ = productIQ.Where(s => s.Price <= 1000 && s.Price >= 500); }
                    if (d == 1500) { productIQ = productIQ.Where(s => s.Price <= 1500 && s.Price >= 1000); }
                    if (d == 2000) { productIQ = productIQ.Where(s => s.Price <= 2000 && s.Price >= 1500); }
                    if (d == 2500) { productIQ = productIQ.Where(s => s.Price <= 2500 && s.Price >= 2000); }
                    if (d == 2501) { productIQ = productIQ.Where(s => s.Price > 2500); }
                }
            }

            if (!String.IsNullOrEmpty(discount))
            {

                if (discount.Contains("%"))
                {
                    int i = int.Parse(discount.Replace("%", ""));
                    if (i == 10) { productIQ = productIQ.Where(s => s.Discount >= i); }
                    if (i == 20) { productIQ = productIQ.Where(s => s.Discount >= i); }
                    if (i == 30) { productIQ = productIQ.Where(s => s.Discount >= i); }
                    if (i == 40) { productIQ = productIQ.Where(s => s.Discount >= i); }
                    if (i == 50) { productIQ = productIQ.Where(s => s.Discount >= i); }
                    if (i == 60) { productIQ = productIQ.Where(s => s.Discount >= i); }
                    if (i == 70) { productIQ = productIQ.Where(s => s.Discount >= i); }
                    if (i == 80) { productIQ = productIQ.Where(s => s.Discount >= i); }
                }
            }

          
            //FILTER END

            //SORT
            //sort fields=sort parameter
            CurrentSort = sortOrder;

            switch (sortOrder)
            {
                case "Name":
                    productIQ = productIQ.OrderBy(s => s.Title);
                    break;
                case "Brand":
                    productIQ = productIQ.OrderBy(s => s.Brand);
                    break;
                case "Price":
                    productIQ = productIQ.OrderBy(s => s.Price);
                    break;
                case "Price_Desc":
                    productIQ = productIQ.OrderByDescending(s => s.Price);
                    break;
                case "Discount":
                    productIQ = productIQ.OrderByDescending(s => s.Discount);
                    break;
                default:
                    productIQ = productIQ.OrderBy(s => s.Title);
                    break;
            }
            //SORT END

            //PAGING
            int pageSize = Configuration.GetValue("PageSize", 10);//10;
            Product = await PaginatedList<CoreSite1.Models.Product>.CreateAsync(
            productIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            //PAGING END
           


        }

       

    }
}
