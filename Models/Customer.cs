using System.ComponentModel.DataAnnotations;

namespace MVCDotnetCore.Models
{
    public class Customer
    {
            public int Id { get; set; }

            [Required]
            [StringLength(100)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(100)]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Phone]
            public string PhoneNumber { get; set; }

            [Required]
            public string Address { get; set; }



            public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? User { get; set; }

        }
    }
    
