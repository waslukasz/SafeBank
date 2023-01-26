using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SafeBank.Data;

namespace SafeBank.Models
{
    public class NotificationRepository : INotificationRepository
    {
        private ApplicationDbContext appDbContext;
        private UserManager<User> userManager;
        private UserRepository userRepository;

        public NotificationRepository(UserManager<User> userManager, UserRepository userRepository, ApplicationDbContext appDbContext)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.appDbContext = appDbContext;
        }

        public DbSet<Notification> GetNotifications()
        {
            return appDbContext.Notifications;
        }

        public List<Notification> GetNotificationsByUser(User user)
        {
            return appDbContext.Notifications.Include(noti => noti.Transaction).Where(noti => noti.Target == user.Id).ToList();
        }

        public void CreateFromTransaction(Transaction transaction)
        {
            appDbContext.Notifications.Add(new Notification()
            {
                Target = userRepository.GetUserByAccountIBAN(transaction.Sender).Id,
                Transaction = transaction
            });

            appDbContext.Notifications.Add(new Notification()
            {
                Target = userRepository.GetUserByAccountIBAN(transaction.Recipient).Id,
                Transaction = transaction
            });

            appDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var notification = appDbContext.Notifications.First(x => x.Id == id);
            appDbContext.Notifications.Remove(notification);
        }
    }
}
