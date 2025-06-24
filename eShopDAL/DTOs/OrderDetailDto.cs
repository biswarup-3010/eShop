using System;
namespace eShopDAL.DTOs
{
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? PriceAtPurchase { get; set; }

        // Optional - Include product info if needed
        public string? ProductName { get; set; }
    }
}

