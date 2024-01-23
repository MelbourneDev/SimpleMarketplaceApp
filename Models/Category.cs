using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleMarketplaceApp.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int categoryId { get; set; }

        [Required]
        public string categoryName { get; set; }
        [Required]
        public string  categoryDescription { get; set; }
    }
}
