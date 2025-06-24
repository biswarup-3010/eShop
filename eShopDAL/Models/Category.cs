using System;
using System.Collections.Generic;

namespace eShopDAL.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
            UserInterests = new HashSet<UserInterest>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UserInterest> UserInterests { get; set; }
    }
}
