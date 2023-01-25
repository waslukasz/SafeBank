using Microsoft.EntityFrameworkCore;
using SafeBank.Data;

namespace SafeBank.Models
{
    public class AccountRepository : IAccountRepository
    {
        public ApplicationDbContext appDbContext;
        public AccountRepository(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public DbSet<Account> GetAccounts()
        {
            return appDbContext.Accounts;
        }

        public User GetAccountOwner(Account account)
        {
            return appDbContext.Users.FirstOrDefault(user => user.Id == account.OwnerId);
        }

        public Account GetAccountByIBAN(string IBAN)
        {
            return appDbContext.Accounts.FirstOrDefault(acc => acc.IBAN == IBAN);
        }

        public Account GetAccountByOwner(User owner)
        {
            return appDbContext.Accounts.First(acc => acc.OwnerId == owner.Id);
        }
    }
}
