using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.WebRequestMethods;

namespace SimpleMarketplaceApp.Models
{
    public class ItemImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int imageID { get; set; }

        [ForeignKey("itemId")]
        public int itemID { get; set; } // FK to Item

     
        // Property to store the binary data of the image
        public byte[] ImageData { get; set; }

        // Property to store the MIME type of the image
        public string ImageMimeType { get; set; }

        // Navigation property to the Item model
        public virtual Item Item { get; set; }
    }

}
