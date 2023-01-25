
using SafeBank.Data;

namespace SafeBank.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext appDbContext;

        public UserRepository(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public User GetUserByAccountIBAN(string IBAN)
        {
            var result = appDbContext.Users.FirstOrDefault(user => user.Id == appDbContext.Accounts.FirstOrDefault(acc => acc.IBAN == IBAN).OwnerId);
            return result;
        }
    }
}
