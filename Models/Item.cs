using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public string? UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }

        [Required]
        public int ItemPrice { get; set; }

        public DateTime DateListed { get; set; }

        public bool IsPastListing { get; set; } = false;

        public bool IsActive { get; set; } = false;

        // Navigation properties

        public virtual User User { get; set; }
       
        public virtual Category Category { get; set; }
        public virtual ICollection<ItemImage>? ItemImages { get; set; }

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending; // Default
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
