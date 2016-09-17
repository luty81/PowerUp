﻿using Microsoft.AspNet.Identity;
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
    public class ApplicationRoleManager: RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore) { }

        public static ApplicationRoleManager Create<TDbContext>(
            IdentityFactoryOptions<ApplicationRoleManager> options, 
            IOwinContext owinContext) where TDbContext : IdentityDbContext
        {
            var owin = owinContext.Get<TDbContext>();
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(owin));
        }
    }
}
