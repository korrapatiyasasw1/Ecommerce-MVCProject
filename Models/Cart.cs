namespace MVCDotnetCore.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Customer Customer { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    }
}
