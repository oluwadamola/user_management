using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UserManagement.Models.Entities;
using UserManagement.Models.Interface.Manager;
using UserManagement.Models.ViewModel;
using UserManagement.Util.Toastr;

namespace UserManagement.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserManager _usermanager;
        StringBuilder message = new StringBuilder();

        public UserController(IUserManager usermanager)
        {
            _usermanager = usermanager;
        }
        //
        // GET: /User/

        public ActionResult Index()
        {
            try
            {
                this.AddToastMessage("User List", "List of available users", ToastType.Success);
                return View(_usermanager.GetUsers());
            }
            catch (Exception ex)
            {
                ViewBag.Message = " Error" + ex.Message;
                return View();
            }

        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(User user, String newpassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.ChangePassword(user.Email, user.Password, newpassword);
                    ModelState.Clear();
                    this.AddToastMessage("Success", "Password Successfully changed", ToastType.Success);
                    return Redirect("Login");
                }
                /*foreach (var modelstate in ModelState.Values)
                {
                    foreach (var modelerror in modelstate.Errors)
                    {
                        message.Append(modelerror.ErrorMessage);
                    }
                }
                ModelState.AddModelError("", message.ToString());
                this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View(user);
            }
        }
        [HttpGet]
        public ActionResult RecoverPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RecoverPassword(string email)
        {
            try
            {
                var pass = _usermanager.Recover(email);
                ModelState.AddModelError("", pass);
                this.AddToastMessage("Success", "Password Recovered Successfully", ToastType.Success);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(UserProfile userprofile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //user.CreatedBy = 1L;
                    //user.CreateOn = DateTime.Now;
                    _usermanager.SignUp(userprofile);
                    ModelState.AddModelError("", "User successfully created");
                    this.AddToastMessage("Success", "User Successfuly created", ToastType.Success);
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    /*foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                    }
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    return View(userprofile);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error " + ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View(userprofile);
            }
        }

        [HttpGet]
        public ActionResult UpdateUser(int id)
        {
            User user = _usermanager.GetUser(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult UpdateUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.UpdateUser(user);
                    ModelState.AddModelError("", "User Successfully Created");
                    this.AddToastMessage("Success", message.ToString(), ToastType.Success);
                    return View(user);
                }
                else
                {
                    /*foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    GetErrorMessages();
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View(user);
            }

        }

        [HttpGet]
        public ActionResult RemoveUser(int id)
        {
            User user = _usermanager.GetUser(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult RemoveUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.RemoveUser(user);
                    this.AddToastMessage("Success", "User Successfully Removed", ToastType.Success);
                    return View(user);

                }
                else
                {
                    /*foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    GetErrorMessages();
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View(user);
            }
        }

        /*       [HttpGet]
               public ActionResult AssignRole()
               {
                   return View();
               }
               [HttpPost]
              public ActionResult AssignRole(int userId, String role)
               {
            
                   try
                   {
                       if(ModelState.IsValid)
                       {
                           _usermanager.AssignRole(userId, role);
                           this.AddToastMessage("Exception", "Role Successfully Assigned", ToastType.Success);
                           return Redirect("index"); 
                       }
                       else
                       {
                           foreach(var modelstate in ModelState.Values)
                           {
                               foreach(var modelerror in modelstate.Errors)
                               {
                                   message.Append(modelerror.ErrorMessage);
                               }
                           }
                           ModelState.AddModelError("", message.ToString());
                           this.AddToastMessage("Exception", message.ToString(), ToastType.Error);
                       }
                   }
                   catch(Exception ex)
                   {
                       ModelState.AddModelError("", ex.Message);
                       this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                       return View(userId);
                   }
                   return Redirect("Index");
               }*/

        [HttpGet]
        public ActionResult RemoveRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RemoveRole(User user, string role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.RemoveRole(user, role);
                    this.AddToastMessage("Success", "Role Successfully Removed", ToastType.Success);
                    return View();
                }
                else
                {
                    /*foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    GetErrorMessages();
                    return View();
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View(user);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignUp(UserProfile userprofile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.SignUp(userprofile);
                    ModelState.Clear();
                    this.AddToastMessage("Success", " User Successfully Saved", ToastType.Success);
                    return View();

                }
                else
                {
                   /* foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    GetErrorMessages();
                    return View(userprofile);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View(userprofile);
            }
        }

        [HttpGet]
        public ActionResult AssignRole()
        {
            User[] users = _usermanager.GetUsers();
            Role[] roles = _usermanager.GetRoles();

            ViewBag.users = users;
            ViewBag.roles = roles;
            return View();
        }
        [HttpPost]
        public ActionResult AssignRole(UserRoleModel model)
        {
            User[] users = _usermanager.GetUsers();
            Role[] roles = _usermanager.GetRoles();

            User user = _usermanager.GetUser(model.UserId);
            Role role = _usermanager.GetRole(model.RoleId);
            ViewBag.users = users;
            ViewBag.roles = roles;
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.AssignRole(user.UserId, role.RoleName);
                    this.AddToastMessage("Success", "Role is Successfully Assigned ", ToastType.Success);
                    return View();

                }
                else
                {
                    /*foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    GetErrorMessages();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateRole(Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.CreateRole(role);
                    ModelState.Clear();
                    this.AddToastMessage("Success", " Role Successfully Added", ToastType.Success);
                    return View();
                }
                else
                {
                    /*foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);

                        }
                    }
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    GetErrorMessages();
                    return View(role);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                this.AddToastMessage("Exception", message.ToString(), ToastType.Error);
                return View(role);
            }
        }

        [HttpGet]
        public ActionResult GetPermission()
        {
            User user = new User();
            return View(user);
        }
        [HttpPost]
        public ActionResult GetPermission(string userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.GetPermissions(userId);
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    /*foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    GetErrorMessages();
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreatePermission()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreatePermission(Permission permission)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usermanager.CreatePermission(permission);
                    this.AddToastMessage("Success", "Permission is Successfully Assigned ", ToastType.Success);
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    /*foreach (var modelstate in ModelState.Values)
                    {
                        foreach (var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);*/
                    GetErrorMessages();
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User usr = _usermanager.ValidateLogin(user.Email, user.Password);

                    if (usr != null)
                    {
                        Role[] roles = _usermanager.GetRoles(user.UserId);

                        List<Claim> claims = new List<Claim>()
                        {
                             new Claim(ClaimTypes.NameIdentifier, user.Email),
                             new Claim(ClaimTypes.Name, usr.Profile.FirstName)                                            
                        };
                        foreach (var rol in roles)
                        {
                           claims.Add(new Claim(ClaimTypes.Role, rol.RoleName));
                        }

                        ClaimsIdentity identities = new ClaimsIdentity( claims,   DefaultAuthenticationTypes.ApplicationCookie);
                        HttpContext.GetOwinContext().Authentication.SignIn(identities);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid UserName or Password");
                        this.AddToastMessage("Exception", "Invalid UserName Or Password", ToastType.Error);
                    }
                    return View(user);
                }
                else
                {
                    /*foreach(var modelstate in ModelState.Values)
                    {
                        foreach(var modelerror in modelstate.Errors)
                        {
                            message.Append(modelerror.ErrorMessage);
                        }
                        
                    }*/

                    GetErrorMessages();
                    return View(user);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                this.AddToastMessage("Exception", ex.Message, ToastType.Error);
                return View(user);
            }
        }

        private void GetErrorMessages()
        {
            string msg = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Aggregate((ag, e) => ag + "," + e);

            ModelState.AddModelError("", msg);
            this.AddToastMessage("Exception", msg, ToastType.Error);
        }

        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
        }
        
    }
}
