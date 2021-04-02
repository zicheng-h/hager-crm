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
using hager_crm.Models.FilterConfig;
using hager_crm.ViewModels;

namespace hager_crm.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly HagerContext _context;
        private readonly GridFilter<Company> _gridFilter;

        public CompaniesController(HagerContext context)
        {
            _context = context;
            _gridFilter = new GridFilter<Company>(new CompanyConfig(), 5);
        }

        private SelectList GetCountriesSelectList(object selectedValue = null) =>
            new SelectList(_context.Countries.ToList(), "CountryID", "CountryName", selectedValue);
        private SelectList GetProvincesSelectList(object selectedValue = null) =>
            new SelectList(_context.Provinces.Select(i => 
            new SelectListItem(
                i.ProvinceName, 
                $"{i.ProvinceID};{i.CountryID}", 
                selectedValue != null && i.ProvinceID.ToString() == selectedValue.ToString())
            )
            .ToList());

        private Tuple<Company, Company> GetSimillarCompanies(List<Company> companies)
        {
            var pairs = new List<Tuple<Company, Company>>();
            for(int i = 0; i < companies.Count; i++)
            {
                for (int j = i + 1; j < companies.Count; j++)
                    pairs.Add(new Tuple<Company, Company>(companies[i], companies[j]));
            }

            foreach(var pair in pairs)
            {
                if (LevenshteinDistance.Compute(pair.Item1.Name, pair.Item2.Name) <= 2)
                    return pair;
            }

            return null;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //Clear the sort/filter/paging URL Cookie
            CookieHelper.CookieSet(HttpContext, "CompaniesURL", "", -1);

            IQueryable<Company> query = _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.VendorType)
                .Include(c => c.CompanyCustomers).ThenInclude(c => c.CustomerType)
                .Include(c => c.CompanyContractors).ThenInclude(c => c.ContractorType)
                .Include(c => c.CompanyVendors).ThenInclude(c => c.VendorType)
                .OrderBy(c => c.Name);

            _gridFilter.ParseQuery(HttpContext);
            ViewBag.gridFilter = _gridFilter;
            ViewData["CountryID"] = GetCountriesSelectList();
            ViewData["ProvinceID"] = GetProvincesSelectList();
            ViewData["DuplicationCompany"] = GetSimillarCompanies(await _context.Companies.OrderBy(c => c.CompanyID).Take(20).ToListAsync());
            //string urlcheck = _gridFilter.GetFilteredData(query).Ur
            await _gridFilter.GetFilteredData(query);
            int countFilter = _gridFilter.OuterFields.Count;
            if(countFilter > 1)
            {
                ViewData["ChangeColor"] = 1;
            }
            else
            {
                ViewData["ChangeColor"] = 0;
            }
            return View(await _gridFilter.GetFilteredData(query));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id, string returnURL)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Companies");

            if (id == null)
            {
                return NotFound();
            }

            //Get the URL of the page that send us here
            //if (String.IsNullOrEmpty(returnURL))
            //{
            //    returnURL = Request.Headers["Referer"].ToString();
            //}
            //ViewData["returnURL"] = returnURL;

            var company = await _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.Currency)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .FirstOrDefaultAsync(m => m.CompanyID == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Create(string CType, string returnURL)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Companies");
            Company company = new Company();

            ////Get the URL of the page that send us here
            //if (String.IsNullOrEmpty(returnURL))
            //{
            //    returnURL = Request.Headers["Referer"].ToString();
            //}

            ViewData["returnURL"] = returnURL;
            ViewData["CType"] = CType;
            ViewData["BillingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName");
            ViewData["BillingProvinceID"] = GetProvincesSelectList();
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms.OrderBy(t => t.Order), "BillingTermID", "Terms");
            ViewData["CurrencyID"] = new SelectList(_context.Currencies.OrderBy(t => t.Order), "CurrencyID", "CurrencyName");
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName");
            ViewData["ShippingProvinceID"] = GetProvincesSelectList();
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes.OrderBy(t => t.Order), "CustomerTypeID", "Type");
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes.OrderBy(t => t.Order), "ContractorTypeID", "Type");
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes.OrderBy(t => t.Order), "VendorTypeID", "Type");

            PopulateAssignedCustomerTypes(company);
            PopulateAssignedContractorTypes(company);
            PopulateAssignedVendorTypes(company);
            return View();
        }
        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Create([Bind("CompanyID,Name,Location,CreditCheck,DateChecked,BillingTermID,CurrencyID,Phone,Website,BillingAddress1,BillingAddress2,BillingProvinceID,BillingPostalCode,BillingCountryID,ShippingAddress1,ShippingAddress2,ShippingProvinceID,ShippingPostalCode,ShippingCountryID,Customer,CustomerTypeID,Vendor,VendorTypeID,Contractor,ContractorTypeID,Active,Notes")] 
        Company company, string CType, string returnURL, string[] selectedOptions, string[] selectedOptionsCont, string[] selectedOptionsVen)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Companies");
            //ViewData["returnURL"] = returnURL;
            try
            {
                UpdateCustomerType(selectedOptions, company);
                UpdateContractorType(selectedOptionsCont, company);
                UpdateVendorType(selectedOptionsVen, company);
                if (ModelState.IsValid)
                {
                    _context.Add(company);
                    await _context.SaveChangesAsync();
                    //If no referrer then go back to index
                    //if (String.IsNullOrEmpty(returnURL))
                    //{
                    //    return RedirectToAction(nameof(Index), new { CType = CType });
                    //}
                    //else
                    //{
                    //    return Redirect(returnURL);
                    //}
                    //return RedirectToAction("Details", new { company.CompanyID });
                    return Redirect(ViewData["returnURL"].ToString());
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
            ViewData["BillingProvinceID"] = GetProvincesSelectList(company.BillingProvinceID);
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms, "BillingTermID", "Terms", company.BillingTermID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyID", company.CurrencyID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", company.ShippingProvinceID);
            ViewData["ShippingProvinceID"] = GetProvincesSelectList(company.ShippingProvinceID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "CustomerTypeID", company.CustomerTypeID);
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "ContractorTypeID", company.ContractorTypeID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "VendorTypeID", company.VendorTypeID);

            PopulateAssignedCustomerTypes(company);
            PopulateAssignedContractorTypes(company);
            PopulateAssignedVendorTypes(company);
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Edit(int? id, string CType, string returnURL)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Companies");

            ViewData["CType"] = CType;

            ////Get the URL of the page that send us here
            //if (String.IsNullOrEmpty(returnURL))
            //{
            //    returnURL = Request.Headers["Referer"].ToString();
            //}
            //ViewData["returnURL"] = returnURL;

            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.CompanyCustomers).ThenInclude( c=> c.CustomerType)
                .Include( c => c.CompanyContractors).ThenInclude( c => c.ContractorType)
                .Include(c => c.CompanyVendors).ThenInclude(c => c.VendorType)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.CompanyID == id);

            if (company == null)
            {
                return NotFound();
            }
            ViewData["BillingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.BillingCountryID);
            ViewData["BillingProvinceID"] = GetProvincesSelectList(company.BillingProvinceID);
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms, "BillingTermID", "Terms", company.BillingTermID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", company.CurrencyID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.ShippingCountryID);
            ViewData["ShippingProvinceID"] = GetProvincesSelectList(company.ShippingProvinceID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type", company.CustomerTypeID);
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type", company.ContractorTypeID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "Type", company.VendorTypeID);

            PopulateAssignedCustomerTypes(company);
            PopulateAssignedContractorTypes(company);
            PopulateAssignedVendorTypes(company);
            return View(company);
        }
        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Edit(int id, string CType, string returnURL, string[] selectedOptions, string[] selectedOptionsCont, string[] selectedOptionsVen)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Companies");

            //ViewData["returnURL"] = returnURL;
            var companyToUpdate = await _context.Companies
                .Include(c => c.BillingTerm)
                .Include(c => c.Currency)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.ShippingCountry)
                .Include(c => c.CompanyCustomers).ThenInclude(c => c.CustomerType)
                .Include(c => c.CompanyContractors).ThenInclude( c=> c.ContractorType)
                .Include( c=> c.CompanyVendors).ThenInclude(c => c.VendorType)
                .SingleOrDefaultAsync(c => c.CompanyID == id);

            if (companyToUpdate == null)
            {
                return NotFound();
            }

            UpdateCustomerType(selectedOptions, companyToUpdate);
            UpdateContractorType(selectedOptionsCont, companyToUpdate);
            UpdateVendorType(selectedOptionsVen, companyToUpdate);

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
                    //if(String.IsNullOrEmpty(returnURL))
                    //{
                    //    return RedirectToAction(nameof(Index), new { CType = CType });
                    //}
                    //else
                    //{
                    //    return Redirect(returnURL);
                    //}

                    //return RedirectToAction("Details", new { companyToUpdate.CompanyID });
                    return Redirect(ViewData["returnURL"].ToString());
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
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
            ViewData["BillingProvinceID"] = GetProvincesSelectList(companyToUpdate.BillingProvinceID);
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms, "BillingTermID", "Terms", companyToUpdate.BillingTermID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", companyToUpdate.CurrencyID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", companyToUpdate.ShippingCountryID);
            ViewData["ShippingProvinceID"] = GetProvincesSelectList(companyToUpdate.ShippingProvinceID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type", companyToUpdate.CustomerTypeID);
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type", companyToUpdate.ContractorTypeID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "Type", companyToUpdate.VendorTypeID);

            PopulateAssignedCustomerTypes(companyToUpdate);
            PopulateAssignedContractorTypes(companyToUpdate);
            PopulateAssignedVendorTypes(companyToUpdate);
            return View(companyToUpdate);
        }
        // GET: Companies/Delete/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Delete(int? id, string returnURL)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Companies");

            ////Get the URL of the page that send us here
            //if (String.IsNullOrEmpty(returnURL))
            //{
            //    returnURL = Request.Headers["Referer"].ToString();
            //}
            //ViewData["returnURL"] = returnURL;

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
        public async Task<IActionResult> DeleteConfirmed(int id, string CType, string returnURL)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Companies");

            //ViewData["returnURL"] = returnURL;
            var company = await _context.Companies.FindAsync(id);
            try
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                //if(String.IsNullOrEmpty(returnURL))
                //{
                //    return RedirectToAction(nameof(Index), new { CType = CType });
                //}
                //else
                //{
                //    return Redirect(returnURL);
                //}
                return Redirect(ViewData["returnURL"].ToString());

            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Company. You cannot delete a Companies that has contacts assigned.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(company);

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
            var leftCompany = _context.Companies
                .Include(c => c.CompanyContractors)
                .ThenInclude(c => c.ContractorType)
                .Include(c => c.CompanyCustomers)
                .ThenInclude(c => c.CustomerType)
                .Include(c => c.CompanyVendors)
                .ThenInclude(c => c.VendorType)
                .FirstOrDefault(c => c.CompanyID == leftCompanyId);
            var rightCompany = _context.Companies
                .Include(c => c.CompanyContractors)
                .ThenInclude(c => c.ContractorType)
                .Include(c => c.CompanyCustomers)
                .ThenInclude(c => c.CustomerType)
                .Include(c => c.CompanyVendors)
                .ThenInclude(c => c.VendorType)
                .FirstOrDefault(c => c.CompanyID == rightCompanyId);
            if (leftCompany == null || rightCompany == null)
                return NotFound();

            ViewData["LeftMerge"] = new Dictionary<string, SelectList>
            {
                { "BillingCountryID", new SelectList(_context.Countries, "CountryID", "CountryName", leftCompany.BillingCountryID) },
                { "BillingProvinceID", new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", leftCompany.BillingProvinceID) },
                { "BillingTermID", new SelectList(_context.BillingTerms, "BillingTermID", "Terms", leftCompany.BillingTermID) },
                { "ContractorTypes", new SelectList(leftCompany.CompanyContractors.Select(c => c.ContractorType), 
                    "ContractorTypeID", "Type") },
                { "CustomerTypes", new SelectList(leftCompany.CompanyCustomers.Select(c => c.CustomerType), 
                    "CustomerTypeID", "Type") },
                { "VendorTypes", new SelectList(leftCompany.CompanyVendors.Select(c => c.VendorType), 
                    "VendorTypeID", "Type") },
                { "CurrencyID", new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", leftCompany.CurrencyID) },
                { "ShippingCountryID", new SelectList(_context.Countries, "CountryID", "CountryName", leftCompany.ShippingCountryID) },
                { "ShippingProvinceID", new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", leftCompany.ShippingProvinceID) },
            };
            ViewData["RightMerge"] = new Dictionary<string, SelectList>
            {
                { "BillingCountryID", new SelectList(_context.Countries, "CountryID", "CountryName", rightCompany.BillingCountryID) },
                { "BillingProvinceID", new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", rightCompany.BillingProvinceID) },
                { "BillingTermID", new SelectList(_context.BillingTerms, "BillingTermID", "Terms", rightCompany.BillingTermID) },
                { "ContractorTypes", new SelectList(rightCompany.CompanyContractors.Select(c => c.ContractorType), 
                    "ContractorTypeID", "Type") },
                { "CustomerTypes", new SelectList(rightCompany.CompanyCustomers.Select(c => c.CustomerType), 
                    "CustomerTypeID", "Type") },
                { "VendorTypes", new SelectList(rightCompany.CompanyVendors.Select(c => c.VendorType), 
                    "VendorTypeID", "Type") },
                { "CurrencyID", new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", rightCompany.CurrencyID) },
                { "ShippingCountryID", new SelectList(_context.Countries, "CountryID", "CountryName", rightCompany.ShippingCountryID) },
                { "ShippingProvinceID", new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", rightCompany.ShippingProvinceID) },
            };
            return View(new Company[]{ leftCompany, rightCompany });
        }

        [Authorize(Roles = "Admin, Supervisor")]
        [HttpPost]
        [Route("Company/Merge/{leftCompanyId}/to/{rightCompanyId}")]
        public async Task<IActionResult> Merge(int leftCompanyId, int rightCompanyId, string[] fieldsToMerge)
        {
            var leftCompany = _context.Companies
                .Include(c => c.Contacts)
                .Include(c => c.CompanyContractors)
                .Include(c => c.CompanyCustomers)
                .Include(c => c.CompanyVendors)
                .FirstOrDefault(c => c.CompanyID == leftCompanyId);
            var rightCompany = _context.Companies
                .Include(c => c.Contacts)
                .Include(c => c.CompanyContractors)
                .Include(c => c.CompanyCustomers)
                .Include(c => c.CompanyVendors)
                .FirstOrDefault(c => c.CompanyID == rightCompanyId);
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
                    case "CustomerTypes":
                        rightCompany.CustomerTypeID = leftCompany.CustomerTypeID;
                        leftCompany.CompanyCustomers.ToList()
                            .ForEach(c => {
                                if (rightCompany.CompanyCustomers.All(rc => rc.CustomerTypeID != c.CustomerTypeID))
                                    rightCompany.CompanyCustomers.Add(new CompanyCustomer
                                        {CompanyID = rightCompany.CompanyID, CustomerTypeID = c.CustomerTypeID});
                            });
                        break;
                    case "Vendor":
                        rightCompany.Vendor = leftCompany.Vendor;
                        break;
                    case "VendorTypes":
                        leftCompany.CompanyVendors.ToList()
                            .ForEach(c => {
                                if (rightCompany.CompanyVendors.All(rc => rc.VendorTypeID != c.VendorTypeID))
                                    rightCompany.CompanyVendors.Add(new CompanyVendor
                                        {CompanyID = rightCompany.CompanyID, VendorTypeID = c.VendorTypeID});
                            });
                        break;
                    case "Contractor":
                        rightCompany.Contractor = leftCompany.Contractor;
                        break;
                    case "ContractorTypes":
                        leftCompany.CompanyContractors.ToList()
                            .ForEach(c => {
                                if (rightCompany.CompanyContractors.All(rc => rc.ContractorTypeID != c.ContractorTypeID))
                                    rightCompany.CompanyContractors.Add(new CompanyContractor
                                        {CompanyID = rightCompany.CompanyID, ContractorTypeID = c.ContractorTypeID});
                            });
                        break;
                    case "Active":
                        rightCompany.Active = leftCompany.Active;
                        break;
                    case "Notes":
                        rightCompany.Notes = leftCompany.Notes;
                        break;
                }

            if (leftCompany.Contacts.Any())
            {
                // To do in query updates you need to install separate package EFCore Plus lol
                // _context.Contacts.Where(c => c.CompanyID == leftCompany.CompanyID).Update();
                // I have to query all entities and change them manually...
                var contacts = await _context.Contacts
                    .Where(c => c.CompanyID == leftCompany.CompanyID)
                    .ToListAsync();
                contacts.ForEach(c => c.CompanyID = rightCompany.CompanyID);
                await _context.SaveChangesAsync();
            }
            _context.Companies.Remove(leftCompany);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = rightCompanyId, returnURL = Url.Action(nameof(Index), new { CType="All" }) });
        }

        //GET: Redirect using company name to Contact's Index page with filter set for Company's name.
        public async Task<IActionResult> Contacts(int id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyID == id);
            string name = company.Name;
            return RedirectToAction("Index", "Contacts", new { CompanyName = name });
        }

        // Customer Types
        private void PopulateAssignedCustomerTypes(Company company)
        {
            var allOptions = _context.CustomerTypes;
            var currentOptionsHS = new HashSet<int>(company.CompanyCustomers.Select(s => s.CustomerTypeID));
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();
            foreach (var s in allOptions)
            {
                if (currentOptionsHS.Contains(s.CustomerTypeID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = s.CustomerTypeID,
                        DisplayText = s.Type
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = s.CustomerTypeID,
                        DisplayText = s.Type
                    });
                }
            }

            ViewData["selOptsCust"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOptsCust"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }
        private void UpdateCustomerType(string[] selectedOptions, Company companyToUpdate)
        {
            if (selectedOptions == null)
            {
                companyToUpdate.CompanyCustomers = new List<CompanyCustomer>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var currentOptionsHS = new HashSet<int>(companyToUpdate.CompanyCustomers.Select(s => s.CustomerTypeID));
            foreach (var s in _context.CustomerTypes)
            {
                if (selectedOptionsHS.Contains(s.CustomerTypeID.ToString()))
                {
                    if (!currentOptionsHS.Contains(s.CustomerTypeID))
                    {
                        companyToUpdate.CompanyCustomers.Add(new CompanyCustomer
                        {
                            CustomerTypeID = s.CustomerTypeID,
                            CompanyID = companyToUpdate.CompanyID
                        });
                    }
                }
                else
                {
                    if (currentOptionsHS.Contains(s.CustomerTypeID))
                    {
                        CompanyCustomer specToRemove = companyToUpdate.CompanyCustomers.SingleOrDefault(c => c.CustomerTypeID == s.CustomerTypeID);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }

        //Contractor Types
        private void PopulateAssignedContractorTypes(Company company)
        {
            var allOptions = _context.ContractorTypes;
            var currentOptionsHS = new HashSet<int>(company.CompanyContractors.Select(s => s.ContractorTypeID));
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();

            foreach (var s in allOptions)
            {
                if (currentOptionsHS.Contains(s.ContractorTypeID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = s.ContractorTypeID,
                        DisplayText = s.Type
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = s.ContractorTypeID,
                        DisplayText = s.Type
                    });
                }
            }

            ViewData["selOptsCont"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOptsCont"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }
        private void UpdateContractorType(string[] selectedOptionsContractor, Company companyToUpdate)
        {
            if (selectedOptionsContractor == null)
            {
                companyToUpdate.CompanyContractors = new List<CompanyContractor>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptionsContractor);
            var currentOptionsHS = new HashSet<int>(companyToUpdate.CompanyContractors.Select(s => s.ContractorTypeID));
            foreach (var s in _context.ContractorTypes)
            {
                if (selectedOptionsHS.Contains(s.ContractorTypeID.ToString()))
                {
                    if (!currentOptionsHS.Contains(s.ContractorTypeID))
                    {
                        companyToUpdate.CompanyContractors.Add(new CompanyContractor
                        {
                            ContractorTypeID = s.ContractorTypeID,
                            CompanyID = companyToUpdate.CompanyID
                        });
                    }
                }
                else
                {
                    if (currentOptionsHS.Contains(s.ContractorTypeID))
                    {
                        CompanyContractor specToRemove = companyToUpdate.CompanyContractors.SingleOrDefault(c => c.ContractorTypeID == s.ContractorTypeID);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }

        //Vendor Types
        private void PopulateAssignedVendorTypes(Company company)
        {
            var allOptions = _context.VendorTypes;
            var currentOptionsHS = new HashSet<int>(company.CompanyVendors.Select(s => s.VendorTypeID));
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();

            foreach (var s in allOptions)
            {
                if (currentOptionsHS.Contains(s.VendorTypeID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = s.VendorTypeID,
                        DisplayText = s.Type
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = s.VendorTypeID,
                        DisplayText = s.Type
                    });
                }
            }

            ViewData["selOptsVen"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOptsVen"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }
        private void UpdateVendorType(string[] selectedOptionsVen, Company companyToUpdate)
        {
            if (selectedOptionsVen == null)
            {
                companyToUpdate.CompanyVendors = new List<CompanyVendor>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptionsVen);
            var currentOptionsHS = new HashSet<int>(companyToUpdate.CompanyVendors.Select(s => s.VendorTypeID));
            foreach (var s in _context.VendorTypes)
            {
                if (selectedOptionsHS.Contains(s.VendorTypeID.ToString()))
                {
                    if (!currentOptionsHS.Contains(s.VendorTypeID))
                    {
                        companyToUpdate.CompanyVendors.Add(new CompanyVendor
                        {
                            VendorTypeID = s.VendorTypeID,
                            CompanyID = companyToUpdate.CompanyID
                        });
                    }
                }
                else
                {
                    if (currentOptionsHS.Contains(s.VendorTypeID))
                    {
                        CompanyVendor specToRemove = companyToUpdate.CompanyVendors.SingleOrDefault(v => v.VendorTypeID == s.VendorTypeID);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }

        //Partial View for Companies details page: CustomerTypes, ContractorTypes and VendorTypes list
        public PartialViewResult ListOfCustomerTypesDetails(int id)
        {
            var query = from s in _context.CompanyCustomers.Include(p => p.CustomerType)
                        where s.CompanyID == id
                        orderby s.CustomerType.Type
                        select s;

            return PartialView("_ListOfCustomerTypes", query.ToList());
        }
        public PartialViewResult ListOfContractorTypesDetails(int id)
        {
            var query = from s in _context.CompanyContractors.Include(p => p.ContractorType)
                        where s.CompanyID == id
                        orderby s.ContractorType.Type
                        select s;

            return PartialView("_ListOfContractorTypes", query.ToList());
        }
        public PartialViewResult ListOfVendorTypesDetails(int id)
        {
            var query = from s in _context.CompanyVendors.Include(p => p.VendorType)
                        where s.CompanyID == id
                        orderby s.VendorType.Type
                        select s;

            return PartialView("_ListOfVendorTypes", query.ToList());
        }


    }
}
