using Microsoft.AspNetCore.Mvc;
using SimpleMarketplaceApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering; 
using SimpleMarketplaceApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace SimpleMarketplaceApp.Controllers
{
    public class ItemsController : Controller
    {
       
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(ApplicationDbContext context, UserManager<User> userManager, ILogger<ItemsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult ListItem()
        {
            var categories = _context.Categories.ToList();
            _logger.LogInformation("Number of categories fetched: " + categories.Count);

            ViewBag.Categories = new SelectList(categories, "categoryId", "categoryName");
            _logger.LogInformation("Rendering ListItem view");
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateItem(Item item)
        {
            _logger.LogInformation("CreateItem POST method called");
            item.UserId = _userManager.GetUserId(User);
            ModelState.Remove("User");
            ModelState.Remove("Category");            
            if (ModelState.IsValid)
            {
                _logger.LogInformation("if block called");
                
                item.DateListed = DateTime.Now;
                _context.Items.Add(item);

                try
                {
                    _logger.LogInformation("try block called");
                    await _context.SaveChangesAsync();
                    TempData["ItemName"] = item.Title;
                    return View("ItemPending"); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while saving the item: " + ex.Message);
                    ModelState.AddModelError(string.Empty, "An error occurred while listing the item");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Model state is invalid. Errors: " + string.Join(", ", errors));

                foreach (var error in errors)
                {
                    _logger.LogInformation(error);
                }
            }
            _logger.LogInformation("the end block is called");
            ViewBag.Categories = new SelectList(_context.Categories, "categoryId", "categoryName");
            return View("ListItem", item);
        }


    }
}
