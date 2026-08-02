using Microsoft.EntityFrameworkCore;

namespace MVCDotnetCore.Models
{
    [Index(nameof(Email), IsUnique = true)]

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string Password { get; set; }
        public string Role { get; set; } = "Customer";
        public DateTime CreateDate{ get; set; }
        public ICollection<EmailOtp>? EmailOtps { get; set; }

    }
}
