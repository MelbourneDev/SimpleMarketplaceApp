using FluentValidation;
using SimpleMarketplaceApp.Models;

namespace SimpleMarketplaceApp.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()

        {
            RuleFor(user => user.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(user => user.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(user => user.Age).NotNull().GreaterThanOrEqualTo(16).WithMessage("Users must be at least 16 years of age to sign up to an account.");




        }
    }
}
