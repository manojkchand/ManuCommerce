using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoreSite1.Models
{
    public class Variant
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string AddedBy { get; set; }
        public string SKU { get; set; }
        public int UnitInStock { get; set; }
        public int OldUnitInStock { get; set; }
        public int DamagedStock { get; set; }
        public int ReturnedDamagedStock { get; set; }
        public string Name { get; set; }
        public String Size { get; set; }
        public int Quantity { get; set; }
        public String color { get; set; }
        [DisplayName("One Default Product in Variants")]
        [DefaultValue(false)]
        public bool IsDefaulProduct { get; set; }
        public String OptionalImageURL { get; set; }
        public decimal OptionalPrice { get; set; }
        public virtual Product Product { get; set; }

    }
}