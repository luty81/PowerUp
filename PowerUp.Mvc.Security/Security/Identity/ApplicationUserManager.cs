using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerUp.Mvc.Security.Identity
{
    public class ApplicationUserManager
    {

        public static void Config<TDbContext>(UserManager<ApplicationUser> manager)
            where TDbContext : IdentityDbContext<ApplicationUser>
        {
            manager.Options.Password = new PasswordOptions
            {
                RequiredLength = 6
            };

            manager.Options.Lockout = new LockoutOptions
            {
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
                MaxFailedAccessAttempts = 5
            };

            manager.RegisterTokenProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>());
            manager.RegisterTokenProvider("Email Code", new EmailTokenProvider<ApplicationUser>());
        }
    }
}
