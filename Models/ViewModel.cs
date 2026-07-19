using MVCDotnetCore.Migrations;

namespace MVCDotnetCore.Models
{
    public class ViewModel
    {
        public List<CartItem> CartItems { get; set; } 
        public int GrandTotal { get; set; }
    }
}