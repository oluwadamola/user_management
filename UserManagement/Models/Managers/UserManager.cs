using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserManagement.Models.Entities;
using UserManagement.Models.Interface.DataAccess;
using System.Data.Linq;
using UserManagement.Util;
using UserManagement.Models.Interface;
using UserManagement.Models.Interface.Manager;
using UserManagement.Models.ViewModel;

namespace UserManagement.Models.Managers
{
    public class UserManager : IUserManager
    {
        private IDataRepository _db;
        public UserManager(IDataRepository db)
        {
            _db = db;
        }
        public bool ValidateUser(string username, string password)
        {
            var user = from u in _db.Query<User>()
                       where u.Email == username && u.Password == password
                       select u;

            if (user.Any())
            {
                return true;
            }
            return false;
        }

        public User ValidateLogin(string username, string password)
        {
            var user = from u in _db.Query<User>(u => u.Profile)
                       where u.Email == username && u.Password == password
                       select u;

            if (user.Any())
            {
                return user.FirstOrDefault();
            }
            throw new Exception("Invalid UserName and Password");
        }

        public string Recover(string email)
        {
            User user = GetUser(email);
            EmailUtil.SendMail("admin@gmail.com", email, "recovered password", "please find below your password" + "\n" + user.Password);
            return user.Password;
        }

        public void ChangePassword(string username, string password, string newpassword)
        {
            if (ValidateUser(username, password))
            {
                User user = GetUser(username);
                user.Password = newpassword;
                _db.Update<User>(user);
                _db.SaveChange();
                return;
            }
            throw new Exception("UserName and PassWord is not Valid");
        }
        public User GetUser(string username)
        {
            return _db.Query<User>().Where(u => u.Email == username).FirstOrDefault();
        }

