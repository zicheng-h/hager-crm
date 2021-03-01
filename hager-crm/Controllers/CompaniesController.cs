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

namespace hager_crm.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly HagerContext _context;

        public CompaniesController(HagerContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index(string SearchString, int? ProvinceID, int? CountryID
            , int? page, int? pageSizeID, string actionButton
            , string sortDirection = "asc", string sortField = "Name")
        {
            PopulateDropDownListsProvince();
            PopulateDropDownListsCountry();
            ViewData["Filtering"] = "";
            var hagerContext = from c in _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.ContractorType)
                .Include(c => c.Currency)
                .Include(c => c.CustomerType)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.VendorType)
                .OrderBy(c => c.Name)
                select c;

            //Add Filter by Province
            if(ProvinceID.HasValue)
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

            else if (sortField == "BillingProvince")
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

            else if (sortField == "BillingCountry")
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

        public async Task<IActionResult> IndexCustomer(string SearchString, int? ProvinceID, int? CountryID
            , int? page, int? pageSizeID, string actionButton
            , string sortDirection = "asc", string sortField = "Name")
        {
            PopulateDropDownListsProvince();
            PopulateDropDownListsCountry();
            ViewData["Filtering"] = "";
            var hagerContext = from c in _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.ContractorType)
                .Include(c => c.Currency)
                .Include(c => c.CustomerType)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.VendorType)
                .OrderBy(c => c.Name)
                .Where(c => c.Customer == true)
                               select c;

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

            if (!String.IsNullOrEmpty(SearchString))
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
            if (sortField == "Name")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(b => b.Name);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(b => b.Name);
                }
            }
            else if (sortField == "Location")
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

            else if (sortField == "BillingProvince")
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

            else if (sortField == "BillingCountry")
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


        public async Task<IActionResult> IndexVendor(string SearchString, int? ProvinceID, int? CountryID
            , int? page, int? pageSizeID, string actionButton
            , string sortDirection = "asc", string sortField = "Name")
        {
            PopulateDropDownListsProvince();
            PopulateDropDownListsCountry();
            ViewData["Filtering"] = "";
            var hagerContext = from c in _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.ContractorType)
                .Include(c => c.Currency)
                .Include(c => c.CustomerType)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.VendorType)
                .OrderBy(c => c.Name)
                .Where(c => c.Vendor == true)
                               select c;

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

            if (!String.IsNullOrEmpty(SearchString))
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
            if (sortField == "Name")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(b => b.Name);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(b => b.Name);
                }
            }
            else if (sortField == "Location")
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

            else if (sortField == "BillingProvince")
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

            else if (sortField == "BillingCountry")
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


        public async Task<IActionResult> IndexContractor(string SearchString, int? ProvinceID, int? CountryID
            , int? page, int? pageSizeID, string actionButton
            , string sortDirection = "asc", string sortField = "Name")
        {
            PopulateDropDownListsProvince();
            PopulateDropDownListsCountry();
            ViewData["Filtering"] = "";
            var hagerContext = from c in _context.Companies
                .Include(c => c.BillingCountry)
                .Include(c => c.BillingProvince)
                .Include(c => c.BillingTerm)
                .Include(c => c.ContractorType)
                .Include(c => c.Currency)
                .Include(c => c.CustomerType)
                .Include(c => c.ShippingCountry)
                .Include(c => c.ShippingProvince)
                .Include(c => c.VendorType)
                .OrderBy(c => c.Name)
                .Where(c => c.Contractor == true)
                               select c;

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

            if (!String.IsNullOrEmpty(SearchString))
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
            if (sortField == "Name")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(b => b.Name);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(b => b.Name);
                }
            }
            else if (sortField == "Location")
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

            else if (sortField == "BillingProvince")
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

            else if (sortField == "BillingCountry")
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
        public async Task<IActionResult> Details(int? id)
        {
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

        public async Task<IActionResult> DetailsCustomer(int? id)
        {
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

        public async Task<IActionResult> DetailsVendor(int? id)
        {
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

        public async Task<IActionResult> DetailsContractor(int? id)
        {
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
        public IActionResult Create()
        {
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
        public async Task<IActionResult> Edit(int? id)
        {
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
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "ContractorTypeID", companyToUpdate.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyID", companyToUpdate.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "CustomerTypeID", companyToUpdate.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", companyToUpdate.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", companyToUpdate.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "VendorTypeID", companyToUpdate.VendorTypeID);
            return View(companyToUpdate);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> EditCustomer(int? id)
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(int id)
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

            if (await TryUpdateModelAsync<Company>(companyToUpdate, "",
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
                    return RedirectToAction(nameof(IndexCustomer));
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
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "ContractorTypeID", companyToUpdate.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyID", companyToUpdate.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "CustomerTypeID", companyToUpdate.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", companyToUpdate.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", companyToUpdate.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "VendorTypeID", companyToUpdate.VendorTypeID);
            return View(companyToUpdate);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> EditVendor(int? id)
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVendor(int id)
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

            if (await TryUpdateModelAsync<Company>(companyToUpdate, "",
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
                    return RedirectToAction(nameof(IndexVendor));
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
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "ContractorTypeID", companyToUpdate.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyID", companyToUpdate.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "CustomerTypeID", companyToUpdate.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", companyToUpdate.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", companyToUpdate.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "VendorTypeID", companyToUpdate.VendorTypeID);
            return View(companyToUpdate);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> EditContractor(int? id)
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContractor(int id)
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

            if (await TryUpdateModelAsync<Company>(companyToUpdate, "",
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
                    return RedirectToAction(nameof(IndexContractor));
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
            ViewData["ContractorTypeID"] = new SelectList(_context.ContractorTypes, "ContractorTypeID", "ContractorTypeID", companyToUpdate.ContractorTypeID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies, "CurrencyID", "CurrencyID", companyToUpdate.CurrencyID);
            ViewData["CustomerTypeID"] = new SelectList(_context.CustomerTypes, "CustomerTypeID", "CustomerTypeID", companyToUpdate.CustomerTypeID);
            ViewData["ShippingCountryID"] = new SelectList(_context.Countries, "CountryID", "CountryName", companyToUpdate.ShippingCountryID);
            ViewData["ShippingProvinceID"] = new SelectList(_context.Provinces, "ProvinceID", "ProvinceName", companyToUpdate.ShippingProvinceID);
            ViewData["VendorTypeID"] = new SelectList(_context.VendorTypes, "VendorTypeID", "VendorTypeID", companyToUpdate.VendorTypeID);
            return View(companyToUpdate);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCustomer(int? id)
        {
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
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexCustomer));
        }

        public async Task<IActionResult> DeleteVendor(int? id)
        {
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
        [HttpPost, ActionName("DeleteVendor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVendorConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexVendor));
        }

        public async Task<IActionResult> DeleteContractor(int? id)
        {
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
        [HttpPost, ActionName("DeleteContractor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContractorConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexContractor));
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

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyID == id);
        }

    }
}
