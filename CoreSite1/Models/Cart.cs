using System.ComponentModel.DataAnnotations;
namespace CoreSite1.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int ProductId { get; set; }
        //optional
        public int? VariantId { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual Product Product { get; set; }
        public virtual Variant Variant { get; set; }

        public OrderType ItemOrderType { get; set; }
    }
}