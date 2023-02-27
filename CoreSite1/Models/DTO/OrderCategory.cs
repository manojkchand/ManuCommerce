using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSite1.Models.DTO
{
    public class OrderCategory
    {
        public string id { get; set; }

        public string vid { get; set; }


        public string parent { get; set; }

        public string text { get; set; }

        public string icon { get; set; }

        public a_attr a_attr { get; set; }
        //public long? population { get; set; }

        //public string flagUrl { get; set; }

        //public bool @checked { get; set; }

        public bool isvariant { get; set; }

        public bool children { get; set; }

        public virtual JsonResult childCategory { get; set; }

        //public virtual JsonResult childProducts { get; set; }

        //public virtual JsonResult childVariants { get; set; }

    }

}
