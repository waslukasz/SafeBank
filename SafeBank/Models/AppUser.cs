using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SafeBank.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [ProtectedPersonalData]
        public string PESEL { get; set; }

        [ProtectedPersonalData]
        public DateTime DateOfBirth { get; set; }
    }
}
