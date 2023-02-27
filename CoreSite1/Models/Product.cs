using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreSite1.Models
{
    //[Bind(Exclude = "ProductId")]
    public class Product
    {
        [ScaffoldColumn(false)]
        public int ProductId { get; set; }
        [ScaffoldColumn(false)]
        //[DisplayName("Category")]
        public int CategoryId { get; set; }
        [ScaffoldColumn(false)]
        public DateTime AddedDate { get; set; }
        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }
        //[DisplayName("Variant")]
        //public int VariantId { get; set; }
        [Required(ErrorMessage = "An Product Title is required")]
        [StringLength(160)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Price is required")]
        //[Range(0.01,100,ErrorMessage="Price must be between 0.01 and 100.00")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "An Product Brand is required")]
        [StringLength(160)]
        public string Brand { get; set; }
        [Required(ErrorMessage = "An Product Discount is required")]
        public int Discount { get; set; }
        [DisplayName("Product Art URL")]
        [StringLength(360)]
        public string ProductArtUrl { get; set; }
        [DefaultValue(0.0)]
        public decimal CostPrice { get; set; }

        [DisplayName("Description")]
        
        public string Description { get; set; }
        public Category Category { get; set; }
        public int? StockOfNonVariant { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //these notmapped field are used to hold data from variant table
        [NotMapped]
        public int? UnitInStock { get; set; }
       
        [ConcurrencyCheck]
        public long? UPC { get; set; } //Universal Product Code -Barcode
        [NotMapped]
        public string Colour { get; set; }
        [NotMapped]
        public string Size { get; set; }
        [NotMapped]
        public int? VariantId { get; set; }
       
        [NotMapped]
        public string OptionalImageURL { get; set; }

        [NotMapped]
        public bool chcekboxAnswer { get; set; }//requred for brand list


        public List<Variant> Variantlist { get; set; }

        public virtual List<OrderItem> OrderDetails { get; set; }
    }
}