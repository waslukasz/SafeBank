using Microsoft.EntityFrameworkCore;
using SafeBank.Data;

namespace SafeBank.Models
{
    public class TransactionRepository : ITransactionRepository
    {
        private ApplicationDbContext appDbContext;
        private AccountRepository accountRepository;
        private UserRepository userRepository;

        public TransactionRepository(AccountRepository accountRepository, UserRepository userRepository, ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
        }

        public void RegisterTransaction(Transaction transaction)
        {
            var senderAccount = accountRepository.GetAccountByIBAN(transaction.Sender);
            var recipientAccount = accountRepository.GetAccountByIBAN(transaction.Recipient);
            senderAccount.Balance -= transaction.Amount;
            recipientAccount.Balance += transaction.Amount;

            appDbContext.Transactions.Add(transaction);
            appDbContext.SaveChanges();
        }

        public void RevertTransaction(Transaction transaction)
        {
            var senderAccount = accountRepository.GetAccountByIBAN(transaction.Sender);
            var recipientAccount = accountRepository.GetAccountByIBAN(transaction.Recipient);
            senderAccount.Balance += transaction.Amount;
            recipientAccount.Balance -= transaction.Amount;

            appDbContext.Transactions.Remove(transaction);
            appDbContext.SaveChanges();
        }

        public List<Transaction> GetTransactionsByAccount(Account account)
        {
            return appDbContext.Transactions.Include(tran => tran.Sender).Include(tran => tran.Recipient).Where(tran => tran.Sender == account.IBAN || tran.Recipient == account.IBAN).ToList();
        }

        public IQueryable<Transaction> GetTransactionsById(int id)
        {
            return appDbContext.Transactions.Include(tran => tran.Sender).Include(tran => tran.Recipient).Where(tran => tran.Id == id);
        }

        public void DeleteTransaction(Transaction transaction)
        {
            appDbContext.Transactions.Remove(transaction);
            appDbContext.SaveChanges();
        }
    }
}
