using System.ComponentModel.DataAnnotations;

namespace SafeBank.Models.ViewModels
{
    public class SettingsViewModel
    {
        [Required(ErrorMessage = "You need to fill this field to save changes.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "There may only be letters in your name.")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Please try valid email address.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [DateMinimumAge(18)]
        [DisplayFormat(DataFormatString = "dd/mm/yyyy")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string? NewPassword { get; set; }
    }
}
