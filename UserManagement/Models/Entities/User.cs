using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace UserManagement.Models.Entities
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool ActiveStatus { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual Profile Profile { get; set; }
        
        public User()
        {
            //UserRoles = new HashSet<UserRole>();
        }
    }
}