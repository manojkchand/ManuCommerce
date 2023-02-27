using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSite1.Models
{
    public class Page
    {
        [ScaffoldColumn(false)]
        public int PageId { get; set; }

        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }

        [ScaffoldColumn(false)]
        public string AddedDate { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }
        //[ScaffoldColumn(false)]

        [DisplayName("Templete")]
        public int PageTempleteId { get; set; }

        [StringLength(350)]
        public string PageName { get; set; }

        [StringLength(350)]
        public string Heading { get; set; }

        [StringLength(350)]
        public string SubHeading { get; set; }


        public string Content { get; set; }

        public string URL { get; set; }

        public string LayoutPage { get; set; }
        
        public string HeaderImage { get; set; }

        //[Required(ErrorMessage = "An Product Brand is required")]
        //[Timestamp]
        //public byte[] RowVersion { get; set; }
        //these notmapped field are used to hold data from variant table

    }
}
