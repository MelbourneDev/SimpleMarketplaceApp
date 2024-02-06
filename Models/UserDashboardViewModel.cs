namespace SimpleMarketplaceApp.Models
{
    public class UserDashboardViewModel
    {
        public IEnumerable<Item> CurrentListings { get; set; }
        public IEnumerable<Item> PastListings { get; set; }
    }


}
