using FluentValidation;
using SimpleMarketplaceApp.Models;

namespace SimpleMarketplaceApp.FluentValidation
{
    public class ItemImagesValidator : AbstractValidator<ItemImage>
    {
        public ItemImagesValidator() 
        
        {
            RuleFor(itemimages => itemimages.Title).MaximumLength(50).WithMessage("No more than 50 characters per image caption");       
        
        
        
        }
    }
}
