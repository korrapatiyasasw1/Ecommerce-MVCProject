using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCDotnetCore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int stock { get; set; }
        public string Brand { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public bool IsAcitve { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
