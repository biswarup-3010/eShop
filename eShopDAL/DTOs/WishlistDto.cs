namespace eShopDAL.DTOs
{
    public class WishlistDto
    {
        public int WishlistId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? AddedAt { get; set; }
    }
}
