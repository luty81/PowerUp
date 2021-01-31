using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin;

namespace PowerUp.Mvc.Security.Identity
{
    //public class ApplicationRoleManager: RoleManager<IdentityRole, IdentityDbContext>
    //{
    //    public ApplicationRoleManager(RoleStore<IdentityRole, IdentityDbContext> roleStore)
    //        : base(roleStore) { }

    //    public static ApplicationRoleManager Create<TDbContext>(
    //        IdentityFactoryOptions<ApplicationRoleManager> options, 
    //        IOwinContext owinContext) where TDbContext : IdentityDbContext
    //    {
    //        var owin = (IdentityDbContext)owinContext.Get<TDbContext>();
    //        return new ApplicationRoleManager(new RoleStore<IdentityRole, IdentityDbContext>(owin));
    //    }
    //}
}
