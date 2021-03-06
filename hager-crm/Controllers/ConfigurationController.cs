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

        public ConfigurationController(ApplicationDbContext identityContext, HagerContext hagerContext,
            UserManager<IdentityUser> userManager)
        {
            _iContext = identityContext;
            _hContext = hagerContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> GetRoles()
        {
            List<RoleVM> roles = new List<RoleVM>();
            List<EmployeeVM> emps = new List<EmployeeVM>();

            var empUsers = _hContext.Employees
                .Where(e => !String.IsNullOrEmpty(e.UserId));

            foreach (var emp in empUsers)
                emps.Add(new EmployeeVM
                {
                    EmployeeID = emp.EmployeeID,
                    Fullname = emp.FullName,
                    UserID = emp.UserId
                });

            foreach (var role in _iContext.Roles)
            {
                var r = new RoleVM
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                var Ids = _iContext.UserRoles
                    .Where(ur => ur.RoleId == r.RoleId)
                    .Select(ur => ur.UserId);

                r.Employees = emps
                    .Where(e => Ids.Contains(e.UserID))
                    .OrderBy(e => e.Fullname)
                    .ToList();

                r.EmployeesNotInRole = emps
                    .Where(e => !Ids.Contains(e.UserID))
                    .OrderBy(e => e.Fullname)
                    .ToList();

                roles.Add(r);
            }

            var userIdsWithRoles = _iContext.UserRoles.Select(ur => ur.UserId).ToList();
            var usersWithNoRoles = empUsers.Where(e => !userIdsWithRoles.Contains(e.UserId));
            if (usersWithNoRoles.Count() > 0)
            {
                var notAssigned = new RoleVM();
                notAssigned.RoleName = "Unassigned";
                foreach (var e in usersWithNoRoles)
                    notAssigned.Employees.Add(new EmployeeVM
                    {
                        EmployeeID = e.EmployeeID,
                        Fullname = e.FullName,
                        UserID = e.UserId
                    });
                roles.Add(notAssigned);
            }

            var orphanUserIds = _iContext.UserRoles
                .Select(ur => ur.UserId)
                .Where(id => !emps.Select(e => e.UserID)
                                .Contains(id)
                    );
            foreach (var id in orphanUserIds)
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
            }

            return PartialView("~/Views/Configuration/Roles/_Roles.cshtml", roles);
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
        public async Task<IActionResult> UpdateLookupOrderAsync(string lookupName, int[] order)
        {
            var lookup = _hContext.GetLookups().Find(i => i.GetLookupName() == lookupName);
            if (lookup == null)
                return NotFound();
            bool result = await lookup.UpdateLookupOrder(_hContext, order);
            return Json(new { result });
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.AddToRoleAsync(user, roleName);

            return Json(new { result });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            return Json(new { result });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.DeleteAsync(user);

            _hContext.Employees.FirstOrDefault(e => e.UserId == userId).UserId = String.Empty;
            var result = await _hContext.SaveChangesAsync();

            return Json(new { result });
        }
    }
}
