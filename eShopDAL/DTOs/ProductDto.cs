using System;
namespace eShopDAL.DTOs
{
	public class ProductDto
	{
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public int? CategoryId { get; set; }
    }
}

