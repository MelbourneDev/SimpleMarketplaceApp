namespace SimpleMarketplaceApp.Services.Item
{
    public interface IItemService
    {
        Task<bool> UpdateItemStatus(int itemId, string userId, bool isActive, bool isAdminAction = false);
        Task<bool> MarkAsPastListing(int itemId, string userId);
        Task<bool> ReinstateListing(int itemId, string userId);
    }
}


