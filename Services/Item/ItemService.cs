using Microsoft.EntityFrameworkCore;
using SimpleMarketplaceApp.Data; // Assuming this is where ApplicationDbContext is located
using SimpleMarketplaceApp.Models;
using System.Threading.Tasks;

namespace SimpleMarketplaceApp.Services.Item
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;

        public ItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateItemStatus(int itemId, string userId, bool isActive, bool isAdminAction = false)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                return false; // Item not found
            }

            // Logic to handle the update based on isAdminAction and userId
            if (isAdminAction)
            {
                // Admin actions might bypass certain checks
                item.IsActive = isActive;
            }
            else if (item.UserId == userId)
            {
                // Ensure that the action is performed by the item's owner
                item.IsActive = isActive;
            }
            else
            {
                return false; // Unauthorized or mismatched user action
            }

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> MarkAsPastListing(int itemId, string userId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.itemId == itemId && i.UserId == userId);
            if (item == null) return false;

            item.IsPastListing = true;
            item.IsActive = false; // Assuming you want to deactivate it as well
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReinstateListing(int itemId, string userId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.itemId == itemId && i.UserId == userId);
            if (item == null)
            {
                return false; // Item not found or does not belong to the user
            }

            // Move the item back to current listings without changing its active status
            item.IsPastListing = false;
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
