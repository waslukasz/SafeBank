using Microsoft.EntityFrameworkCore;

namespace SafeBank.Models
{
    public interface IAccountRepository
    {
        public DbSet<Account> GetAccounts();
        public Account GetAccountByOwner(User owner);
    }
}
