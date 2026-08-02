using System.ComponentModel.DataAnnotations;
namespace MVCDotnetCore.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
