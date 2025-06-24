using System;
using System.Collections.Generic;

namespace eShopDAL.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductImages = new HashSet<ProductImage>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
