using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeBank.Data;
using SafeBank.Models;
using System.Transactions;

namespace SafeBank.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext appDbContext;
        private readonly UserManager<User> userManager;
        private readonly TransactionRepository transactionRepository;
        private readonly SignInManager<User> signInManager;

        public AdminController(ApplicationDbContext appDbContext, UserManager<User> userManager, TransactionRepository transactionRepository, SignInManager<User> signInManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.transactionRepository = transactionRepository;
            this.signInManager = signInManager;
        }

        public IActionResult UserManagement()
        {
            return View(appDbContext.Users.ToList());
        }

        public IActionResult TransactionManagement()
        {
            return View(appDbContext.Transactions.ToList());
        }

        [HttpPost]
        public IActionResult RevertTransaction(Models.Transaction transaction, string? returnUrl)
        {
            transactionRepository.RevertTransaction(transaction);
            return LocalRedirect(returnUrl);
        }
    }
}
