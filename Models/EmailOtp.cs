namespace MVCDotnetCore.Models
{
    public class EmailOtp
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Email { get; set; }
        public string OtpCode { get; set; }
        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; }
        public DateTime CreatedDate { get; set; }





    }
}
