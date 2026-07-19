using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;
using System.Security.Claims;

namespace MVCDotnetCore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IAccountService AccountService;

        public AccountController(IAccountService _AccountService)
        {
            AccountService = _AccountService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                await AccountService.Register(model);
                return RedirectToAction("VerifyOtp");
            }
                return View(model);
            
        }
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtp model)
        {
            if (ModelState.IsValid)
            {
                await AccountService.VerifyOtp(model);
                return View();
            }
            return View(model);


        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await AccountService.Login(model);

            var claims = new List<Claim>
             {
                   new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                   new Claim(ClaimTypes.Email,user.Email),
                   new Claim(ClaimTypes.Role, user.Role)
             };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction("Index", "Home");
        }
       
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");

        }
    }
}
