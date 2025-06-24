using System;
namespace eShopDAL.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}

