using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeBank.Models;

namespace SafeBank.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if(result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError("InvalidLogin", "Invalid login attempt.");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.DateOfBirth = new DateTime(day: 1, month: 1, year: DateTime.Today.Year);
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl)
        {
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");

            if(ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    FullName = registerViewModel.FullName,
                    PESEL = registerViewModel.PESEL,
                    DateOfBirth = registerViewModel.DateOfBirth
                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if(result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError("Password", "User could not be created. Password not unique enough.");
            }
            return View(registerViewModel);
        }
    }
}
