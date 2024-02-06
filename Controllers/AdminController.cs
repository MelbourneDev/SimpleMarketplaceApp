using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMarketplaceApp.Data;
using SimpleMarketplaceApp.Models;
using SimpleMarketplaceApp.Services.Item;

namespace SimpleMarketplaceApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IItemService _itemService;


        public AdminController(ApplicationDbContext context, IItemService itemService)
        {
            _context = context;
            _itemService = itemService;
        }
                  
           

        public async Task<IActionResult>AdminDashboard()
        {

            var pendingItems = await _context.Items.Where(i => i.Status == Models.ApprovalStatus.Pending)
                .ToListAsync();

            var approvedItems = await _context.Items.Where(i => i.Status == Models.ApprovalStatus.Approved) .ToListAsync();

            var rejectedItems = await _context.Items.Where(i => i.Status == Models.ApprovalStatus.Rejected) .ToListAsync();

            var model = new DashboardViewModel

            {
                PendingItems = pendingItems,
                ApprovedItems = approvedItems,
                RejectedItems = rejectedItems
            };

            return View(model);
        }


        public async Task<IActionResult> ApproveItem(int itemId, string activeTab)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                item.Status = ApprovalStatus.Approved;
                item.IsActive = true;
                await _context.SaveChangesAsync();
                

            }
            TempData["ActiveTab"] = activeTab;
            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> RejectItem(int itemId, string activeTab)
        {
            // Assuming isAdminAction is true for an admin rejecting an item
            var success = await _itemService.UpdateItemStatus(itemId, null, false, true);
            if (success)
            {
                TempData["Message"] = "Item successfully rejected.";
            }
            else
            {
                TempData["Error"] = "Failed to reject item.";
            }
            TempData["ActiveTab"] = activeTab;
            return RedirectToAction("AdminDashboard");
        }


        public async Task<IActionResult> PullItem(int itemId, string activeTab)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                item.Status = ApprovalStatus.Pending;
                item.IsActive = false;
                await _context.SaveChangesAsync();
                
            }
            TempData["ActiveTab"] = activeTab;
            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> ReinstateItem(int itemId, string activeTab)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                item.Status = ApprovalStatus.Approved;
                await _context.SaveChangesAsync();
                item.IsActive = true;
            }
            TempData["ActiveTab"] = activeTab;
            return RedirectToAction("AdminDashboard");
        }

    }
}
