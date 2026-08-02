using System.ComponentModel.DataAnnotations;

namespace MVCDotnetCore.Models
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
