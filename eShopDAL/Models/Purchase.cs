using System;
using System.Collections.Generic;

namespace eShopDAL.Models
{
    public partial class Purchase
    {
        public int PurchaseId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }

        public virtual Order? Order { get; set; }
    }
}
