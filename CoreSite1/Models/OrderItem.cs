using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CoreSite1.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public string Title { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal FinalUnitPrice { get; set; }//discounted
        public int Discount { get; set; }
        public string ProductImage { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual Product Product { get; set; }
        public virtual Variant Variant { get; set; }
        public virtual Order Order { get; set; }
        public OrderStatus Status { get; set; }
    }
}