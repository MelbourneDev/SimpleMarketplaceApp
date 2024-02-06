namespace SimpleMarketplaceApp.Models
{
    public class DashboardViewModel
    {

        public List<Item> PendingItems { get; set; }
        public List<Item> ApprovedItems { get; set; }
        public List<Item> RejectedItems { get; set; }
    }
}
