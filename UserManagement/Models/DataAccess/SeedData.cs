using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserManagement.Models.Entities;

namespace UserManagement.Models.DataAccess
{
    public class SeedData
    {
        public static void CreateRoles(DataEntities db)
        {
            db.Roles.Add(new Role()  { RoleName = "Admin"  } );

        }
    }
}