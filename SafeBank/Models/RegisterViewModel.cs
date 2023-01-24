using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SafeBank.Models
{

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "This field is required!")]
        [RegularExpression("^([a-zA-Z]{1,30}\\s[a-zA-Z]{1,30}\\s[a-zA-Z]{1,30})|([a-zA-Z]{1,30}\\s[a-zA-Z]{1,30})$", ErrorMessage = "There may only be letters in your name.")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [EmailAddress(ErrorMessage = "Please try valid email address.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [RegularExpression("^\\d{11}$", ErrorMessage = "PESEL is made of 11 numbers.")]
        [Display(Name = "PESEL")]
        public string PESEL { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [DateMinimumAge(18)]
        [DisplayFormat(DataFormatString = "dd/mm/yyyy")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string? ReturnUrl { get; set; }
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
            if ((value != null && DateTime.TryParse(value.ToString(), out date)))
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
