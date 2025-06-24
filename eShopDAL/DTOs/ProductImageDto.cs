using System;
namespace eShopDAL.DTOs
{
    public class ProductImageDto
    {
        public int ImageId { get; set; }
        public int? ProductId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool? IsPrimary { get; set; }
    }
}

