using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserManagement.Models.Entities;
using UserManagement.Models.DataAccess;
using UserManagement.Models.Interface.Manager;
using System.Text;
using UserManagement.Util.Toastr;

namespace UserManagement.Controllers
{
    public class RoleController : Controller
    {
        private IUserManager _usermanager;
        StringBuilder message = new StringBuilder();

        public RoleController(IUserManager usermanager)
        {
            _usermanager = usermanager;
        }
           
        public ActionResult Index()
        {
            return View(_usermanager.GetRoles());
        }
        // GET: /Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
            try
            {
                role.CreateOn = DateTime.Now;

                if (ModelState.IsValid)
                {
                    _usermanager.CreateRole(role);
                    ViewBag.Message = "Role is saved successfully";
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    GetErrorMessages();
                }
                return View(role);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(role);
            }
            
        }

        private void GetErrorMessages()
        {
            foreach (var modelstate in ModelState.Values)
            {
                foreach (var modelerror in modelstate.Errors)
                {
                    ModelState.AddModelError("", modelerror.ErrorMessage);
                }
            }
        }

        [HttpGet]
        public ActionResult AssignPermissionToRole()
        {
            Permission[] permissions = _usermanager.GetPermissions();
            Role[] roles = _usermanager.GetRoles();
            ViewBag.permissions = permissions;
            ViewBag.roles = roles;
            return View();
        }
        [HttpPost]
        public ActionResult AssignPermissionToRole(RolePermission model)
        {
            Permission[] permissions = _usermanager.GetPermissions();
            Role[] roles = _usermanager.GetRoles();

            Permission permission = _usermanager.GetPermission(model.PermissionId);
            Role role = _usermanager.GetRole(model.RoleId);
           
            ViewBag.permissions = permissions;
            ViewBag.roles = roles;
             
            try
            {
               
                if (ModelState.IsValid)
                {
                    _usermanager.AssignPermissionToRole(role.RoleId, permission.Name);
                    this.AddToastMessage("Success", "Permission Successfully Assigned to Role", ToastType.Success);
                    return View();
                }
                else
                {
                    GetErrorMessages();
                    ModelState.AddModelError("", message.ToString());
                    this.AddToastMessage("Exception", message.ToString(), ToastType.Error);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

    }
}
