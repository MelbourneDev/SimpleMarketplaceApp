using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SimpleMarketplaceApp.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int transactionID { get; set; }

        [ForeignKey("Buyer")]
        public string buyerId { get; set; }

        [ForeignKey("Seller")]
        public string sellerId { get; set; }

        [ForeignKey("Item")]
        public int itemId { get; set; }

        public decimal transactionAmount { get; set; }

        public DateTime transactionDate { get; set; }

        // Navigation properties
        public virtual User Buyer { get; set; }
        public virtual User Seller { get; set; }
        public virtual Item Item { get; set; }
    }

}
