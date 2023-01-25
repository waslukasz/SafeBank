using Microsoft.AspNetCore.Identity;

namespace SafeBank.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [ProtectedPersonalData]
        public DateTime DateOfBirth { get; set; }
    }
}
