using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeBank.Models
{
    public class AppUser : IdentityUser
    {
        public int AccountNumber { get; set; }

        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public decimal Balance { get; set; }

        [ProtectedPersonalData]
        public string PESEL { get; set; }

        [ProtectedPersonalData]
        public DateTime DateOfBirth { get; set; }
    }
}
