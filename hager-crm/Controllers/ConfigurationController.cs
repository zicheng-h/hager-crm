using hager_crm.Data;
using hager_crm.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConfigurationController : Controller
    {
        private readonly ApplicationDbContext _iContext;
        private readonly HagerContext _hContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ConfigurationController(ApplicationDbContext identityContext, HagerContext hagerContext,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _iContext = identityContext;
            _hContext = hagerContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetRoles()
        {
            List<RoleVM> roles = new List<RoleVM>();
            foreach (var role in _iContext.Roles)
            {
                var r = new RoleVM {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                var Ids = from ur in _iContext.UserRoles
                        where ur.RoleId == r.RoleId
                        select ur.UserId;

                foreach (var UserId in Ids)
                {
                    var emp = _hContext.Employees.FirstOrDefault(e => e.UserId == UserId);

                    var vm = new EmployeeVM
                    {
                        EmployeeID = emp.EmployeeID,
                        Fullname = emp.FullName,
                        UserID = UserId
                    };
                    vm.Roles.Add(r);

                    r.Employees.Add(vm);
                }
                roles.Add(r);
            }

            var empWithRoles = _iContext.UserRoles.Select(ur => ur.UserId).ToList();
            var notAssigned = new RoleVM();
            notAssigned.RoleName = "Unassigned";
            foreach (var e in _hContext.Employees.Where(e => !(String.IsNullOrEmpty(e.UserId) || empWithRoles.Contains(e.UserId))))
                notAssigned.Employees.Add(new EmployeeVM
                {
                    EmployeeID = e.EmployeeID,
                    Fullname = e.FullName,
                    UserID = e.UserId
                });
            if (notAssigned.Employees.Count > 0)
                roles.Add(notAssigned);

            return PartialView("~/Views/Configuration/Roles/_Roles.cshtml", roles);
        }

        public PartialViewResult GetUsers(string role)
        {
            var roleId = _iContext.Roles.SingleOrDefault(r => r.Name == role)?.Id;

            if (roleId == null)
            {
                var empWithRoles = _iContext.UserRoles.Select(ur => ur.UserId).ToList();
                var noRoles = new List<EmployeeVM>();
                foreach (var e in _hContext.Employees.Where(e => !empWithRoles.Contains(e.UserId)))
                    noRoles.Add(new EmployeeVM
                    {
                        EmployeeID = e.EmployeeID,
                        Fullname = e.FullName,
                        UserID = e.UserId
                    });
                return PartialView("~/Views/Configuration/Roles/_UsersNoRole.cshtml", noRoles);
            }

            var userIds = _iContext.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .Select(e => e.UserId)
                .ToList();

            List<EmployeeVM> users = new List<EmployeeVM>();
            List<EmployeeVM> usersNotInRole = new List<EmployeeVM>();

            foreach (var emp in _hContext.Employees
                                .OrderBy(e => e.LastName)
                                .ThenBy(e => e.FirstName)
                )
                if (userIds.Contains(emp.UserId))
                    users.Add(new EmployeeVM
                    {
                        EmployeeID = emp.EmployeeID,
                        Fullname = emp.FullName,
                        UserID = emp.UserId
                    });
                else if (emp.IsUser)
                    usersNotInRole.Add(new EmployeeVM
                    {
                        EmployeeID = emp.EmployeeID,
                        Fullname = emp.FullName,
                        UserID = emp.UserId
                    });

            ViewData["UsersNotInRole"] = new SelectList(usersNotInRole, "UserID", "Fullname");
            ViewData["Role"] = role;

            return PartialView("~/Views/Configuration/Roles/_Users.cshtml", users);
        }

        public PartialViewResult NewRole()
        {
            return PartialView("~/Views/Configuration/Roles/_NewRole.cshtml");
        }

        public async Task<PartialViewResult> GetLookups()
        {
            var lookups = new List<LookupVM>();
            foreach (var lookup in _hContext.GetLookups())
            {
                lookups.Add(new LookupVM {LookupName = lookup.GetLookupName(), Items = await lookup.GetAll(_hContext)});
            }
            return PartialView("~/Views/Configuration/Lookups/_Lookups.cshtml", lookups);
        }

        [HttpPost]
        public async Task<IActionResult> AddLookup(string lookupName, string displayName)
        {
            var lookup = _hContext.GetLookups().Find(i => i.GetLookupName() == lookupName);
            if (lookup == null)
                return NotFound();
            int lookupId = await lookup.AddLookup(_hContext, displayName);
            return Json(new {lookupId});
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateLookup(string lookupName, string displayName, int lookupId)
        {
            var lookup = _hContext.GetLookups().Find(i => i.GetLookupName() == lookupName);
            if (lookup == null)
                return NotFound();
            bool result = await lookup.UpdateLookup(_hContext, lookupId, displayName);
            return Json(new {result});
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteLookup(string lookupName, int lookupId)
        {
            var lookup = _hContext.GetLookups().Find(i => i.GetLookupName() == lookupName);
            if (lookup == null)
                return NotFound();
            bool result = await lookup.DeleteLookup(_hContext, lookupId);
            return Json(new {result});
        }
        
        [HttpPost]
        public async void AddUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.AddToRoleAsync(user, roleName);

            return;
        }

        [HttpPost]
        public async void RemoveUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.RemoveFromRoleAsync(user, roleName);

            return;
        }

        [HttpPost]
        public async void DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.DeleteAsync(user);

            _hContext.Employees.FirstOrDefault(e => e.UserId == userId).UserId = String.Empty;
            await _hContext.SaveChangesAsync();

            return;
        }

        [HttpPost]
        public async void DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            await _roleManager.DeleteAsync(role);

            return;
        }

        [HttpPost]
        public async void CreateRoleAsync(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
                await _roleManager.CreateAsync(new IdentityRole(roleName));

            return;
        }
    }
}
