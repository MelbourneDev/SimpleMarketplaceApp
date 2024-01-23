using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMarketplaceApp.Models
{
    public class ItemImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int imageID { get; set; }

        [ForeignKey("itemId")]
        public int itemID { get; set; } // FK to Item

        [Required]
        public string Title { get; set; } // This is just a regular property

        [Required]
        public string imageURL { get; set; }

        // Navigation property to the Item model
        [Required]
        public virtual Item Item { get; set; }
    }

}
