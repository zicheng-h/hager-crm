using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hager_crm.Data;
using hager_crm.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using hager_crm.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace hager_crm.Controllers
{
    [Authorize(Roles = "Admin, Supervisor")]
    public class EmployeesController : Controller
    {
        private readonly HagerContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeesController(HagerContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private SelectList GetCountriesSelectList(object selectedValue = null) =>
            new SelectList(_context.Countries.OrderBy(i => i.CountryName), "CountryID", "CountryName", selectedValue);
        private SelectList GetProvincesSelectList(object selectedValue = null) =>
            new SelectList(_context.Provinces.OrderBy(i => i.ProvinceName), "ProvinceID", "ProvinceName", selectedValue);
        private SelectList GetEmploymentTypesSelectList(object selectedValue = null) =>
            new SelectList(_context.EmploymentTypes.OrderBy(i => i.Type), "EmploymentTypeID", "Type", selectedValue);
        private SelectList GetPositionsSelectList(object selectedValue = null) =>
            new SelectList(_context.JobPositions.OrderBy(i => i.Position), "JobPositionID", "Position", selectedValue);

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var hagerContext = _context.Employees
                .Include(e => e.EmployeeCountry)
                .Include(e => e.EmployeeProvince)
                .Include(e => e.EmploymentType)
                .Include(e => e.JobPosition);
            return View(await hagerContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.EmployeeCountry)
                .Include(e => e.EmployeeProvince)
                .Include(e => e.EmploymentType)
                .Include(e => e.JobPosition)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["EmployeeCountryID"] = GetCountriesSelectList();
            ViewData["EmployeeProvinceID"] = GetProvincesSelectList();
            ViewData["EmploymentTypeID"] = GetEmploymentTypesSelectList();
            ViewData["JobPositionID"] = GetPositionsSelectList();
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FirstName,LastName,JobPositionID,EmploymentTypeID,EmployeeAddress1,EmployeeAddress2,EmployeeProvinceID,EmployeePostalCode,EmployeeCountryID,CellPhone,WorkPhone,Email,DOB,Wage,Expense,DateJoined,KeyFob,EmergencyContactName,EmergencyContactPhone,Active,Notes,UserId")] Employee employee,
            string userPassword,
            string userRepeatPassword,
            bool isUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (isUser)
                        employee.UserId = await HandleIdentityCreation(employee.Email, userPassword, userRepeatPassword);
                    if (!(isUser && string.IsNullOrEmpty(employee.UserId)))
                    {
                        _context.Add(employee);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception e)
                {
                    _context.HandleDatabaseException(e, ModelState);
                }
            }
            ViewData["EmployeeCountryID"] = GetCountriesSelectList(employee.EmployeeCountryID);
            ViewData["EmployeeProvinceID"] = GetProvincesSelectList(employee.EmployeeProvinceID);
            ViewData["EmploymentTypeID"] = GetEmploymentTypesSelectList(employee.EmploymentTypeID);
            ViewData["JobPositionID"] = GetPositionsSelectList(employee.JobPositionID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmployeeCountryID"] = GetCountriesSelectList(employee.EmployeeCountryID);
            ViewData["EmployeeProvinceID"] = GetProvincesSelectList(employee.EmployeeProvinceID);
            ViewData["EmploymentTypeID"] = GetEmploymentTypesSelectList(employee.EmploymentTypeID);
            ViewData["JobPositionID"] = GetPositionsSelectList(employee.JobPositionID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? id, 
            string userPassword,
            string userRepeatPassword,
            bool isUser)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(employee, "",
                x => x.FirstName,
                x => x.LastName,
                x => x.JobPositionID,
                x => x.EmploymentTypeID,
                x => x.EmployeeAddress1,
                x => x.EmployeeAddress2,
                x => x.EmployeeProvinceID,
                x => x.EmployeePostalCode,
                x => x.EmployeeCountryID,
                x => x.CellPhone,
                x => x.WorkPhone,
                x => x.Email,
                x => x.DOB,
                x => x.Wage,
                x => x.Expense,
                x => x.DateJoined,
                x => x.KeyFob,
                x => x.EmergencyContactName,
                x => x.EmergencyContactPhone,
                x => x.Active,
                x => x.Notes))
            {
                try
                {
                    if (isUser && !employee.IsUser)
                        employee.UserId = await HandleIdentityCreation(employee.Email, userPassword, userRepeatPassword);
                    else if (!string.IsNullOrEmpty(userPassword))
                        await HandleIdentityPasswordChange(employee.UserId, userPassword, userRepeatPassword);
                        
                    if (!(isUser && string.IsNullOrEmpty(employee.UserId)))
                    {
                        await HandleIdentityEmailChange(employee.UserId, employee.Email);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                        
                }
                catch (Exception e)
                {
                    _context.HandleDatabaseException(e, ModelState);
                }
            }
            ViewData["EmployeeCountryID"] = GetCountriesSelectList(employee.EmployeeCountryID);
            ViewData["EmployeeProvinceID"] = GetProvincesSelectList(employee.EmployeeProvinceID);
            ViewData["EmploymentTypeID"] = GetEmploymentTypesSelectList(employee.EmploymentTypeID);
            ViewData["JobPositionID"] = GetPositionsSelectList(employee.JobPositionID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.EmployeeCountry)
                .Include(e => e.EmployeeProvince)
                .Include(e => e.EmploymentType)
                .Include(e => e.JobPosition)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            var identity = await _userManager.FindByIdAsync(employee.UserId);
            if (identity != null)
                await _userManager.DeleteAsync(identity);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ValidateCreateUserVM(string email, string password, string repeatPassword)
        {
            var validationResults = new List<ValidationResult>(); 
            var identity = new CreateUserVM { Email = email, Password = password, RepeatPassword = repeatPassword };
            var context = new ValidationContext(identity, serviceProvider: null, items: null);
            var isValid = Validator.TryValidateObject(identity, context, validationResults, true);
            foreach (var valRes in validationResults)
                ModelState.AddModelError(string.Join(", ", valRes.MemberNames), valRes.ErrorMessage);
            return isValid;
        }

        private async Task HandleIdentityPasswordChange(string userId, string password, string repeatPassword)
        {
            var isValid = ValidateCreateUserVM("", password, repeatPassword);
            if (!isValid)
                return;
            var identity = await _userManager.FindByIdAsync(userId);
            if (identity != null)
            {
                await _userManager.RemovePasswordAsync(identity);
                await _userManager.AddPasswordAsync(identity, password);
            }
        }

        private async Task HandleIdentityEmailChange(string userId, string email)
        {
            var identity = await _userManager.FindByIdAsync(userId);
            if (identity != null)
            {
                identity.Email = email;
                identity.UserName = email;
                await _userManager.UpdateAsync(identity);
            }
        }

        private async Task<string> HandleIdentityCreation(string email, string password, string repeatPassword)
        {
            var isValid = ValidateCreateUserVM(email, password, repeatPassword);
            if (!isValid)
                return null;
            var user = new IdentityUser { Email = email, UserName = email };
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded ? user.Id : null;
        }
    }
}
