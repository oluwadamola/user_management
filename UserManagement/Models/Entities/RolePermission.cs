﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models.Entities
{
    public class RolePermission : BaseEntity
    {
        public int RolePermissionId { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public Permission Permission { get; set; }
        public int PermissionId { get; set; }

    }
}