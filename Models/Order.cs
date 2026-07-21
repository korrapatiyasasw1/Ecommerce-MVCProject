namespace MVCDotnetCore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }
        public int CustomerAddressId { get; set; }
        public CustomerAddress? CustomerAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); 
    }
}
