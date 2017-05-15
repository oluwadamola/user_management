using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models.ViewModel;
using UserManagement.Models.Entities;

namespace UserManagement.Models.DataAccess
{
    public class RoleSeed
    {
        public static void SeedRoles(DataEntities context)
        {
            context.Roles.AddOrUpdate(

                r => r.RoleName, new Role() { RoleName = AppRoles.Admin.ToString(), CreateOn = DateTime.Now },
                new Role() { RoleName = AppRoles.User.ToString(), CreateOn = DateTime.Now }
                
                );
        }

    }
}