        public User[] GetUsers()
        {
            try
            {
                return _db.Query<User>().ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public User GetUser(int userID)
        {
            return _db.Query<User>().Where(u => u.UserId == userID).FirstOrDefault();
        }

        public void CreateUser(User user)
        {
            try
            {
                _db.Add<User>(user);
                _db.SaveChange();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void SignUp(UserProfile userprofile)
        {
            try
            {
                if (userprofile == null)
                    throw new Exception("Object is null");


                User user = new User();
                if (GetUser(userprofile.Email) != null)
                    throw new Exception("User already exist ");

                user.Email = userprofile.Email;
                user.Password = userprofile.Password;
                user.CreateOn = DateTime.Now;
                _db.Add<User>(user);

                Profile profile = new Profile();
                profile.FirstName = userprofile.FirstName;
                profile.Gender = userprofile.Gender;
                profile.PhoneNumber = userprofile.PhoneNumber;
                _db.Add<Profile>(profile);

                _db.SaveChange();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                if (user != null)
                {
                    _db.Update<User>(user);
                    _db.SaveChange();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveUser(User user)
        {
            try
            {
                if (user != null)
                {
                    _db.Delete<User>(user);
                    _db.SaveChange();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CreateRole(Role role)
        {
            try
            {
                role.CreateOn = DateTime.Now;
                _db.Add<Role>(role);
                _db.SaveChange();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Role[] GetRoles(string userID)
        {
            try
            {
                /*User user = (from u in _db.Query<User>()
                             where u.UserId == Int32.Parse(userID)
                             select u).FirstOrDefault();*/

                var roleids = (from r in _db.Query<UserRole>()
                               where r.UserId == Int32.Parse(userID)
                               select r.RoleId).ToList();

                Role[] roles = (from r in _db.Query<Role>()
                                where roleids.Contains(r.RoleId)
                                select r).ToArray();
                return roles;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Role[] GetRoles()
        {
            return _db.Query<Role>().ToArray();
        }
        public Permission[] GetPermissions()
        {
            return _db.Query<Permission>().ToArray();
        }

        public Role GetRole(int roleId)
        {
            return _db.Query<Role>().Where(r => r.RoleId == roleId).FirstOrDefault();
        }

        public void AssignRole(int userId, string role)
        {
            try
            {
                User user = GetUser(userId);
                if (user == null) throw new Exception("User does not exist");

                Role rol = GetRoleByName(role);
                if (rol == null) throw new Exception("role not found");

                if (HasRole(user,role))
                    throw new Exception("User already has this role : ");

                UserRole userrole = new UserRole();
                userrole.RoleId = rol.RoleId;
                userrole.UserId = user.UserId;
                userrole.CreateOn = DateTime.Now;
                
                    _db.Add<UserRole>(userrole);
                    _db.SaveChange();
            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool HasRole(User user, string role)
        {

            //Role rol = _db.Query<Role>(r => r.RoleName == role).FirstOrDefault();
            Role rol = _db.Query<Role>().FirstOrDefault(r => r.RoleName == role);

            return _db.Query<UserRole>()
             .Where(ur => ur.UserId == user.UserId)
             .Where(ur => ur.RoleId == rol.RoleId)
             .Any();
        }

        public void AssignPermissionToRole(int roleId, string permission)
        {
            Role role = GetRole(roleId);
            if (role == null) throw new Exception("role does not exist");

            Permission permit = GetPermissionByName(permission);
            if (permit == null) throw new Exception("permission does not exist");

            if (HasPermission(role, permission)) throw new Exception("Role has this Permission");

            RolePermission rolepermission = new RolePermission();
            rolepermission.PermissionId = permit.PermissionId;
            rolepermission.RoleId = role.RoleId;
            rolepermission.CreateOn = DateTime.Now;
            _db.Add<RolePermission>(rolepermission);
            _db.SaveChange();

        }
        public void RemoveRole(User user, string role)
        {
            try
            {
                Role rol = _db.Query<Role>(r => r.RoleName == role).FirstOrDefault();
                UserRole userrole = new UserRole();
                userrole.RoleId = rol.RoleId;
                userrole.UserId = user.UserId;
                if (!HasRole(user, role))
                {
                    _db.Delete<UserRole>(userrole);
                    _db.SaveChange();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Permission[] GetPermissions(string userID)
        {
            try
            {
                var roleids = (from r in _db.Query<UserRole>()
                               where r.UserId == Int32.Parse(userID)
                               select r.RoleId).ToList();

                var permissionids = (from rp in _db.Query<RolePermission>()
                                     where roleids.Contains(rp.RoleId)
                                     select rp.PermissionId);

                Permission[] permissions = (from p in _db.Query<Permission>()
                                            where permissionids.Contains(p.PermissionId)
                                            select p).ToArray();

                return permissions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       /* public bool HasPermission(User user, string permission)
        {
            try
            {
                var roleids = (from r in _db.Query<UserRole>()
                               where r.UserId == user.UserId
                               select r.RoleId).ToList();
                Permission permition = _db.Query<Permission>(p => p.Name == permission).FirstOrDefault();
                if (_db.Query<RolePermission>(rp => rp.PermissionId == permition.PermissionId && roleids.Contains(rp.RoleId)).Any())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }*/
        public bool HasPermission(Role role, string permission)
        {
            Permission permision =_db.Query<Permission>().Where(p => p.Name == permission).FirstOrDefault();

           return _db.Query<RolePermission>()
                .Where(rp => rp.RoleId == role.RoleId)
                .Where(rp => rp.PermissionId == permision.PermissionId).Any();
        }

        public Role[] GetRoles(int userID)
        {
            try
            {
                //return _db.Query<UserRole>(ur => ur.User, ur => ur.Role).Where(ur => ur.UserId == userID).Select(ur => ur.Role).ToArray();
                var roles = (from ur in _db.Query<UserRole>(ur => ur.User, ur => ur.Role)
                           where ur.UserId == userID
                           select ur.Role).ToArray();
                return roles;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Role GetRoleByName(string RoleName)
        {
            try
            {
                //return _db.Query<Role>(r => r.UserRoles).Select(r => r.RoleName == RoleName).FirstOrDefault();
                return _db.Query<Role>().FirstOrDefault(r => r.RoleName == RoleName);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void CreatePermission(Permission permission)
        {
            try
            {

                _db.Add<Permission>(permission);
                _db.SaveChange();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Permission GetPermissionByName(string PermissionName)
        {
            try
            {
                return _db.Query<Permission>().FirstOrDefault(p => p.Name == PermissionName);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Permission GetPermission(int PermissionId)
        {
            return _db.Query<Permission>().Where(p => p.PermissionId == PermissionId).FirstOrDefault();
        }
    }
}