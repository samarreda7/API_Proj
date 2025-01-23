
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Projectapi.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        

    }
}
