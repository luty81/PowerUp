using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PowerUp.Mvc.Security.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public async Task<IdentityResult> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            return await manager.CreateAsync(this);
        }
    }
}