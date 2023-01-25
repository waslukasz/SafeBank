using Microsoft.EntityFrameworkCore;
using SafeBank.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeBank.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public AppUser Sender { get; set; }

        public AppUser Recipient { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
