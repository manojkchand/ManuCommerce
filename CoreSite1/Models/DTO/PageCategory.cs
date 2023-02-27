using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreSite1.Models.DTO
{
    public class PageCategory
    {
       
            public string id { get; set; }

            public string parent { get; set; }

            public string text { get; set; }

            public string icon { get; set; }

            public a_attr a_attr { get; set; }
            //public long? population { get; set; }

            //public string flagUrl { get; set; }

            //public bool @checked { get; set; }

            //public bool hasChildren { get; set; }

            public virtual List<PageCategory> children { get; set; }

            public virtual List<PageCategory> childPages { get; set; }

    }

    //public class a_attr {
    //    public string id { get; set; }
    //    public string href { get; set; }
    //    public a_attr(string Id, String Href) { id = Id; href = Href; }
    //}
}
