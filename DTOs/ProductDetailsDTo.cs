namespace MVCDotnetCore.DTOs
{
    public class ProductDetailsDTo
    {
        public int Id { get; set; }

        public decimal Price { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string? ImageUrl { get; set; }

        public string? BrandDescription { get; set; }
        public string? Description
        {
            get; set;
        }

        }
    }
