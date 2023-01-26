using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using SafeBank.Data;

namespace SafeBank.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext appDbContext;

        public NotificationController(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult Delete(int id, string? returnUrl)
        {
            var noti = appDbContext.Notifications.FirstOrDefault(noti => noti.Id == id);
            appDbContext.Notifications.Remove(noti);
            appDbContext.SaveChanges();

            return LocalRedirect(returnUrl);
        }
    }
}
