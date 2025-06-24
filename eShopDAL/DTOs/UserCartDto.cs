using System;
namespace eShopDAL.DTOs
{
    public class UserCartDto
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime? AddedAt { get; set; }
    }
}

