using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSite1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoreSite1.Pages.Admin
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class OListModel : PageModel
    {
        protected readonly ApplicationDbContext context;

        public OListModel(ApplicationDbContext _context)
        {
            context = _context;
        }

        public List<CoreSite1.Models.Order> Categorys { get; set; }
        public List<CoreSite1.Models.DTO.OrderCategory> records;

        List<CoreSite1.Models.DTO.OrderCategory> monthrecords = new List<CoreSite1.Models.DTO.OrderCategory>();
        List<CoreSite1.Models.DTO.OrderCategory> dayrecords = new List<CoreSite1.Models.DTO.OrderCategory>();

        public string CurrentFilter { get; set; }
        public PaginatedList<CoreSite1.Models.Order> Order { get; set; }

        public async Task OnGetAsync(string currentFilter, string searchString, int? pageIndex,string dt, string yr, string mnt)
        {
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;
            IQueryable<CoreSite1.Models.Order> orderIQ;


            if (dt != null)
            {
                DateTime seareddate = DateTime.Parse(dt); 
                 orderIQ = from s in context.Orders.Include("OrderDetails").Where(e => e.OrderDate.Date == seareddate.Date)
                                                             select s;
            }
            else if (mnt != null && yr != null)
            {
                //DateTime seareddate = DateTime.Parse(dt);
                orderIQ = from s in context.Orders.Include("OrderDetails").Where(e => e.OrderDate.Month == int.Parse(mnt) && e.OrderDate.Year == int.Parse(yr))
                          select s;
            }
            else if (yr != null)
            {
                //DateTime seareddate = DateTime.Parse(dt);
                orderIQ = from s in context.Orders.Include("OrderDetails").Where(e => e.OrderDate.Year == int.Parse(yr))
                          select s;
            }
           
            else
            {
                orderIQ = from s in context.Orders.Include("OrderDetails")
                                                             select s;
            }

           

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchString.All(char.IsDigit))
                {
                    orderIQ = orderIQ.Where(s => s.OrderId.Equals(int.Parse(searchString)));
                }
                else
                {
                    orderIQ = orderIQ.Where(s => s.AddedBy.Contains(searchString));
                }

            }

            foreach(var v in orderIQ)
            {
               foreach(var od in v.OrderDetails)
                {
                    od.SKU = context.Products.Where(e => e.ProductId == od.ProductId).FirstOrDefault().ProductArtUrl;
                }
            }

            int pageSize = 10;
            Order = await PaginatedList<CoreSite1.Models.Order>.CreateAsync(
            orderIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            //Order = await _context.Orders
            //    .Include(o => o.Address).Include(o => o.OrderDetails).ToListAsync();
        }

        public JsonResult OnGetOCategorys()//string query)
        {
            records = new List<Models.DTO.OrderCategory>();
            var g = new CoreSite1.Models.DTO.OrderCategory
            {
                id = "1",
                text = "Orders",
                //@checked = l.Checked,
                parent = "#",
                icon = "fas fa-calendar",
                a_attr = new CoreSite1.Models.DTO.a_attr("1", "/Admin/OList"),

                //LAZY LOAD needs to be implement using property.Check BOOK.
                //Data can be brought while 2Foreach loop Below or using property below.
                //While using property child needs to be populated uing lazy load. normally parent is returned.
                childCategory = null
                //childProducts = GetChildProduct(Products, l.CategoryId),
                //children = Categorys.Any(l => l.ParentCategoryId == l.CategoryId)
            };
            records.Add(g);
            //Categorys = (from o in context.Orders
            //            select new CoreSite1.Models.Order{OrderId = o.OrderId, OrderDate = o.OrderDate.Date}
            //            ).Distinct().ToList();

            var date= context.Orders.Select(e => e.OrderDate.Date).Distinct().ToList();
            //var years = date.Select(e => e.Date.Year).Distinct().ToList();
            //var months = date.Select(e => e.Date.Month).Distinct().ToList();
            //var days = date.Select(e => e.Date.Day).Distinct().ToList();
            //Variants = context.Variants.ToList();
            //if (!string.IsNullOrWhiteSpace(query))
            //{
            //    PageCategorys = PageCategorys.Where(q => q.Name.Contains(query)).ToList();
            //}

            records.AddRange( date.Select(e => e.Year).Distinct() //.Where(l => l.ParentCategoryId == 0) //.OrderBy(l => l.OrderNumber)
                .Select(l => new CoreSite1.Models.DTO.OrderCategory
                {
                    id = "year" + l.ToString(),
                    text = l.ToString(),
                    //@checked = l.Checked,
                    parent = "1",
                    icon = "fas fa-calendar",
                    a_attr = new CoreSite1.Models.DTO.a_attr("year" + l.ToString(), "/Admin/OList?yr=" + l),

                    //LAZY LOAD needs to be implement using property.Check BOOK.
                    //Data can be brought while 2Foreach loop Below or using property below.
                    //While using property child needs to be populated uing lazy load. normally parent is returned.
                    childCategory = GetChildren(date, l)
                    //monthrecords.AddRange(GetChildren(date, l.Year))
                    //childProducts = GetChildProduct(Products, l.CategoryId),
                    //children = Categorys.Any(l => l.ParentCategoryId == l.CategoryId)
                }).ToList());
            records.AddRange(dayrecords);
            records.AddRange(monthrecords);
           

            foreach(var v in records)
            {
                v.childCategory = null;
            }
            
            ////CHANGE THe root node Parent from 0 to #. As required in Json Format at UI.
            //foreach (var v in records)
            //{
            //    if (v.parent == "0")
            //    {
            //        v.parent = "#";
            //        //break;
            //    }
            //}

            
            return new JsonResult(records);
        }

     
        private JsonResult GetChildren(List<DateTime> OCategory, int parentId)
        {
            //List<CoreSite1.Models.DTO.ProductCategory>
            //OCategory = OCategory. /*context.Categorys.Where(e => e.ParentCategoryId == parentId).ToList();*/

            var records = OCategory.Where(l => l.Year == parentId).Select(l => l.Month).Distinct() //.OrderBy(l => l.OrderNumber)
                .Select(l => new CoreSite1.Models.DTO.OrderCategory
                {
                    id = "Month"+ l.ToString()+ parentId.ToString(),
                    text = l.ToString() + "th Month",
                    parent = "year" + parentId.ToString(),
                    icon = "fas fa-calendar",
                    a_attr = new CoreSite1.Models.DTO.a_attr("Month" + l.ToString() + parentId.ToString(), "/Admin/OList?mnt=" + l +"&yr=" + parentId),
                    childCategory = GetChildrenChildren(OCategory, parentId, l)
                   
                    //childProducts = GetChildProduct(Products, l.CategoryId),
                    //children = PageCategorys.Any(l => l.ParentCategoryId == l.CategoryId) // || context.Products.Any(e => e.CategoryId == l.CategoryId)
                }).ToList();
            //return records;
            monthrecords.AddRange(records);
            return new JsonResult(records);
        }

        private JsonResult GetChildrenChildren(List<DateTime> OCategory, int year, int month)
        {
            //List<CoreSite1.Models.DTO.ProductCategory>
           // OCategory = OCategory. /*context.Categorys.Where(e => e.ParentCategoryId == parentId).ToList();*/

            var records = OCategory.Where(l => l.Year == year && l.Month == month).Select(l => l.Day).Distinct() //.OrderBy(l => l.OrderNumber)
                .Select(l => new CoreSite1.Models.DTO.OrderCategory
                {
                    id = "day" + l.ToString() + year.ToString() + month.ToString(),
                    text = l.ToString(),
                    parent = "Month" + month.ToString() + year.ToString(),
                    icon = "fas fa-calendar",
                    a_attr = new CoreSite1.Models.DTO.a_attr("day" + l.ToString() + year.ToString() + month.ToString(), "/Admin/OList?dt=" +new DateTime(year,month,l).Date.ToString()),
                    childCategory = null
                    //childProducts = GetChildProduct(Products, l.CategoryId),
                    //children = PageCategorys.Any(l => l.ParentCategoryId == l.CategoryId) // || context.Products.Any(e => e.CategoryId == l.CategoryId)
                }).ToList();
            //return records;
            dayrecords.AddRange(records);
            return new JsonResult(records);
        }
    }
}
