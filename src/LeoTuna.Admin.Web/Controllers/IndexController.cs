using LeoTuna.Admin.Web.Dtos;
using LeoTuna.Admin.Web.Models;
using LeoTuna.Admin.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LeoTuna.Admin.Web.Controllers
{
    public class IndexController : Controller
    {
        private SignInManager<User> SignInManager { get; }
        private UserManager<User> UserManager { get; }
        private EmailService EmailService { get; }

        public IndexController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            EmailService emailService)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            EmailService = emailService;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }
            return RedirectToAction(nameof(SignIn));
        }

        [HttpGet("/sign-in")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("/sign-in")]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            var result = await SignInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet("/sign-up")]
        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost("/sign-up")]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            var user = new User { FullName = model.FullName, UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(Index));
            }
            throw new NotImplementedException();
        }

        [HttpGet("/log-out")]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        [HttpGet("/recover-password")]
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost("/recover-password")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDto model)
        {
            var user = await UserManager.FindByNameAsync(model.Email);

            if (user is null)
            {
                return View();
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user);

            try
            {
                EmailService.SendPasswordRecovery(code, user.Email);
            }
            catch
            {
                throw new NotImplementedException();
            }

            return View();
        }

        [HttpGet("/recover-password/confirm")]
        public IActionResult ConfirmPasswordRecovery([FromQuery] string id, [FromQuery] string email)
        {
            return View(new ConfirmPasswordRecoveryDto { Id = id, Email = email });
        }

        [HttpPost("/recover-password/confirm")]
        public async Task<IActionResult> ConfirmPasswordRecovery([FromQuery] string id, [FromQuery] string email, ConfirmPasswordRecoveryDto model)
        {
            var user = await UserManager.FindByNameAsync(email);

            var result = await UserManager.ResetPasswordAsync(user, id, model.Password);

            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(RecoverPassword));
            }

            return RedirectToAction(nameof(SignIn));
        }
    }
}
