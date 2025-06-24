using System;
using System.Collections.Generic;

namespace eShopDAL.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Purchases = new HashSet<Purchase>();
        }

        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
