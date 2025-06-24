using System;
using System.Collections.Generic;

namespace eShopDAL.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
            UserInterests = new HashSet<UserInterest>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<UserInterest> UserInterests { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
