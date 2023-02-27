using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CoreSite1.Models
{
    //[Bind(Exclude = "OrderId")]
    public partial class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        [ScaffoldColumn(false)]
        public string AddedBy { get; set; }

        [ScaffoldColumn(false)]
        public int AddressID { get; set; }

        [ScaffoldColumn(false)]
        public OrderStatus Status { get; set; }

        [ScaffoldColumn(false)]
        public PaymentStatus PaymentStatus { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(256)]
        public string ShippingMethod { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }

        [ScaffoldColumn(false)]
        public decimal ShippingAmount { get; set; }

        [StringLength(256)]
        [ScaffoldColumn(false)]
        public string TransactionID { get; set; }


        [ScaffoldColumn(false)]
        public System.DateTime? ShippingDate { get; set; }


        [StringLength(256)]
        [ScaffoldColumn(false)]
        public string TrackingID { get; set; }

        [StringLength(256)]
        public string ReOpendReason { get; set; }

        [DefaultValue("False")]
        public bool ReOpend_RefundManually { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public List<OrderItem> OrderDetails { get; set; }
        public virtual Address Address { get; set; }


        [DefaultValue(OrderType.SalesOrder)]
        [ScaffoldColumn(false)]
        public OrderType OrderType { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Deliverd,
        Closed,
        Reopend,
        Complete
    }

    public enum PaymentStatus
    {
        Received,
        Pending,
        PaymentOnDelivery,
        ReceivedPreviously_ExchangeOrder,
        Paid
    }

    public enum OrderType
    {
        SalesOrder,
        PurchaseOrder
    }
}