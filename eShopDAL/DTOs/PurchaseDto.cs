using System;
namespace eShopDAL.DTOs
{
    public class PurchaseDto
    {
        public int PurchaseId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
    }
}

