using FluentValidation;
using SimpleMarketplaceApp.Models;

namespace SimpleMarketplaceApp.FluentValidation
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            RuleFor(item => item.Title).NotEmpty().WithMessage("Title is required for listing.");
            RuleFor(item => item.Description).NotEmpty().MinimumLength(20).MaximumLength(200).WithMessage("Description required for listing");
            
        
        }

    }
}
