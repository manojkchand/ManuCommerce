using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoreSite1.Models
{
    public class Category
    {
        [ScaffoldColumn(false)]
        public int CategoryId { get; set; }
        [ScaffoldColumn(false)]
        public DateTime AddedDate { get; set; }
        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
        public string Description { get; set; } 

        public string LinkType { get; set; }
        //public PaginatedList <Product> Products { get; set; }
        public List<Product> Products { get; set; }
    }
}