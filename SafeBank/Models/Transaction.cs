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
        
        public string Sender { get; set; }
        
        public string Recipient { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
