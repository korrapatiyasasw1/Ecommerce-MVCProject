using MVCDotnetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVCDotnetCore.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace MVCDotnetCore.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;
        private readonly EmailSettings _settings;


        public AccountService(AppDbContext context, IConfiguration configuration,
            IOptions<EmailSettings> settings)
        {
            _context = context;
            _configuration = configuration;
            _settings = settings.Value;
        }
        public async Task Register(RegisterDTO model)
        {
            var existingUser = await _context.Users
.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (existingUser != null)
            {
                throw new Exception("Email already exists.");
            }
            var User = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
                CreateDate = DateTime.Now,
            };
            _context.Users.Add(User);
            await _context.SaveChangesAsync();
            var message = new MailMessage();
            message.From = new MailAddress(_settings.Email);
            message.To.Add(new MailAddress(model.Email));
            string otp = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            message.Subject = "Your Otp";
            message.Body = otp;
            message.IsBodyHtml = true;
            using var smtp = new SmtpClient(_settings.Host, _settings.Port);
            smtp.Credentials = new NetworkCredential(_settings.Email, _settings.Password);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(message);

            _context.EmailOtps.Add(new EmailOtp
            {
                UserId = User.Id,
                OtpCode = otp,
                Email = User.Email,
                CreatedDate = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            });

            await _context.SaveChangesAsync();
        }
        public async Task VerifyOtp(VerifyOtp OTP)
        {
            var emailotp = _context.EmailOtps.Include(x => x.User).
                FirstOrDefault(x => x.Email == OTP.Email && x.OtpCode == OTP.OtpCode );
            if (emailotp == null)
            {
                throw new Exception("OTP IS NOT VERIFIED");
            }
            if (emailotp.ExpiryTime < DateTime.UtcNow)
            {
                throw new Exception("otp has been expired");
            }
            emailotp.IsUsed = true;
            _context.SaveChanges();
        }
        public async Task<User?> Login(LoginDTO model)
        {
            if (model == null)
            {
                throw new Exception("Please give me login credietials ");
            }
            var login = _context.Users.Include(x=>x.EmailOtps).FirstOrDefault(
                x=>x.Email == model.Email && x.Password ==  model.Password
                );
            if (login == null)
            {
                throw new Exception("Please enter valid email and password");
            }

            return login;

        }
       


    }
}
