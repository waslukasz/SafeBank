using Microsoft.EntityFrameworkCore;

namespace SafeBank.Models
{
    public interface INotificationRepository
    {
        public DbSet<Notification> GetNotifications();
        public List<Notification> GetNotificationsByUser(User user);
        public void CreateFromTransaction(Transaction transaction);
        public void Delete(int id);
    }
}
