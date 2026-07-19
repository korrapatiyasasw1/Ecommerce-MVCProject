using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface IAccountService
    {
        Task Register(RegisterDTO model);
        Task VerifyOtp(VerifyOtp OTP);
         Task<User?> Login(LoginDTO model);
     //   Task Logout();
        //Task<User> GetProfile(int userId);

    }
}
