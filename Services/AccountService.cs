using MVCDotnetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVCDotnetCore.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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


            if (existingUser != null && existingUser.IsEmailVerified)
            {
                throw new Exception("Already registered");
            }
            else if (existingUser == null)
            {
                var User = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "Customer",
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
                    ExpiryTime = DateTime.UtcNow.AddMinutes(2),
                    IsUsed = false
                });
                await _context.SaveChangesAsync();
            }
            else
               // if (existingUser != null && !existingUser.IsEmailVerified )
            {
                var existingOtp = await _context.EmailOtps
    .FirstOrDefaultAsync(x => x.UserId == existingUser.Id);

                var mess = new MailMessage();
                mess.From = new MailAddress(_settings.Email);
                mess.To.Add(new MailAddress(model.Email));
                string otpcode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
                mess.Subject = "Your Otp";
                mess.Body = otpcode;
                mess.IsBodyHtml = true;
                using var sm = new SmtpClient(_settings.Host, _settings.Port);
                sm.Credentials = new NetworkCredential(_settings.Email, _settings.Password);
                sm.EnableSsl = true;
                if (existingOtp == null)
                {
                    _context.EmailOtps.Add(new EmailOtp
                    {
                        UserId = existingUser.Id,
                        Email = existingUser.Email,
                        OtpCode = otpcode,
                        CreatedDate = DateTime.UtcNow,
                        ExpiryTime = DateTime.UtcNow.AddMinutes(2),
                        IsUsed = false
                    });
                }
                else
                {
                    existingOtp.OtpCode = otpcode;
                    existingOtp.CreatedDate = DateTime.UtcNow;
                    existingOtp.ExpiryTime = DateTime.UtcNow.AddMinutes(2);
                    existingOtp.IsUsed = false;
                }
                await _context.SaveChangesAsync();
                await sm.SendMailAsync(mess);

            }

        }
        public async Task VerifyOtp(VerifyOtp OTP)
        {
            var emailotp = await _context.EmailOtps.Include(x => x.User).
                FirstOrDefaultAsync (x => x.Email == OTP.Email  );
            if (emailotp == null)
            {
                throw new Exception("OTP IS NOT VERIFIED");
            }
            if (emailotp.ExpiryTime < DateTime.UtcNow)
            {
                throw new Exception("otp has been expired");
            }
            if(emailotp.OtpCode != OTP.OtpCode)
            {
                throw new Exception("Otp does not match");
            }
            
           
           emailotp.User.IsEmailVerified = true;
            emailotp.IsUsed = true;
            await  _context.SaveChangesAsync();
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
                return null;
            }

            if (!login.IsEmailVerified)
            {
                throw new Exception("Please verify your email before logging in.");
            }
            
            return login;

        }
       


    }
}
