using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCDotnetCore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("CategoryName")]
        [Required(ErrorMessage = "Category Name is required")]


        [MaxLength(30)]
        public string Name { get; set; }


    }
}
