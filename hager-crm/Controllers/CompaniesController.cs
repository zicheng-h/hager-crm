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
            IQueryable<Company> query = _context.Companies
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

            _gridFilter.ParseQuery(HttpContext);
            ViewBag.gridFilter = _gridFilter;
            ViewData["CountryID"] = GetCountriesSelectList();
            ViewData["ProvinceID"] = GetProvincesSelectList();
            ViewData["DuplicationCompany"] = GetSimillarCompanies(await _context.Companies.OrderBy(c => c.CompanyID).Take(20).ToListAsync());
            return View(await _gridFilter.GetFilteredData(query));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id, string returnURL)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get the URL of the page that send us here
            if (String.IsNullOrEmpty(returnURL))
            {
                returnURL = Request.Headers["Referer"].ToString();
            }
            ViewData["returnURL"] = returnURL;

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
        public IActionResult Create(string CType, string returnURL)
        {

            //Get the URL of the page that send us here
            if (String.IsNullOrEmpty(returnURL))
            {
                returnURL = Request.Headers["Referer"].ToString();
            }
            ViewData["returnURL"] = returnURL;
            ViewData["CType"] = CType;
            ViewData["BillingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName");
            ViewData["BillingProvinceID"] = GetProvincesSelectList();
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms.OrderBy(t => t.Order), "BillingTermID", "Terms");
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes.OrderBy(t => t.Order), "ContractorTypeID", "Type");
            ViewData["CurrencyID"] = new SelectList(_context.Currencies.OrderBy(t => t.Order), "CurrencyID", "CurrencyName");
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes.OrderBy(t => t.Order), "CustomerTypeID", "Type");
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName");
            ViewData["ShippingProvinceID"] = GetProvincesSelectList();
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes.OrderBy(t => t.Order), "VendorTypeID", "Type");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Create([Bind("CompanyID,Name,Location,CreditCheck,DateChecked,BillingTermID,CurrencyID,Phone,Website,BillingAddress1,BillingAddress2,BillingProvinceID,BillingPostalCode,BillingCountryID,ShippingAddress1,ShippingAddress2,ShippingProvinceID,ShippingPostalCode,ShippingCountryID,Customer,CustomerTypeID,Vendor,VendorTypeID,Contractor,ContractorTypeID,Active,Notes")] Company company, string CType, string returnURL)
        {
            ViewData["returnURL"] = returnURL;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(company);
                    await _context.SaveChangesAsync();
                    //If no referrer then go back to index
                    if (String.IsNullOrEmpty(returnURL))
                    {
                        return RedirectToAction(nameof(Index), new { CType = CType });
                    }
                    else
                    {
                        return Redirect(returnURL);
                    }
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
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "ContractorTypeID", company.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyID", company.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "CustomerTypeID", company.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", company.ShippingProvinceID);
            ViewData["ShippingProvinceID"] = GetProvincesSelectList(company.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "VendorTypeID", company.VendorTypeID);
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Edit(int? id, string CType, string returnURL)
        {
            ViewData["CType"] = CType;

            //Get the URL of the page that send us here
            if (String.IsNullOrEmpty(returnURL))
            {
                returnURL = Request.Headers["Referer"].ToString();
            }
            ViewData["returnURL"] = returnURL;

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
            ViewData["BillingProvinceID"] = GetProvincesSelectList(company.BillingProvinceID);
            ViewData["BillingTermID"] = new SelectList(_context.BillingTerms, "BillingTermID", "Terms", company.BillingTermID);
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type", company.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", company.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type", company.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", company.ShippingCountryID);
            ViewData["ShippingProvinceID"] = GetProvincesSelectList(company.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "Type", company.VendorTypeID);
            return View(company);
        }
        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Edit(int id, string CType, string returnURL)
        {
            ViewData["returnURL"] = returnURL;
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
                    if(String.IsNullOrEmpty(returnURL))
                    {
                        return RedirectToAction(nameof(Index), new { CType = CType });
                    }
                    else
                    {
                        return Redirect(returnURL);
                    }
                    
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
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "Type", companyToUpdate.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyName", companyToUpdate.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "Type", companyToUpdate.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", companyToUpdate.ShippingCountryID);
            ViewData["ShippingProvinceID"] = GetProvincesSelectList(companyToUpdate.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "Type", companyToUpdate.VendorTypeID);
            return View(companyToUpdate);
        }
        // GET: Companies/Delete/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Delete(int? id, string returnURL)
        {
            //Get the URL of the page that send us here
            if (String.IsNullOrEmpty(returnURL))
            {
                returnURL = Request.Headers["Referer"].ToString();
            }
            ViewData["returnURL"] = returnURL;

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
            ViewData["returnURL"] = returnURL;
            var company = await _context.Companies.FindAsync(id);
            try
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                if(String.IsNullOrEmpty(returnURL))
                {
                    return RedirectToAction(nameof(Index), new { CType = CType });
                }
                else
                {
                    return Redirect(returnURL);
                }
                
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
            var leftCompany = _context.Companies
                .Include(c => c.Contacts)
                .FirstOrDefault(c => c.CompanyID == leftCompanyId);
            var rightCompany = _context.Companies
                .Include(c => c.Contacts)
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


            return RedirectToAction(nameof(Details), new { id = rightCompanyId });
        }

        //GET: Redirect using company name to Contact's Index page with filter set for Company's name.
        public async Task<IActionResult> Contacts(int id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyID == id);
            string name = company.Name;
            return RedirectToAction("Index", "Contacts", new { CompanyName = name });
        }
    }
}
