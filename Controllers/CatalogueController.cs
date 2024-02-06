using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMarketplaceApp.Data;
using SimpleMarketplaceApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleMarketplaceApp.Controllers
{
    public class CatalogueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CatalogueController> _logger;

        public CatalogueController(ApplicationDbContext context, ILogger<CatalogueController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> CatalogueDashboard()
        {
            _logger.LogInformation("Loading CatalogueDashboard");
            var categories = await _context.Categories.ToListAsync();

            var items = await _context.Items
                                     .Where(i => i.IsActive && i.Status == ApprovalStatus.Approved)
                                     .Include(i => i.User)
                                     .ToListAsync();

            var model = new CatalogueViewModel
            {
                Categories = categories,
                Items = items
            };

            return View(model);
        }



        public async Task<IActionResult> GetItemsByCategory(int? categoryId)
        {
            _logger.LogInformation($"Loading items for category ID: {categoryId}");
            if (!categoryId.HasValue)
            {
                _logger.LogWarning("Category ID was null, redirecting to a default view or handling the error.");
                // Handle the case where categoryId is null, maybe redirect to an error page or a default listing.
                return RedirectToAction("ErrorView"); // Or however you wish to handle it.
            }

            // Assuming i.IsActive is a boolean indicating whether the item should be visible in listings.
            var items = await _context.Items
                                      .Include(i => i.ItemImages)
                                      .Include(i => i.User) // Assuming you want to include user details for each item.
                                      .Where(i => i.CategoryId == categoryId && i.Status == ApprovalStatus.Approved && i.IsActive)
                                      .ToListAsync();

            return PartialView("_ItemsGrid", items);
        }





    }
}