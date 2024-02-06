using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SimpleMarketplaceApp.Models
{

    // User entity representing all users that will participate in the application. 
    // For input validation logic please see UserValidator class in the FluentValidation folder.
    public class User : IdentityUser
    {
              
        public string? FirstName { get; set; }      
        public string? LastName { get; set; }       
        public int Age { get; set; }        



    }
}
