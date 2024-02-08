using SimpleMarketplaceApp.Models;

namespace SimpleMarketplaceApp.Models
{
    public class UserDashboardViewModel
    {
        public IEnumerable<Item> CurrentListings { get; set; }
        public IEnumerable<Item> PastListings { get; set; }
        public IEnumerable<SoldItemViewModel> SoldListings { get; set; }

    }


    public class SoldItemViewModel
    {
        public Item Item { get; set; }
        public string BuyerUsername { get; set; }
    }

}