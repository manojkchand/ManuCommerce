using CoreSite1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSite1.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        protected readonly ApplicationDbContext context;

        public PrivacyModel(ILogger<PrivacyModel> logger, ApplicationDbContext _context)
        {
            _logger = logger;
            context = _context;
        }



        //public IList<CoreSite1.Models.PageTemplate> Templates { get; set; }
        public IList<CoreSite1.Models.Page> Pages { get; set; }
        public List<CoreSite1.Models.PageCategory> PageCategorys { get; set; }
        public List<CoreSite1.Models.DTO.PageCategory> records;


        public void OnGet()
        {
        }

        public JsonResult OnGetPCategorys()
        {
            //List<Location> locations;
            //List<Models.DTO.Location> records;
            List<CoreSite1.Models.DTO.PageCategory> Pagerecords = new List<CoreSite1.Models.DTO.PageCategory>();
            PageCategorys = context.PCategorys.ToList();

            //if (!string.IsNullOrWhiteSpace(query))
            //{
            //    locations = locations.Where(q => q.Name.Contains(query)).ToList();
            //}

            records = PageCategorys //.Where(l => l.ParentCategoryId == 1) //.OrderBy(l => l.OrderNumber)
                .Select(l => new CoreSite1.Models.DTO.PageCategory
                {
                    id = l.PageCategoryId.ToString(),
                    text = l.Name,
                        //@checked = l.Checked,
                        parent = l.ParentCategoryId.ToString(),
                    a_attr = new Models.DTO.a_attr(l.PageCategoryId.ToString(), context.PTemplate.Where(e => e.PageTemplateId == l.PageTempleteId).FirstOrDefault().TempleteURL)
                        //population = l.Population,
                        //flagUrl = l.FlagUrl,
                        //children = GetChildren(PageCategorys, l.PageCategoryId),
                        //childPages= context.Pages.Where(e => e.CategoryId  == l.PageCategoryId)
                        //.Select(l1 => new CoreSite1.Models.DTO.PageCategory
                        //{
                        //    id = l1.PageId,
                        //    text = l1.Heading,
                        //    parent = l.ParentCategoryId.ToString(),
                        //    hasChildren = false
                        //}).ToList()
                    }).ToList();

            foreach (var v in PageCategorys)
            {
                Pagerecords.AddRange((context.Pages.Where(e => e.CategoryId == v.PageCategoryId)
                        .Select(l1 => new CoreSite1.Models.DTO.PageCategory
                        {
                            id = "Page" + l1.PageId.ToString(),
                            text = l1.PageName, //.PageId.ToString(),
                            parent = l1.CategoryId.ToString(),
                            icon = "jstree-file",
                            a_attr = new Models.DTO.a_attr("Page" + l1.PageId.ToString(), context.PTemplate.Where(e => e.PageTemplateId == l1.PageTempleteId).FirstOrDefault().TempleteURL)
                            //hasChildren = false
                        }).ToList()
                        ));
            }
            //Add Page records from Page Table.
            records.AddRange(Pagerecords);
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

        public JsonResult OnGetLazyPCategory(int? parentId)
        {
            //List<Location> locations;
            //List<Models.DTO.Location> records;
            List<CoreSite1.Models.DTO.PageCategory> Pagerecords = new List<CoreSite1.Models.DTO.PageCategory>();
            PageCategorys = context.PCategorys.Where(e => e.ParentCategoryId == parentId).ToList();

            records = PageCategorys //.Where(l => l.ParentCategoryId == 1) //.OrderBy(l => l.OrderNumber)
                   .Select(l => new CoreSite1.Models.DTO.PageCategory
                   {
                       id = l.PageCategoryId.ToString(),
                       text = l.Name,
                       //@checked = l.Checked,
                       parent = l.ParentCategoryId.ToString(),
                       a_attr = new Models.DTO.a_attr(l.PageCategoryId.ToString(), context.PTemplate.Where(e => e.PageTemplateId == l.PageTempleteId).FirstOrDefault().TempleteURL)
                   }).ToList();

            foreach (var v in PageCategorys)
            {
                Pagerecords.AddRange((context.Pages.Where(e => e.CategoryId == v.PageCategoryId)
                        .Select(l1 => new CoreSite1.Models.DTO.PageCategory
                        {
                            id = "Page" + l1.PageId.ToString(),
                            text = l1.PageName, //.PageId.ToString(),
                            parent = l1.CategoryId.ToString(),
                            icon = "jstree-file",
                            a_attr = new Models.DTO.a_attr("Page" + l1.PageId.ToString(), context.PTemplate.Where(e => e.PageTemplateId == l1.PageTempleteId).FirstOrDefault().TempleteURL)
                            //hasChildren = false
                        }).ToList()
                        ));
            }

            //Add Page records from Page Table.
            records.AddRange(Pagerecords);
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

        private List<CoreSite1.Models.DTO.PageCategory> GetChildren(List<CoreSite1.Models.PageCategory> PCategory, int parentId)
        {
            List<CoreSite1.Models.DTO.PageCategory> Pagerecords = new List<CoreSite1.Models.DTO.PageCategory>();
            PCategory = context.PCategorys.Where(e => e.ParentCategoryId == parentId).ToList();
            var childs = PCategory.Where(l => l.ParentCategoryId == parentId) //.OrderBy(l => l.OrderNumber)
                .Select(l => new CoreSite1.Models.DTO.PageCategory
                {
                    id = l.PageCategoryId.ToString(),
                    text = l.Name,
                    parent = l.ParentCategoryId.ToString(),
                    a_attr = new Models.DTO.a_attr(l.PageCategoryId.ToString(), context.PTemplate.Where(e => e.PageTemplateId == l.PageTempleteId).FirstOrDefault().TempleteURL)
                    //population = l.Population,
                    //flagUrl = l.FlagUrl,
                    //@checked = l.Checked,
                    //children = GetChildren(PCategory, l.PageCategoryId),
                    //hasChildren = GetChildren(PCategory, l.PageCategoryId).Any(),
                    //childPages = context.Pages.Where(e => e.CategoryId == l.PageCategoryId)
                    //    .Select(l1 => new CoreSite1.Models.DTO.PageCategory
                    //    {
                    //        id = l1.PageId,
                    //        text = l1.Heading,
                    //        parent = l.ParentCategoryId.ToString() + l.PageCategoryId.ToString(),
                    //        hasChildren = false
                    //    }).ToList()
                }).ToList();

            foreach (var v in PageCategorys)
            {
                Pagerecords.AddRange((context.Pages.Where(e => e.CategoryId == v.PageCategoryId)
                        .Select(l1 => new CoreSite1.Models.DTO.PageCategory
                        {
                            id = "Page" + l1.PageId.ToString(),
                            text = l1.PageName,
                            parent = l1.CategoryId.ToString(),
                            icon = "jstree-file",

                            a_attr = new Models.DTO.a_attr("Page" + l1.PageId.ToString(), context.PTemplate.Where(e => e.PageTemplateId == l1.PageTempleteId).FirstOrDefault().TempleteURL)
                        }).ToList()
                        ));
            }

            childs.AddRange(Pagerecords);
            return childs;
        }

        // PUT: api/JSTree/5
        //[HttpPut("{id}")]
        //[HttpPost]
        public JsonResult OnPostPageName(int id, string pagename)
        {
            //if (pagename == "")
            //{
            //    return BadRequest();
            //}

            CoreSite1.Models.Page page = context.Pages.Where(e => e.PageId == id).FirstOrDefault();
            if (page != null)
            {
                page.PageName = pagename;
            }
            //context.Entry(page).State = EntityState.Modified;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (context.Pages.Any(e => e.PageId == id))
                {
                    return new JsonResult(false);
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(true);
        }

    }
}
