using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMarketplaceApp.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int itemId { get; set; }

        // Assuming the primary key of User is an int
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        // Make sure the type matches the primary key type in Category
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public DateTime DateListed { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ItemImage>? ItemImages { get; set; }

        public ApprovalStatus Status { get; set; }
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
