using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models.Entities
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }


    }
}