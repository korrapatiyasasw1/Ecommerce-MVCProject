using System.ComponentModel.DataAnnotations;

namespace MVCDotnetCore.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Please enter the mail")]
        [EmailAddress(ErrorMessage ="enter a valid mail")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Pls enter the password")]
        [StringLength(15)]
        public string Password { get; set; }
    }
}

