using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.Database;
using RehabCV.Models;
using RehabCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Ви не підтвердили свою електронну пошту!");
                    return View(model);
                }

                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Невірний логін або пароль");
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var userWithSameEmail = model.Email == null ? await _userManager.FindByNameAsync(model.Login)
                                                        : await _userManager.FindByEmailAsync(model.Email);

            if (ModelState.IsValid)
            {
                if (userWithSameEmail == null)
                {
                    var user = new User
                    {
                        UserName = model.Email ?? model.Login,
                        FirstName = model.FirstNameOfUser,
                        MiddleName = model.MiddleNameOfUser,
                        LastName = model.LastNameOfUser,
                        Email = model.Email,
                        Phone = model.PhoneNumber
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Parent");

                        if (model.Email != null)
                        {
                            // установка куки
                            await _signInManager.SignInAsync(user, false);
                            // генерация токена для пользователя
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                            var callbackUrl = Url.Action(
                                "ConfirmEmail",
                                "Account",
                                new { userId = user.Id, code },
                                protocol: HttpContext.Request.Scheme);
                            EmailService emailService = new EmailService();
                            await emailService.SendEmailAsync(model.Email, "Confirm your account",
                                $"Підтвердіть реєстрацію, перейшовши за посиланням: <a href='{callbackUrl}'>link</a>");

                            return Content("Для завершення реєстрації перевірте електронну пошту та перейдіть за посиланням, зазначеної в листі");
                        }
                        else
                        {
                            user.EmailConfirmed = true;

                            await _userManager.UpdateAsync(user);

                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Користувач з даною електронною поштою/логіном вже існує");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
            else
                return View("Error");
        }
    }
}
