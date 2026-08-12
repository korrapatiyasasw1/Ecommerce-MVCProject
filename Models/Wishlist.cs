namespace MVCDotnetCore.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<WishlistItem> WishlistItems { get; set; } = new();


    }
}
