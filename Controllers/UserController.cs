using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMarketplaceApp.Models;
using System.Linq;
using System.Threading.Tasks;
using SimpleMarketplaceApp.Data;
using System.Diagnostics;
using SimpleMarketplaceApp.Services.Item;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;


namespace SimpleMarketplaceApp.Controllers
{
    [Authorize(Roles = "User")] // Ensure only authenticated users can access these actions
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ItemsController> _logger;
        private readonly IItemService _itemService;

        public UserController(ApplicationDbContext context, UserManager<User> userManager, ILogger<ItemsController> logger, IItemService itemService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _itemService = itemService;
        }


        public async Task<IActionResult> UserDashboard()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                // Handle user not found
                return RedirectToAction("Login", "Account");
            }

            var userId = _userManager.GetUserId(User);

            var items = _context.Items.Include(item => item.Category).ToList();
            var userItems = await _context.Items
                .Where(item => item.UserId == currentUser.Id)
                .ToListAsync();

            var soldItems = await _context.Transactions
                .Include(t => t.Item)
                .Include(t => t.Buyer)
                .Where(t => t.sellerId == userId && t.Item.IsSold)
                .Select(t => new SoldItemViewModel
                {
                    Item = t.Item,
                    BuyerUsername = t.Buyer.UserName 
                })
        .ToListAsync();

            var model = new UserDashboardViewModel
            {
                CurrentListings = userItems.Where(i => !i.IsPastListing && !i.IsSold), 
                PastListings = userItems.Where(i => i.IsPastListing),
                SoldListings = soldItems,
            };

            return View(model);
        }



        public async Task<IActionResult> RemoveListing(int itemId)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _itemService.MarkAsPastListing(itemId, user.Id);

            if (result)
            {
                TempData["Message"] = "Listing moved to past listings.";
            }
            else
            {
                TempData["Error"] = "Failed to remove listing.";
            }
            return RedirectToAction(nameof(UserDashboard));
        }

        public async Task<IActionResult> ReinstateListing(int itemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.itemId == itemId && i.UserId == _userManager.GetUserId(User));
            if (item != null)
            {
                // Check if the item was approved before being removed
                item.IsActive = item.Status == ApprovalStatus.Approved;
                item.IsPastListing = false; // Move back to current listings

                await _context.SaveChangesAsync();
                TempData["Success"] = "Listing reinstated successfully.";
            }
            else
            {
                TempData["Error"] = "Listing not found.";
            }
            return RedirectToAction("UserDashboard");
        }


        public async Task<IActionResult> EditListing(int itemId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var item = await _context.Items
                                     .Where(i => i.itemId == itemId && i.UserId == currentUser.Id)
                                     .FirstOrDefaultAsync();

            if (item == null)
            {
                TempData["Error"] = "Listing not found.";
                return RedirectToAction("UserDashboard");
            }

            var categories = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "categoryId", "categoryName", item.CategoryId);

            var model = new EditListingViewModel
            {
                ItemId = item.itemId,
                Title = item.Title,
                Description = item.Description,
                Price = item.ItemPrice,
                CategoryId = item.CategoryId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitEdit(EditListingViewModel model)
        {
            var item = await _context.Items.FindAsync(model.ItemId);

            if (item == null || item.UserId != _userManager.GetUserId(User))
            {
                TempData["Error"] = "Listing not found or unauthorized.";
                return RedirectToAction("UserDashboard");
            }

            item.Title = model.Title;
            item.Description = model.Description;
            item.ItemPrice = model.Price;
            item.CategoryId = model.CategoryId; // Update CategoryId

            item.Status = ApprovalStatus.Pending;
            item.IsActive = false;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Listing updated successfully and is pending review.";
            return RedirectToAction("UserDashboard");
        }



        public async Task<IActionResult> PermanentlyDeleteListing(int itemId, string activeTab)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.itemId == itemId && i.UserId == _userManager.GetUserId(User));
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            TempData["ActiveTab"] = activeTab;
            return RedirectToAction("UserDashboard");
        }





    }
}