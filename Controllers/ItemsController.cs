using Microsoft.AspNetCore.Mvc;
using SimpleMarketplaceApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering; 
using SimpleMarketplaceApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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


        // Upon page create, fetch the categories to populate the dropdown for the user form.
        [HttpGet]
        public IActionResult ListItem()
        {
            var categories = _context.Categories.ToList();
            _logger.LogInformation("Number of categories fetched: " + categories.Count);

            ViewBag.Categories = new SelectList(categories, "categoryId", "categoryName");
            _logger.LogInformation("Rendering ListItem view");
            return View();
        }


        // User creates a listing, referencing the relative form in ListItem.cshtml
        // action has image validation for BLOB storage of files. 

        [HttpPost]
        public async Task<IActionResult> CreateItem(Item item, List<IFormFile> imageFiles)
        {
            _logger.LogInformation("CreateItem POST method called");
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            item.UserId = _userManager.GetUserId(User);

            ModelState.Remove("User");
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Model state is valid. Processing {imageFiles.Count} files.");

                if (imageFiles != null && imageFiles.Count > 0)
                {
                    foreach (var file in imageFiles)
                    {
                        if (file.Length > 0)
                        {
                            _logger.LogInformation($"Processing file: {file.FileName} with size {file.Length} bytes.");

                            using var memoryStream = new MemoryStream();
                            await file.CopyToAsync(memoryStream);
                            var imageData = memoryStream.ToArray();

                            var image = new ItemImage
                            {
                                ImageData = imageData,
                                ImageMimeType = file.ContentType,
                            };

                            item.ItemImages.Add(image);
                        }
                        else
                        {
                            _logger.LogWarning($"Skipped file: {file.FileName} because it was empty.");
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("No files were uploaded.");
                }

                item.DateListed = DateTime.Now;
                _context.Items.Add(item);

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["ItemName"] = item.Title;
                    return RedirectToAction("ItemPending");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError($"An error occurred while saving the item: {ex.InnerException?.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while listing the item.");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning($"Model state is invalid. Errors: {string.Join(", ", errors)}");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "categoryId", "categoryName");
            return View("ListItem", item);
        }



        // Action for RedirecToAction when successfully completeing ListItem form.
        public IActionResult ItemPending()
        {
            return View();
        }



        public async Task<IActionResult> GetItemImage(int imageId)
        {
            var image = await _context.ItemImages.FindAsync(imageId);
            if (image != null && image.ImageData != null)
            {
                return File(image.ImageData, image.ImageMimeType);
            }
            else
            {
                
                return NotFound();
            }
        }

    }
}
