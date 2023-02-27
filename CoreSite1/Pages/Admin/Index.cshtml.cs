using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSite1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreSite1.Pages.Admin
{
    
    [Authorize(Roles = "ThisSiteAdmin")]
    public class IndexModel : PageModel
    {
        protected readonly ApplicationDbContext context;

        public IndexModel(ApplicationDbContext _context)
        {
            context = _context;
        }

        //public IList<CoreSite1.Models.PageTemplate> Templates { get; set; }
        public IList<CoreSite1.Models.Product> Products { get; set; }

        public IList<CoreSite1.Models.Variant> Variants { get; set; }
        public List<CoreSite1.Models.Category> PageCategorys { get; set; }
        public List<CoreSite1.Models.DTO.ProductCategory> records;


        public void OnGet()
        {
        }

        public JsonResult OnGetPCategorys()//string query)
        {
            List<CoreSite1.Models.DTO.ProductCategory> Pagerecords = new List<CoreSite1.Models.DTO.ProductCategory>();
            List<CoreSite1.Models.DTO.ProductCategory> PVariantrecords = new List<CoreSite1.Models.DTO.ProductCategory>();

            PageCategorys = context.Categorys.ToList();
            Products= context.Products.ToList();
            //Variants = context.Variants.ToList();
            //if (!string.IsNullOrWhiteSpace(query))
            //{
            //    PageCategorys = PageCategorys.Where(q => q.Name.Contains(query)).ToList();
            //}

            records = PageCategorys //.Where(l => l.ParentCategoryId == 0) //.OrderBy(l => l.OrderNumber)
                .Select(l => new CoreSite1.Models.DTO.ProductCategory
                {
                    id = l.CategoryId.ToString(),
                    text = l.Name,
                        //@checked = l.Checked,
                        parent = l.ParentCategoryId.ToString(),
                    icon = "fas fa-bars",
                    a_attr = new CoreSite1.Models.DTO.a_attr(l.CategoryId.ToString(), "/Admin/PList?cid=" + l.CategoryId),

                    //LAZY LOAD needs to be implement using property.Check BOOK.
                    //Data can be brought while 2Foreach loop Below or using property below.
                    //While using property child needs to be populated uing lazy load. normally parent is returned.
                    childCategory = GetChildren(PageCategorys, l.CategoryId),
                    childProducts = GetChildProduct(Products, l.CategoryId),
                    children = PageCategorys.Any(l => l.ParentCategoryId == l.CategoryId)
                }).ToList();

            //foreach (var v in PageCategorys)
            //{
            //    Pagerecords.AddRange(
            //                            //context.Products.Where(e => e.CategoryId == v.CategoryId).include(p => p.Clients)
            //                            (from p in context.Products
            //                             where p.CategoryId == v.CategoryId

            //                             join v1 in context.Variants
            //                             on p.ProductId equals v1.ProductId
            //                             where v1.IsDefaulProduct == true
            //                             select new CoreSite1.Models.DTO.ProductCategory
            //                             {
            //                                 id = "Page" + p.ProductId.ToString(),
            //                                 vid = v1.VariantId.ToString(),
            //                                 text = p.Title, //.PageId.ToString(),
            //                                 parent = p.CategoryId.ToString(),
            //                                 icon = "fa fa-cube",
            //                                 a_attr = new CoreSite1.Models.DTO.a_attr("Page" + p.ProductId.ToString(), "/MyStore/details")
            //                                 //hasChildren = false
            //                             }
            //                            ).ToList()
            //                        );
            //}
            ////Add Page records from Page Table.
            //records.AddRange(Pagerecords);
            //foreach (var v in Pagerecords)
            //{
            //    PVariantrecords.AddRange((context.Variants.Where(e => e.ProductId == int.Parse(v.id.Remove(0, 4).Trim()))
            //            .Select(v => new CoreSite1.Models.DTO.ProductCategory
            //            {
            //                id = "VPage" + v.VariantId.ToString(),
            //                //vid = v1.VariantId.ToString(),
            //                text = v.Name, //.PageId.ToString(),
            //                parent = "Page" + v.ProductId.ToString(),
            //                icon = "fa fa-cubes",
            //                a_attr = new CoreSite1.Models.DTO.a_attr("Page" + v.VariantId.ToString(), "/MyStore/details"),
            //                isvariant = true
            //            }).ToList()
            //            ));
            //}
            ////Add Variant records from Variant Table.
            //records.AddRange(PVariantrecords);
            ////CHANGE THe root node Parent from 0 to #. As required in Json Format at UI.
            foreach (var v in records)
            {
                if (v.parent == "0")
                {
                    v.parent = "#";
                    //break;
                }
            }
            return new JsonResult(records);
        }

        public JsonResult OnGetLazyPCategory(int? id) //parent id
        {
            List<CoreSite1.Models.DTO.ProductCategory> Pagerecords = new List<CoreSite1.Models.DTO.ProductCategory>();
            List<CoreSite1.Models.DTO.ProductCategory> PVariantrecords = new List<CoreSite1.Models.DTO.ProductCategory>();


            PageCategorys = context.Categorys.ToList();
            Products = context.Products.ToList();

            records = PageCategorys.Where(l => l.ParentCategoryId == id) //.Where(l => l.ParentCategoryId == 1) //.OrderBy(l => l.OrderNumber)
                .Select(l => new CoreSite1.Models.DTO.ProductCategory 
                {
                    id = l.CategoryId.ToString(),
                    text = l.Name,
                    parent = l.ParentCategoryId.ToString(),
                    icon = "fas fa-bars",
                    a_attr = new CoreSite1.Models.DTO.a_attr(l.CategoryId.ToString(), "/MyStore/details"),
                    //childCategory = GetChildren(PageCategorys, l.CategoryId),
                    //childProducts = GetChildProduct(Products, l.CategoryId),
                    children = PageCategorys.Any(l => l.ParentCategoryId == l.CategoryId)
                }).ToList();

            //records.Add(new CoreSite1.Models.DTO.ProductCategory { id = "#" , text = "Home" });

            //foreach (var v in PageCategorys)
            //{
            //    Pagerecords.AddRange(
            //                            /*context.Products.Where(e => e.CategoryId == v.CategoryId)*/
            //                            (from p in context.Products
            //                            join v1 in context.Variants on p.ProductId equals v1.ProductId
            //                            where p.CategoryId == v.CategoryId
            //                            select new CoreSite1.Models.DTO.ProductCategory
            //                            {
            //                                id = "Page" + p.ProductId.ToString(),
            //                                vid = v1.VariantId.ToString(),
            //                                text = p.Title, //.PageId.ToString(),
            //                                parent = p.CategoryId.ToString(),
            //                                icon = "jstree-file",
            //                                a_attr = new CoreSite1.Models.DTO.a_attr("Page" + p.ProductId.ToString(), "/MyStore/details")
            //                                //hasChildren = PageCategorys.Any(l => l.ParentCategoryId == l.CategoryId)
            //                            }
            //                            ).ToList()
            //                        );
            //}
            ////Add Page records from Page Table.
            //records.AddRange(Pagerecords);
            //foreach (var v in Pagerecords)
            //{
            //    PVariantrecords.AddRange((context.Variants.Where(e => e.ProductId == int.Parse(v.id.Remove(0,4).Trim()))
            //            .Select(v => new CoreSite1.Models.DTO.ProductCategory
            //            {
            //                id = "VPage" + v.VariantId.ToString(),
            //                //vid = v1.VariantId.ToString(),
            //                text = v.Name, //.PageId.ToString(),
            //                parent = "Page" + v.ProductId.ToString(),
            //                icon = "jstree-file",
            //                a_attr = new CoreSite1.Models.DTO.a_attr("Page" + v.VariantId.ToString(), "/MyStore/details"),
            //                isvariant = true
            //            }).ToList()
            //            ));
            //}
            ////Add Variant records from Variant Table.
            //records.AddRange(PVariantrecords);
            //CHANGE THe root node Parent from 0 to #. As required in Json Format at UI.
            foreach (var v in records)
            {
                if (v.parent == "0")
                {
                    v.parent = "#";
                }
            }
            return new JsonResult(records);
        }

        private JsonResult GetChildren(List<CoreSite1.Models.Category> PCategory, int parentId)
        {
            //List<CoreSite1.Models.DTO.ProductCategory>
            PCategory = context.Categorys.Where(e => e.ParentCategoryId == parentId).ToList();

            var records = PCategory.Where(l => l.ParentCategoryId == parentId) //.OrderBy(l => l.OrderNumber)
                .Select(l => new CoreSite1.Models.DTO.ProductCategory
                {
                    id = l.CategoryId.ToString(),
                    text = l.Name,
                    parent = l.ParentCategoryId.ToString(),
                    a_attr = new CoreSite1.Models.DTO.a_attr(l.CategoryId.ToString(), "/MyStore/details"),
                    childCategory = GetChildren(PageCategorys, l.CategoryId),
                    childProducts = GetChildProduct(Products, l.CategoryId),
                    children = PageCategorys.Any(l => l.ParentCategoryId == l.CategoryId) // || context.Products.Any(e => e.CategoryId == l.CategoryId)
                }).ToList();
            //return records;
            return new JsonResult(records);
        }

        private JsonResult GetChildProduct(IList<CoreSite1.Models.Product> PProduct, int parentId)
        {
            //List<CoreSite1.Models.DTO.ProductCategory>
            Products = context.Products.Where(e => e.CategoryId == parentId).ToList();

            var childs = (from p in context.Products
                          where p.CategoryId == parentId

                          join v1 in context.Variants
                          on p.ProductId equals v1.ProductId
                          where v1.IsDefaulProduct == true
                          select new CoreSite1.Models.DTO.ProductCategory
                        {
                            id = "Page" + p.ProductId.ToString(),
                            vid = v1.VariantId.ToString(),
                            text = p.Title, //.PageId.ToString(),
                            parent = p.CategoryId.ToString(),
                            icon = "jstree-file",
                            a_attr = new CoreSite1.Models.DTO.a_attr("Page" + p.ProductId.ToString(), "/MyStore/details"),
                            childVariants= GetChildVariant(context.Variants.Where(e => e.ProductId == parentId).ToList(), p.ProductId),
                            children=true 
                          }).ToList();
            //return childs;
            return new JsonResult(childs);
        }

        private static JsonResult GetChildVariant(IList<CoreSite1.Models.Variant> PVariant, int parentId)
        {
            //PVariant = context.Variants.Where(e => e.ProductId == parentId).ToList();List<CoreSite1.Models.DTO.ProductCategory>

            var childs = PVariant //.Where(l => l.ParentCategoryId == parentId) //.OrderBy(l => l.OrderNumber)
                 .Select(v => new CoreSite1.Models.DTO.ProductCategory
                 {
                     id = "VPage" + v.VariantId.ToString(),
                     //vid = v1.VariantId.ToString(),
                     text = v.Name, //.PageId.ToString(),
                     parent = "Page" + v.ProductId.ToString(),
                     icon = "fa fa-cubes",
                     a_attr = new CoreSite1.Models.DTO.a_attr("Page" + v.VariantId.ToString(), "/MyStore/details"),
                     isvariant = true,
                     children =false
                 }).ToList();

            //return childs; 
            return new JsonResult(childs);
        }

    }
}
