using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using SafeBank.Data;
using SafeBank.Models;
using SafeBank.Models.ViewModels;
using System;

namespace SafeBank.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext appDbContext;
        private readonly AccountRepository accountRepository;
        private readonly TransactionRepository transactionRepository;
        private readonly NotificationRepository notificationRepository;
        private readonly UserRepository userRepository;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AccountRepository accountRepository, NotificationRepository notificationRepository, TransactionRepository transactionRepository, SignInManager<User> signInManager, UserRepository userRepository, ApplicationDbContext appDbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.notificationRepository = notificationRepository;
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.userRepository = userRepository;
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Wrong username or password.");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                DateOfBirth = new DateTime(day: 1, month: 1, year: DateTime.Today.Year - 18)
            };
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FullName = registerViewModel.FullName,
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    DateOfBirth = registerViewModel.DateOfBirth
                };
                var result = await userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    Random rand = new Random();
                    while (true)
                    {
                        string gen = rand.Next(10000, 99999).ToString();

                        if (appDbContext.Accounts.Any(acc => acc.IBAN == gen)) continue;

                        Account account = new Account()
                        {
                            IBAN = gen,
                            OwnerId = user.Id,
                            Balance = 0
                        };
                        appDbContext.Accounts.Add(account);
                        appDbContext.SaveChanges();
                        break;
                    }
                    return RedirectToAction("Index", "Account");
                }
                ModelState.AddModelError("Email", "User could not be created. Email already taken.");
                return View(registerViewModel);
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");
            SettingsViewModel settingsViewModel = new SettingsViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
            };

            return View(settingsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(SettingsViewModel settingsViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null) { return RedirectToAction("Index", "Home"); }
                if (userManager.CheckPasswordAsync(user, settingsViewModel.CurrentPassword).Result)
                {
                    user.FullName = settingsViewModel.FullName;
                    user.Email = settingsViewModel.Email;
                    user.DateOfBirth = settingsViewModel.DateOfBirth;
                    await userManager.SetUserNameAsync(user, settingsViewModel.Email);
                    appDbContext.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is wrong.");
                }
                return RedirectToAction("Index", "Account");
            }
            return View(settingsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel();
            paymentViewModel.SenderAccountNo = accountRepository.GetAccountByOwner(userManager.GetUserAsync(User).Result).IBAN;
            return View(paymentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(PaymentViewModel paymentViewModel)
        {
            var sender = accountRepository.GetAccountByOwner(userManager.GetUserAsync(User).Result);
            paymentViewModel.SenderAccountNo = sender.IBAN;
            var recipient = accountRepository.GetAccounts().FirstOrDefault(acc => acc.IBAN == paymentViewModel.RecipientAccountNo);
            if (recipient == null)
            {
                ModelState.AddModelError("RecipientAccountNo", "This account number does not exist.");
                return View(paymentViewModel);
            }
            
            if (recipient == sender)
            {
                ModelState.AddModelError("RecipientAccountNo", "You can't send money to yourself!");
                return View(paymentViewModel);
            }

            if (paymentViewModel.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Amount must be positive.");
            }

            if (paymentViewModel.Amount > sender.Balance)
            {
                ModelState.AddModelError("Amount", $"You don't have enough funds. Your balance: {sender.Balance}.");
            }

            if (!ModelState.IsValid) return View(paymentViewModel);

            var transaction = new Transaction()
            {
                Sender = sender.IBAN,
                Recipient = recipient.IBAN,
                Title = paymentViewModel.Title,
                Amount = paymentViewModel.Amount,
                Date = DateTime.Now
            };

            transactionRepository.RegisterTransaction(transaction);
            notificationRepository.CreateFromTransaction(transaction);

            return View("PaymentSuccess", transaction);
        }

        [HttpGet]
        public IActionResult History()
        {
            var currentAccount = accountRepository.GetAccountByOwner(userManager.GetUserAsync(User).Result);
            var result = appDbContext.Transactions.Where(tran => tran.Sender == currentAccount.IBAN || tran.Recipient == currentAccount.IBAN).OrderByDescending(res => res.Date).ToList();
            return View(result);
        }
    }
}
