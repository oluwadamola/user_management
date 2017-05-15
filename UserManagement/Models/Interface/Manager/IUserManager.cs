using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Models.Entities;
using UserManagement.Models.ViewModel;

namespace UserManagement.Models.Interface.Manager
{
    public interface IUserManager
    {
        bool ValidateUser(string username, string password);

        User ValidateLogin(string username, string password);
        void SignUp(UserProfile userprofile);
        string Recover(string email);
        void ChangePassword(string username, string password, string newpassword);
        void CreateUser(User user);
        User GetUser(string username);
        User GetUser(int userID);        
        User[] GetUsers();
        void UpdateUser(User user);
        void RemoveUser(User user);

        void CreateRole(Role role);
        Role[] GetRoles(int userID);
        Role[] GetRoles();
        Permission[] GetPermissions();
        Permission GetPermission(int PermissionId);
        void AssignRole(int userId, string role);
        bool HasRole(User user, string role);
        void RemoveRole(User user, string role);
        Role GetRole(int RoleId);
        Role GetRoleByName(string RoleName);
        Permission[] GetPermissions(string userID);
        //bool HasPermission(User user, string permission);
        bool HasPermission(Role role, string permission);

        Permission GetPermissionByName(string PermissionName);
        void CreatePermission(Permission permission);
        void AssignPermissionToRole(int roleId, string permission);
    }
}
