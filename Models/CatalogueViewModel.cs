using System.Collections.Generic;
namespace SimpleMarketplaceApp.Models
{
    public class CatalogueViewModel
    {

        public List<Category> Categories { get; set; }
        public List<Item> Items { get; set; }
        public string SelectedCategoryName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
