using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CoreSite1.Models
{

    public class ShippingMethod
    {
        [ScaffoldColumn(false)]
        public int ShippingMethodID { get; set; }
        [ScaffoldColumn(false)]
        public DateTime AddedDate { get; set; }
        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }
        [StringLength(256)]
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}