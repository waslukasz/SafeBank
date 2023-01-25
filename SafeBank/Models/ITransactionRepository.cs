namespace SafeBank.Models
{
    public interface ITransactionRepository
    {
        void RegisterTransaction(Transaction transaction);
        
    }
}
