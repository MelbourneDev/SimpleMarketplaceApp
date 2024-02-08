using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMarketplaceApp.Data;
using SimpleMarketplaceApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMarketplaceApp.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TransactionsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransaction(int itemId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var item = await _context.Items.FirstOrDefaultAsync(i => i.itemId == itemId);

            if (item == null || item.IsActive == false || item.UserId == currentUser.Id)
            {
                TempData["Error"] = "Invalid transaction attempt.";
                return RedirectToAction("ItemDetails", new { itemId = itemId });
            }

            var transaction = new Transaction
            {
                buyerId = currentUser.Id,
                sellerId = item.UserId,
                itemId = item.itemId,
                transactionAmount = item.ItemPrice,
                transactionDate = DateTime.Now
            };

            // Update item status
            item.IsActive = false;
            item.IsSold = true;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"You have successfully purchased {item.Title} from {transaction.sellerId}.";

            return RedirectToAction("TransactionDetails", new { transactionId = transaction.transactionID });
        }

        
        public async Task<IActionResult> TransactionDetails(int transactionId)
        {
            var transaction = await _context.Transactions
                                             .Include(t => t.Buyer)
                                             .Include(t => t.Seller)
                                             .Include(t => t.Item)
                                             .FirstOrDefaultAsync(t => t.transactionID == transactionId);

            var currentUser = await _userManager.GetUserAsync(User);
            if (transaction == null || (transaction.buyerId != currentUser.Id && transaction.sellerId != currentUser.Id))
            {
                return View("Error"); // Error handling
            }

            return View(transaction);
        }

        public async Task CompleteTransaction(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                item.IsSold = true;
                item.IsActive = false; 
                await _context.SaveChangesAsync();
                
            }
        }


        public async Task<IActionResult> ReinstateItem(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null && item.IsSold)
            {
                item.IsSold = false;
                item.IsActive = true; // Or set based on your application's logic
                await _context.SaveChangesAsync();
                // Redirect or return appropriate response
                return RedirectToAction("YourDashboardAction");
            }
            return NotFound();
        }

    }


}
