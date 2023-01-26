using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SafeBank.Models.ViewModels
{

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "This field is required!")]
        [RegularExpression("^([a-zA-Z]{1,30}\\s[a-zA-Z]{1,30}\\s[a-zA-Z]{1,30})|([a-zA-Z]{1,30}\\s[a-zA-Z]{1,30})$", ErrorMessage = "There may only be letters in your name. Format: Firstname Surname")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [EmailAddress(ErrorMessage = "Please try valid email address.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [DateMinimumAge(18)]
        [DisplayFormat(DataFormatString = "dd/mm/yyyy")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[*.!@$%^&(){}[]:;<>,.?/~_+-=|\\]).{6,32}$", ErrorMessage = "Password should have at least 6 characters, 1 number, 1 uppercase, 1 lowercase and 1 special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class DateMinimumAgeAttribute : ValidationAttribute
    {
        public DateMinimumAgeAttribute(int minimumAge)
        {
            MinimumAge = minimumAge;
            ErrorMessage = "You must be someone at least {1} years of age.";
        }

        public override bool IsValid(object value)
        {
            DateTime date;
            if (value != null && DateTime.TryParse(value.ToString(), out date))
            {
                return date.AddYears(MinimumAge) < DateTime.Now;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, MinimumAge);
        }

        public int MinimumAge { get; }
    }
}
