using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserManagement.Models.Entities
{
    public class Role : BaseEntity
    {
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public string Area { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

    }
}