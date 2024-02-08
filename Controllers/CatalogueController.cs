using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMarketplaceApp.Data;
using SimpleMarketplaceApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Drawing.Printing;

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

        public async Task<IActionResult> CatalogueDashboard(int? selectedCategoryId = null, int pageNumber = 1, int pageSize = 9)
        {
            _logger.LogInformation("Loading CatalogueDashboard");

            // Fetch categories and add "All Items" category
            var categories = await _context.Categories.ToListAsync();
            var allItemsCategory = new Category { categoryId = 0, categoryName = "All Items" };
            categories.Insert(0, allItemsCategory);

            // Query items based on the selected category
            IQueryable<Item> itemsQuery = _context.Items
                                                  .Where(i => i.IsActive && i.Status == ApprovalStatus.Approved)
                                                  .Include(i => i.User)
                                                  .Include(i => i.ItemImages);
            if (selectedCategoryId.HasValue && selectedCategoryId > 0)
            {
                itemsQuery = itemsQuery.Where(i => i.CategoryId == selectedCategoryId);
            }

            // Calculate total items for pagination
            var totalItems = await itemsQuery.CountAsync();
            var items = await itemsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var model = new CatalogueViewModel
            {
                Categories = categories,
                Items = items,
                SelectedCategoryName = categories.FirstOrDefault(c => c.categoryId == selectedCategoryId)?.categoryName ?? "All Items"
            };

            // Pass pagination data
            ViewBag.TotalItems = totalItems;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SelectedCategoryId = selectedCategoryId; // Pass the selected category ID to the view for highlighting or logic

            return View(model);
        }




        public async Task<IActionResult> GetItemsByCategory(int? categoryId)
        {
            _logger.LogInformation($"Loading items for category ID: {categoryId}");

                    if (!categoryId.HasValue)
                    {
                        _logger.LogWarning("Category ID was null, redirecting to a default view or handling the error.");
                
                        return RedirectToAction("ErrorView"); 
                    }           
            var items = await _context.Items
                                      .Include(i => i.ItemImages)
                                      .Include(i => i.User)
                                      .Where(i => i.CategoryId == categoryId && i.Status == ApprovalStatus.Approved && i.IsActive)
                                      .ToListAsync();

            return PartialView("_ItemsGrid", items);
        }

        public async Task<IActionResult> ItemDetails(int itemId)
        {
            var item = await _context.Items
                                     .Include(i => i.ItemImages)
                                     .Include(i => i.User) 
                                     .FirstOrDefaultAsync(i => i.itemId == itemId);

            if (item == null)
            {
              
                return NotFound();
            }

            return View(item);
        }



    }
}