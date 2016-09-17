using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.Mvc.Security.Identity
{
    public class ApplicationUserManager: UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> userStore)
            : base(userStore) { }

        public static ApplicationUserManager Create<TDbContext>(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
            where TDbContext : IdentityDbContext<ApplicationUser>
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<TDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator { RequiredLength = 6 };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. 
            // This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Seu código de segurança é {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Código de Segurança",
                BodyFormat = "Seu código de segurança é {0}"
            });
            manager.EmailService = new EmailIdentityService();
            manager.SmsService = new SmsIdentityService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if(dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public virtual async Task<IdentityResult>AddUserToRolesAsync(string userId, IEnumerable<string> roles)
        {
            var userRoleStore = Store as IUserRoleStore<ApplicationUser, string>;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid user ID");
            }

            var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
            foreach(var role in roles.Where(role => !userRoles.Contains(role)))
            {
                await userRoleStore.AddToRoleAsync(user, role).ConfigureAwait(false);
            }

            return await UpdateAsync(user).ConfigureAwait(false);
        }

        public virtual async Task<IdentityResult> RemoveUserFromRolesAsync(string userId, IList<string> roles)
        {
            var userRoleStore = (IUserRoleStore<ApplicationUser, string>)Store;

            var user = await FindByIdAsync(userId).ConfigureAwait(false);
            if(user == null)
            {
                throw new InvalidOperationException("Invalid user ID");
            }

            var userRoles = await userRoleStore.GetRolesAsync(user).ConfigureAwait(false);
            foreach(var role in roles.Where(userRoles.Contains))
            {
                await userRoleStore.RemoveFromRoleAsync(user, role).ConfigureAwait(false);
            }
            
            // Call update once when all roles are removed
            return await UpdateAsync(user).ConfigureAwait(false);
        }


    }
}
