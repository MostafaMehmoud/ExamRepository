using Exam.BL.Services.IServices;
using Exam.DAL.Dtos;
using Exam.DAL.Entities;
using Exam.DAL.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new RegisterDto
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password
                };

                var result = await _authService.RegisterAsync(dto);

                if (result)
                    return RedirectToAction("Index", "Home", new { area = "" });


                ModelState.AddModelError("ConfirmPassword", "فشل تسجيل الحساب.");
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dto = new LoginDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                };

                var result = await _authService.LoginAsync(dto);

                if (result)
                    return RedirectToAction("Index", "Home", new { area = "" });


                ModelState.AddModelError("", "فشل تسجيل الدخول.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });

        }
    }
}
