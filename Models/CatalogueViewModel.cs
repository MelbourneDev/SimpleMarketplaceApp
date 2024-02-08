using System.Collections.Generic;
namespace SimpleMarketplaceApp.Models
{
    public class CatalogueViewModel
    {

        public List<Category> Categories { get; set; }
        public List<Item> Items { get; set; }
        public string SelectedCategoryName { get; set; }
    }
}
