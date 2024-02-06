using System.ComponentModel.DataAnnotations;

namespace SimpleMarketplaceApp.Models
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
     
        public int Age { get; set; }
  
    }


}
