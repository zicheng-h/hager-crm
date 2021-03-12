using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hager_crm.Data;
using hager_crm.Models;
using hager_crm.Utils;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;

namespace hager_crm.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly HagerContext _context;

        public CompaniesController(HagerContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index(string SearchString, int? ProvinceID, int? CountryID,
            int? page, int? pageSizeID, string actionButton, string sortDirection = "asc",
            string sortField = "Name", string CompanyType = "All")
        {
            PopulateDropDownListsProvince();
            PopulateDropDownListsCountry();
            ViewData["Filtering"] = "";
            ViewData["CompanyType"] = CompanyType;

            IQueryable<Company> hagerContext = _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.ContractorType)
                .Include(c => c.Currency)
                .Include(c => c.CustomerType)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.VendorType)
                .OrderBy(c => c.Name);

            switch (CompanyType)
            {
                case "All":
                    break;
                case "Customer":
                    hagerContext = hagerContext.Where(c => c.Customer == true);
                    break;
                case "Vendor":
                    hagerContext = hagerContext.Where(c => c.Vendor == true);
                    break;
                case "Contractor":
                    hagerContext = hagerContext.Where(c => c.Contractor == true);
                    break;
                default:  //all other values are invalid
                    return NotFound();
            }

            //Add Filter by Province
            if (ProvinceID.HasValue)
            {
                hagerContext = hagerContext.Where(b => b.BillingProvinceID == ProvinceID);
                ViewData["Filtering"] = "show";
            }

            //Add Filter by Country
            if (CountryID.HasValue)
            {
                hagerContext = hagerContext.Where(b => b.BillingCountryID == CountryID);
                ViewData["Filtering"] = "show";
            }
            //Search by Company Name

            if(!String.IsNullOrEmpty(SearchString))
            {
                hagerContext = hagerContext.Where(b => b.Name.ToUpper().Contains(SearchString.ToUpper()));
                ViewData["Filtering"] = "show";
            }
            //Call for a change of filtering or sorting
            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }

            //Sort by field and revert direction when click again
            if(sortField == "Name")
            {
                if(sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(b => b.Name);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(b => b.Name);
                }
            }
            else if(sortField == "Location")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(b => b.Location);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(b => b.Location);
                }
            }

            else if (sortField == "Province")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(b => b.BillingProvince);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(b => b.BillingProvince);
                }
            }

            else if (sortField == "Country")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(b => b.BillingCountry);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(b => b.BillingCountry);
                }
            }

            else if (sortField == "Active")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(b => b.Active);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(b => b.Active);
                }
            }

            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            //Paging

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;
            }

            //Handle Paging
            int pageSize;//This is the value we will pass to PaginatedList
            if (pageSizeID.HasValue)
            {
                //Value selected from DDL so use and save it to Cookie
                pageSize = pageSizeID.GetValueOrDefault();
                CookieHelper.CookieSet(HttpContext, "pageSizeValue", pageSize.ToString(), 30);
            }
            else
            {
                //Not selected so see if it is in Cookie
                pageSize = Convert.ToInt32(HttpContext.Request.Cookies["pageSizeValue"]);
            }
            pageSize = (pageSize == 0) ? 3 : pageSize;//Neither Selected or in Cookie so go with default
            ViewData["pageSizeID"] =
                new SelectList(new[] { "3", "5", "10", "20", "30", "40", "50", "100", "500" }, pageSize.ToString());

            var pagedData = await PaginatedList<Company>.CreateAsync(hagerContext.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id, string CompanyType)
        {
            ViewData["CompanyType"] = CompanyType;

            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.ContractorType)
                .Include(c => c.Currency)
                .Include(c => c.CustomerType)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.VendorType)
                .FirstOrDefaultAsync(m => m.CompanyID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Create(string CompanyType)
        {
            ViewData["CompanyType"] = CompanyType;
            ViewData["BillingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName");
            ViewData["BillingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName");
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms, "BillingTermID", "Terms");
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type");
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyName");
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type");
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName");
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName");
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "Type");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Create([Bind("CompanyID,Name,Location,CreditCheck,DateChecked,BillingTermID,CurrencyID,Phone,Website,BillingAddress1,BillingAddress2,BillingProvinceID,BillingPostalCode,BillingCountryID,ShippingAddress1,ShippingAddress2,ShippingProvinceID,ShippingPostalCode,ShippingCountryID,Customer,CustomerTypeID,Vendor,VendorTypeID,Contractor,ContractorTypeID,Active,Notes")] Company company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(company);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Unable to create new record.");
            }
            
            ViewData["BillingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.BillingCountryID);
            ViewData["BillingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", company.BillingProvinceID);
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms, "BillingTermID", "Terms", company.BillingTermID);
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "ContractorTypeID", company.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyID", company.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "CustomerTypeID", company.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", company.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "VendorTypeID", company.VendorTypeID);
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Edit(int? id, string CompanyType)
        {
            ViewData["CompanyType"] = CompanyType;

            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            ViewData["BillingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.BillingCountryID);
            ViewData["BillingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", company.BillingProvinceID);
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms, "BillingTermID", "Terms", company.BillingTermID);
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type", company.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", company.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type", company.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", company.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "Type", company.VendorTypeID);
            return View(company);
        }
        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Edit(int id)
        {
            var companyToUpdate = await _context.Companies
                .Include(c => c.BillingTerm)
                .Include(c => c.Currency)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.ShippingCountry)
                .Include(c => c.CustomerType)
                .Include(c => c.VendorType)
                .Include(c => c.ContractorType)
                .SingleOrDefaultAsync(c => c.CompanyID == id);
            if (companyToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Company>(companyToUpdate,"",
                c => c.Name,
                c => c.Location,
                c => c.CreditCheck,
                c => c.DateChecked,
                c => c.BillingTermID,
                c => c.CurrencyID,
                c => c.Phone,
                c => c.Website,
                c => c.BillingAddress1,
                c => c.BillingAddress2,
                c => c.BillingProvinceID,
                c => c.BillingPostalCode,
                c => c.BillingCountryID,
                c => c.ShippingAddress1,
                c => c.ShippingAddress2,
                c => c.ShippingProvinceID,
                c => c.ShippingPostalCode,
                c => c.ShippingCountryID,
                c => c.Customer,
                c => c.CustomerTypeID,
                c => c.Vendor,
                c => c.VendorTypeID,
                c => c.Contractor,
                c => c.ContractorTypeID,
                c => c.Active,
                c => c.Notes
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(companyToUpdate.CompanyID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            ViewData["BillingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", companyToUpdate.BillingCountryID);
            ViewData["BillingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", companyToUpdate.BillingProvinceID);
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms, "BillingTermID", "Terms", companyToUpdate.BillingTermID);
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type", companyToUpdate.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", companyToUpdate.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type", companyToUpdate.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", companyToUpdate.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", companyToUpdate.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "Type", companyToUpdate.VendorTypeID);
            return View(companyToUpdate);
        }
        // GET: Companies/Delete/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Delete(int? id, string CompanyType)
        {
            ViewData["CompanyType"] = CompanyType;

            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.ContractorType)
                .Include(c => c.Currency)
                .Include(c => c.CustomerType)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.VendorType)
                .FirstOrDefaultAsync(m => m.CompanyID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDownListsProvince(Company company = null)
        {
            var cQuery = from c in _context.Provinces
                         orderby c.ProvinceName
                         select c;
            ViewData["ProvinceID"] = new SelectList(cQuery, "ProvinceID", "ProvinceName", company?.BillingProvinceID);
        }

        private void PopulateDropDownListsCountry(Company company = null)
        {
            var cQuery = from c in _context.Countries
                         orderby c.CountryName
                         select c;
            ViewData["CountryID"] = new SelectList(cQuery, "CountryID", "CountryName", company?.BillingCountryID);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> InsertFromExcelCompany(IFormFile theExcel)
        {
            if (theExcel == null)
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
            List<Company> companies = new List<Company>();

            for (int row = start.Row + 1; row <= end.Row; row++)
            {
                //Check if employee exist in database already
                string nameCompare = workSheet.Cells[row, 1].Text;
                //string tempName = _context.Employees.FirstOrDefault(p => p.Email == emailCompare).Email;
                if (_context.Companies.FirstOrDefault(p => p.Name == nameCompare) == null)
                {
                    try
                    {
                        Company a = new Company
                        {
                            Name = workSheet.Cells[row, 1].Text,
                            Location = workSheet.Cells[row, 2].Text,
                            CreditCheck = workSheet.Cells[row, 3].Text == "1",
                            DateChecked = DateTime.Parse((workSheet.Cells[row, 4].Text == "") ? "1900-01-01" : workSheet.Cells[row, 4].Text),
                            BillingTermID = _context.BillingTerms.FirstOrDefault(b => b.Terms == workSheet.Cells[row, 5].Text).BillingTermID,
                            BillingTerm = _context.BillingTerms.FirstOrDefault(b => b.Terms == workSheet.Cells[row, 5].Text),
                            CurrencyID = _context.Currencies.FirstOrDefault(b => b.CurrencyName == workSheet.Cells[row, 6].Text).CurrencyID,
                            Currency = _context.Currencies.FirstOrDefault(b => b.CurrencyName == workSheet.Cells[row, 6].Text),
                            Phone = (workSheet.Cells[row, 7].Text != "") ? Convert.ToInt64(workSheet.Cells[row, 7].Text.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "")) : Convert.ToInt64("0"),
                            Website = workSheet.Cells[row, 8].Text,
                            BillingAddress1 = workSheet.Cells[row, 9].Text,
                            BillingAddress2 = workSheet.Cells[row, 10].Text,
                            BillingCity = workSheet.Cells[row, 11].Text,
                            BillingProvinceID = _context.Provinces.FirstOrDefault(b => b.ProvinceName == workSheet.Cells[row, 12].Text).ProvinceID,
                            BillingPostalCode = workSheet.Cells[row, 13].Text,
                            BillingCountryID = _context.Countries.FirstOrDefault(b => b.CountryName == workSheet.Cells[row, 14].Text).CountryID,

                            ShippingAddress1 = workSheet.Cells[row, 15].Text,
                            ShippingAddress2 = workSheet.Cells[row, 16].Text,
                            ShippingCity = workSheet.Cells[row, 17].Text,
                            ShippingProvinceID = _context.Provinces.FirstOrDefault(b => b.ProvinceName == workSheet.Cells[row, 18].Text).ProvinceID,
                            ShippingPostalCode = workSheet.Cells[row, 19].Text,
                            ShippingCountryID = _context.Countries.FirstOrDefault(b => b.CountryName == workSheet.Cells[row, 20].Text).CountryID,

                            Customer = workSheet.Cells[row, 21].Text == "1",
                            CustomerTypeID = _context.CustomerTypes.FirstOrDefault(b => b.Type == workSheet.Cells[row, 22].Text).CustomerTypeID,

                            Vendor = workSheet.Cells[row, 23].Text == "1",
                            VendorTypeID = _context.VendorTypes.FirstOrDefault(b => b.Type == workSheet.Cells[row, 24].Text).VendorTypeID,

                            Contractor = workSheet.Cells[row, 25].Text == "1",
                            ContractorTypeID = _context.ContractorTypes.FirstOrDefault(b => b.Type == workSheet.Cells[row, 26].Text).ContractorTypeID,
                            Active = workSheet.Cells[row, 27].Text == "1",
                            Notes = workSheet.Cells[row, 28].Text

                        };
                        companies.Add(a);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("Error", "Error while parsing the file");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            _context.Companies.AddRange(companies);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyID == id);
        }

        [Authorize(Roles = "Admin, Supervisor")]
        [Route("Company/Compare/{leftCompanyId}/with/{rightCompanyId}")]
        public async Task<IActionResult> Compare(int leftCompanyId, int rightCompanyId)
        {
            var leftCompany = _context.Companies.FirstOrDefault(c => c.CompanyID == leftCompanyId);
            var rightCompany = _context.Companies.FirstOrDefault(c => c.CompanyID == rightCompanyId);
            if (leftCompany == null || rightCompany == null)
                return NotFound();

            ViewData["LeftMerge"] = new Dictionary<string, SelectList>
            {
                { "BillingCountryID", new SelectList(_context.Countries, "CountryID", "CountryName", leftCompany.BillingCountryID) },
                { "BillingProvinceID", new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", leftCompany.BillingProvinceID) },
                { "BillingTermID", new SelectList(_context.BillingTerms, "BillingTermID", "Terms", leftCompany.BillingTermID) },
                { "ContractorTypeID", new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type", leftCompany.ContractorTypeID) },
                { "CurrencyID", new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", leftCompany.CurrencyID) },
                { "CustomerTypeID", new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type", leftCompany.CustomerTypeID) },
                { "ShippingCountryID", new SelectList(_context.Countries, "CountryID", "CountryName", leftCompany.ShippingCountryID) },
                { "ShippingProvinceID", new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", leftCompany.ShippingProvinceID) },
                { "VendorTypeID", new SelectList(_context.VendorTypes, "VendorTypeID", "Type", leftCompany.VendorTypeID) },
            };
            ViewData["RightMerge"] = new Dictionary<string, SelectList>
            {
                { "BillingCountryID", new SelectList(_context.Countries, "CountryID", "CountryName", rightCompany.BillingCountryID) },
                { "BillingProvinceID", new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", rightCompany.BillingProvinceID) },
                { "BillingTermID", new SelectList(_context.BillingTerms, "BillingTermID", "Terms", rightCompany.BillingTermID) },
                { "ContractorTypeID", new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type", rightCompany.ContractorTypeID) },
                { "CurrencyID", new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", rightCompany.CurrencyID) },
                { "CustomerTypeID", new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type", rightCompany.CustomerTypeID) },
                { "ShippingCountryID", new SelectList(_context.Countries, "CountryID", "CountryName", rightCompany.ShippingCountryID) },
                { "ShippingProvinceID", new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", rightCompany.ShippingProvinceID) },
                { "VendorTypeID", new SelectList(_context.VendorTypes, "VendorTypeID", "Type", rightCompany.VendorTypeID) },
            };
            return View(new Company[]{ leftCompany, rightCompany });
        }

        [Authorize(Roles = "Admin, Supervisor")]
        [HttpPost]
        [Route("Company/Merge/{leftCompanyId}/to/{rightCompanyId}")]
        public async Task<IActionResult> Merge(int leftCompanyId, int rightCompanyId, string[] fieldsToMerge)
        {
            var leftCompany = _context.Companies.FirstOrDefault(c => c.CompanyID == leftCompanyId);
            var rightCompany = _context.Companies.FirstOrDefault(c => c.CompanyID == rightCompanyId);
            if (leftCompany == null || rightCompany == null)
                return NotFound();

            // This is ridiculous, but I don't want to deal with a reflection in loop.
            foreach (var field in fieldsToMerge)
            switch (field)
            {
                case "Name":
                    rightCompany.Name = leftCompany.Name;
                    break;
                case "Location":
                    rightCompany.Location = leftCompany.Location;
                    break;
                case "CreditCheck":
                    rightCompany.CreditCheck = leftCompany.CreditCheck;
                    break;
                case "DateChecked":
                    rightCompany.DateChecked = leftCompany.DateChecked;
                    break;
                case "BillingTermID":
                    rightCompany.BillingTermID = leftCompany.BillingTermID;
                    break;
                case "CurrencyID":
                    rightCompany.CurrencyID = leftCompany.CurrencyID;
                    break;
                case "Phone":
                    rightCompany.Phone = leftCompany.Phone;
                    break;
                case "Website":
                    rightCompany.Website = leftCompany.Website;
                    break;
                case "BillingAddress":
                    rightCompany.BillingAddress1 = leftCompany.BillingAddress1;
                    rightCompany.BillingAddress2 = leftCompany.BillingAddress2;
                    break;
                case "BillingCity":
                    rightCompany.BillingCity = leftCompany.BillingCity;
                    break;
                case "BillingProvinceID":
                    rightCompany.BillingProvinceID = leftCompany.BillingProvinceID;
                    break;
                case "BillingPostalCode":
                    rightCompany.BillingPostalCode = leftCompany.BillingPostalCode;
                    break;
                case "BillingCountryID":
                    rightCompany.BillingCountryID = leftCompany.BillingCountryID;
                    break;
                case "ShippingAddress":
                    rightCompany.ShippingAddress1 = leftCompany.ShippingAddress1;
                    rightCompany.ShippingAddress2 = leftCompany.ShippingAddress2;
                    break;
                case "ShippingCity":
                    rightCompany.ShippingCity = leftCompany.ShippingCity;
                    break;
                case "ShippingProvinceID":
                    rightCompany.ShippingProvinceID = leftCompany.ShippingProvinceID;
                    break;
                case "ShippingPostalCode":
                    rightCompany.ShippingPostalCode = leftCompany.ShippingPostalCode;
                    break;
                case "ShippingCountryID":
                    rightCompany.ShippingCountryID = leftCompany.ShippingCountryID;
                    break;
                case "Customer":
                    rightCompany.Customer = leftCompany.Customer;
                    break;
                case "CustomerTypeID":
                    rightCompany.CustomerTypeID = leftCompany.CustomerTypeID;
                    break;
                case "Vendor":
                    rightCompany.VendorTypeID = leftCompany.VendorTypeID;
                    break;
                case "Contractor":
                    rightCompany.Contractor = leftCompany.Contractor;
                    break;
                case "ContractorTypeID":
                    rightCompany.ContractorTypeID = leftCompany.ContractorTypeID;
                    break;
                case "Active":
                    rightCompany.Active = leftCompany.Active;
                    break;
                case "Notes":
                    rightCompany.Notes = leftCompany.Notes;
                    break;

            }

            _context.Companies.Remove(leftCompany);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Details), new { id = rightCompanyId });
        }


    }
}
