using System;
namespace eShopDAL.DTOs
{
    public class UserInterestDto
    {
        public int InterestId { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? InterestedAt { get; set; }
    }
}

