using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMarketplaceApp.Models
{
    public class EditListingViewModel
    {
        public int ItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? CategoryId { get; set; } 
    }
}
