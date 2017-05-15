using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models.Entities
{
    public class BaseEntity
    {
        public DateTime CreateOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }

    }
}