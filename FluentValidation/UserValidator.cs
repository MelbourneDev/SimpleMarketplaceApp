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
            RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must contain at least 8 characters")
                .MaximumLength(16).WithMessage("Password must contain no more than 25 characters")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least 1 uppercase letter")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least 1 lowercase letter")
                .Matches(@"[\!\?\*\.]+").WithMessage("Password must contain at least 1 special character");
            RuleFor(user => user.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(user => user.Role).NotEmpty().Must(r => r == "User" || r == "Admin");
             







        }
    }
}
