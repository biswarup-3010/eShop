using System;
using System.Collections.Generic;

namespace eShopDAL.Models
{
    public partial class UserInterest
    {
        public int InterestId { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? InterestedAt { get; set; }

        public virtual Category? Category { get; set; }
        public virtual User? User { get; set; }
    }
}
