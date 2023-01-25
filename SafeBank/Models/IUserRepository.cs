namespace SafeBank.Models
{
    public interface IUserRepository
    {
        public User GetUserByAccountIBAN(string IBAN);
    }
}
