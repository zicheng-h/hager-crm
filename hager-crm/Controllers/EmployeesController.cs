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
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using hager_crm.Models.FilterConfig;
using hager_crm.Utils;

namespace hager_crm.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly HagerContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GridFilter<Employee> _gridFilter;

        public EmployeesController(HagerContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _gridFilter = new GridFilter<Employee>(new EmployeeConfig(), 5);
        }

        private SelectList GetCountriesSelectList(object selectedValue = null) =>
            new SelectList(_context.Countries.OrderBy(i => i.CountryName), "CountryID", "CountryName", selectedValue);
        private SelectList GetProvincesSelectList(object selectedValue = null) =>
            new SelectList(_context.Provinces.OrderBy(i => i.ProvinceName), "ProvinceID", "ProvinceName", selectedValue);
        private SelectList GetEmploymentTypesSelectList(object selectedValue = null) =>
            new SelectList(_context.EmploymentTypes.OrderBy(i => i.Order), "EmploymentTypeID", "Type", selectedValue);
        private SelectList GetPositionsSelectList(object selectedValue = null) =>
            new SelectList(_context.JobPositions.OrderBy(i => i.Order), "JobPositionID", "Position", selectedValue);

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            //Clear the sort/filter/paging URL Cookie
            CookieHelper.CookieSet(HttpContext, "EmployeesURL", "", -1);

            var query = _context.Employees
                .Include(e => e.EmployeeCountry)
                .Include(e => e.EmployeeProvince)
                .Include(e => e.EmploymentType)
                .Include(e => e.JobPosition);
            
            _gridFilter.ParseQuery(HttpContext);
            ViewBag.gridFilter = _gridFilter;
            ViewData["EmploymentTypeID"] = GetEmploymentTypesSelectList();
            ViewData["JobPositionID"] = GetPositionsSelectList();

            await _gridFilter.GetFilteredData(query);
            int countFilter = _gridFilter.OuterFields.Count;
            if (countFilter > 2)
            {
                ViewData["ChangeColor"] = 1;
            }
            else
            {
                ViewData["ChangeColor"] = 0;
            }

            return View(await _gridFilter.GetFilteredData(query));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Employees");

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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Employees");

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [Bind("FirstName,LastName,JobPositionID,EmploymentTypeID,EmployeeAddress1,EmployeeAddress2,EmployeeProvinceID,EmployeePostalCode,EmployeeCountryID,CellPhone,WorkPhone,Email,DOB,Wage,Expense,DateJoined,KeyFob,EmergencyContactName,EmergencyContactPhone,Active,Notes,UserId")] Employee employee,
            string userPassword,
            string userRepeatPassword,
            bool isUser)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Employees");

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
                        //return RedirectToAction(nameof(Index));
                        return Redirect(ViewData["returnURL"].ToString());
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Employees");

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            int? id, 
            string userPassword,
            string userRepeatPassword,
            bool isUser)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Employees");

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
                    bool pwdRes = true;
                    if (isUser && !employee.IsUser)
                        employee.UserId = await HandleIdentityCreation(employee.Email, userPassword, userRepeatPassword);
                    else if (!string.IsNullOrEmpty(userPassword))
                         pwdRes = await HandleIdentityPasswordChange(employee.Email, employee.UserId, userPassword, userRepeatPassword);
                        
                    if (!(isUser && string.IsNullOrEmpty(employee.UserId)) && pwdRes)
                    {
                        var res = await HandleIdentityEmailChange(employee.UserId, employee.Email);
                        if (res)
                        {
                            await _context.SaveChangesAsync();
                            //return RedirectToAction(nameof(Index));
                            return Redirect(ViewData["returnURL"].ToString());
                        }
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Employees");

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Employees");

            var employee = await _context.Employees.FindAsync(id);
            var identity = await _userManager.FindByIdAsync(employee.UserId);
            if (identity != null)
                await _userManager.DeleteAsync(identity);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return Redirect(ViewData["returnURL"].ToString());
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

        private async Task<bool> HandleIdentityPasswordChange(string email, string userId, string password, string repeatPassword)
        {
            var isValid = ValidateCreateUserVM(email, password, repeatPassword);
            if (!isValid)
                return false;
            var identity = await _userManager.FindByIdAsync(userId);
            if (identity != null)
            {
                await _userManager.RemovePasswordAsync(identity);
                await _userManager.AddPasswordAsync(identity, password);
            }
            return true;
        }

        private async Task<bool> HandleIdentityEmailChange(string userId, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("Email", "Email address is required when creating or editing employee with a user");
                return false;
            }
            var identity = await _userManager.FindByIdAsync(userId);
            if (identity != null)
            {
                identity.Email = email;
                identity.UserName = email;
                await _userManager.UpdateAsync(identity);
            }
            return true;
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertFromExcel(IFormFile theExcel)
        {
            if(theExcel == null)
            {
                ModelState.AddModelError("No files selected", "Please select a file");
                return RedirectToAction(nameof(Index));
            }
            ExcelPackage excel;
            using (var memoryStream = new MemoryStream())
            {
                await theExcel.CopyToAsync(memoryStream);
                excel = new ExcelPackage(memoryStream);
            }
            var workSheet = excel.Workbook.Worksheets[0];
            var start = workSheet.Dimension.Start;
            var end = workSheet.Dimension.End;

            //Start a new list to hold imported objects
            List<Employee> employees = new List<Employee>();

            for (int row = start.Row +1; row <= end.Row; row++)
            {
                //Check if employee exist in database already
                string emailCompare = workSheet.Cells[row, 3].Text;
                //string tempName = _context.Employees.FirstOrDefault(p => p.Email == emailCompare).Email;
                if (_context.Employees.FirstOrDefault(p => p.Email == emailCompare) == null)
                {
                    try
                    {
                        Employee a = new Employee
                        {
                            FirstName = workSheet.Cells[row, 1].Text,
                            LastName = workSheet.Cells[row, 2].Text,
                            JobPositionID = _context.JobPositions.FirstOrDefault(p => p.Position == workSheet.Cells[row, 15].Text).JobPositionID,
                            EmploymentTypeID = _context.EmploymentTypes.FirstOrDefault(t => t.Type == workSheet.Cells[row, 16].Text).EmploymentTypeID,
                            EmployeeAddress1 = workSheet.Cells[row, 9].Text,
                            EmployeeAddress2 = workSheet.Cells[row, 10].Text,
                            EmployeeProvinceID = _context.Provinces.FirstOrDefault(p => p.ProvinceName == workSheet.Cells[row, 13].Text).ProvinceID,
                            EmployeePostalCode = workSheet.Cells[row, 12].Text,
                            EmployeeCountryID = _context.Countries.FirstOrDefault(c => c.CountryName == workSheet.Cells[row, 14].Text).CountryID,
                            CellPhone = (workSheet.Cells[row, 5].Text != "") ? Convert.ToInt64(workSheet.Cells[row, 5].Text.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "")) : Convert.ToInt64("0"),
                            WorkPhone = (workSheet.Cells[row, 5].Text != "") ? Convert.ToInt64(workSheet.Cells[row, 5].Text.Replace(")","").Replace("(","").Replace("-","").Replace(" ","")): Convert.ToInt64("0"),
                            Email = (workSheet.Cells[row, 3].Text == "")?(workSheet.Cells[row, 1].Text+ workSheet.Cells[row, 2].Text+ "@hagerindustries.com") : workSheet.Cells[row, 3].Text,
                            DOB = DateTime.Parse((workSheet.Cells[row, 6].Text == "")? "1900-01-01": workSheet.Cells[row, 6].Text),
                            Notes = workSheet.Cells[row, 6].Text,
                            Wage = Convert.ToDecimal((workSheet.Cells[row, 7].Text == "")?"0": (workSheet.Cells[row, 7].Text)),
                            Expense = Convert.ToDecimal((workSheet.Cells[row, 8].Text == "")?"0": workSheet.Cells[row, 8].Text),
                            DateJoined = DateTime.Parse(workSheet.Cells[row, 17].Text),
                            KeyFob = int.Parse(workSheet.Cells[row, 24].Text),
                            EmergencyContactName = workSheet.Cells[row, 22].Text,
                            EmergencyContactPhone = (workSheet.Cells[row, 23].Text != "")?Convert.ToInt64(workSheet.Cells[row, 23].Text.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "")): Convert.ToInt64("0"),
                            Active = workSheet.Cells[row, 20].Text == "1"
                        };
                        employees.Add(a);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("Error", "Error while parsing the file");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            _context.Employees.AddRange(employees);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        // GET: Employees/GetSignedIn
        [Authorize]
        public async Task<string> GetSignedIn()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees
                .Where(e => e.UserId == user.Id)
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                return User.Identity.Name;
            }
            return employee.FirstName;
        }
    }
}
