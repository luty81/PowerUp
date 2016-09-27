using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.Mvc.Security.Identity
{
    public static class AuthStarter
    {
        public static void SetOwinUp<TDbContext>(this IAppBuilder app, TDbContext dbContext = null) 
            where TDbContext : IdentityDbContext<ApplicationUser>, new()
        {
            app.CreatePerOwinContext(() => dbContext ?? new TDbContext());
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create<TDbContext>);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);


            Func<string, string> AppAuthSetting = name => ConfigurationManager.AppSettings["AuthStarter:" + name].ToString();

            var facebookOptions = new FacebookAuthenticationOptions
            {
                AppId = AppAuthSetting("Facebook:AppId"),
                AppSecret = AppAuthSetting("Facebook:AppSecret"),
            };
            facebookOptions.Scope.Add("public_profile");
            facebookOptions.Scope.Add("email");
            app.UseFacebookAuthentication(facebookOptions);

            app.UseGoogleAuthentication(AppAuthSetting("Google:ClientId"), AppAuthSetting("Google:ClientSecret"));


            //app.UseTwitterAuthentication(
            //   consumerKey: "OJQVnrGZHIMifg4cJHKDXSO7l",
            //   consumerSecret: "9RPP4tE2RO1TyLERVxGD4QeGbEBYfwVY2i7RRrs8lvBE3mUoaw");
        }
    }
    
}
