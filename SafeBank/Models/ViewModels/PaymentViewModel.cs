using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SafeBank.Models.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sender")]
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account number contains numbers only.")]
        public string SenderAccountNo { get; set; }

        [Display(Name = "Recipient")]
        [Required(ErrorMessage = "Please enter account number.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account number contains numbers only.")]
        public string RecipientAccountNo { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Payment Title")]
        [MinLength(5, ErrorMessage = "Minimum title lenght is 5 characters.")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Date of transaction")]
        public DateTime Date { get; set; }
    }
}
