using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CoreSite1.Models
{
    public class PageCategory
    {
        [ScaffoldColumn(false)]
        public int PageCategoryId { get; set; }
        [ScaffoldColumn(false)]
        public DateTime AddedDate { get; set; }
        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentCategoryId { get; set; }
        [DisplayName("Templete")]
        public int PageTempleteId { get; set; }
        public string LayoutPage { get; set; }
        public bool Checked { get; set; }
        public int OrderNumber { get; set; }
        public virtual List<PageCategory> Children { get; set; }
        public virtual PageCategory Parent { get; set; }

        public List<Page> Pages { get; set; }
    }
}
