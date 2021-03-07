using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hager_crm.Data;
using hager_crm.Models;
using hager_crm.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using hager_crm.Utils;
using Microsoft.AspNetCore.Authorization;
//Filters


namespace hager_crm.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly HagerContext _context;


        public ContactsController(HagerContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index(string SearchString, int? CategoryID, int? page,
            int? pageSizeID, string actionButton, string sortDirection = "asc",
            string sortField = "Name", string ContactType = "All")
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories.OrderBy(c => c.Category), "ID", "Category");
            ViewData["Filtering"] = ""; // Assume not filtering!
            ViewData["ContactType"] = ContactType;

            CookieHelper.CookieSet(HttpContext, "PatientsURL", "", -1);

            //All Contacts
            IQueryable<Contact> hagerContext = _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.ContactCategories).ThenInclude(c => c.Categories)
                .OrderBy(c => c.FirstName);

            switch (ContactType)
            {
                case "All":
                    break;
                case "Customer":
                    hagerContext = hagerContext.Where(c => c.Company.Customer == true);
                    break;
                case "Vendor":
                    hagerContext = hagerContext.Where(c => c.Company.Vendor == true);
                    break;
                case "Contractor":
                    hagerContext = hagerContext.Where(c => c.Company.Contractor == true);
                    break;
                default:  //all other values are invalid
                    return NotFound();
            }

            //Filters
            if (!String.IsNullOrEmpty(SearchString))
            {
                hagerContext = hagerContext.Where(c => c.LastName.ToUpper().Contains(SearchString.ToUpper())
                                           || c.FirstName.ToUpper().Contains(SearchString.ToUpper()));
                ViewData["Filtering"] = " show";
            }
            if (CategoryID.HasValue)
            {
                hagerContext = hagerContext.Where(c => c.ContactCategories.Any(c => c.CategoriesID == CategoryID));
                ViewData["Filtering"] = " show";
            }

            //Before sorting by a specific direction
            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;
                if (actionButton != "Filter")
                {
                    if (actionButton == sortField)
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;
                }
            }

            if (sortField == "Contact")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderBy(c => c.LastName)
                        .ThenBy(p => p.FirstName);
                }
                else
                {
                    hagerContext = hagerContext.OrderByDescending(c => c.LastName)
                        .ThenBy(p => p.FirstName);
                }
            }
            else if (sortField == "Company")
            {
                if (sortDirection == "asc")
                {
                    hagerContext = hagerContext.OrderByDescending(c => c.Company);
                }
                else
                {
                    hagerContext = hagerContext.OrderBy(c => c.Company);
                }
            }
            //else if (sortField == "Category")
            //{
            //    if (sortDirection == "asc")
            //    {
            //        hagerContext = hagerContext.OrderByDescending(c => c.ContactCategories);
            //    }
            //    else
            //    {
            //        hagerContext = hagerContext.OrderBy(c => c.ContactCategories);
            //    }
            //}


            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            //Paginating
            int pageSize;
            if (pageSizeID.HasValue)
            {
                pageSize = pageSizeID.GetValueOrDefault();
                CookieHelper.CookieSet(HttpContext, "pageSizeValue", pageSize.ToString(), 30);
            }
            else
            {
                pageSize = Convert.ToInt32(HttpContext.Request.Cookies["pageSizeValue"]);
            }
            pageSize = (pageSize == 0) ? 3 : pageSize;//Neither Selected or in Cookie so go with default
            ViewData["pageSizeID"] = new SelectList(new[] { "3", "5", "10", "20", "30", "40", "50", "100", "500" }, pageSize.ToString());

            var pagedData = await PaginatedList<Contact>.CreateAsync(hagerContext.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id, string ContactType)
        {
            ViewData["ContactType"] = ContactType;

            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.ContactID == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Create(string ContactType)
        {
            ViewData["ContactType"] = ContactType;
            ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "Name");

            var contact = new Contact();
            PopulateAssignedCategoryData(contact);

            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Create([Bind("ContactID,FirstName,LastName,JobTitle,CellPhone,WorkPhone,Email,Active,Notes,CompanyID")] Contact contact,
            string[] selectedOptions)
        {
            try
            {
                // Adding the multi select categories
                if (selectedOptions != null)
                {
                    foreach (var category in selectedOptions)
                    {
                        var categoryToAdd = new ContactCategories { ContactID = contact.ContactID, CategoriesID = int.Parse(category) };
                        contact.ContactCategories.Add(categoryToAdd);
                    }
                }

                if (ModelState.IsValid)
                {
                    _context.Add(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "Name", contact.CompanyID);
                return View(contact);

            }
            catch (RetryLimitExceededException) // Multiple tries
            {
                ModelState.AddModelError("", "Ineffectual save changes after multiple attempts. Try again. If the problem persists, please contact your system administrator.");
            }

            return View(contact);

        }

        // GET: Contacts/Edit/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Edit(int? id, string ContactType)
        {
            ViewData["ContactType"] = ContactType;

            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.ContactCategories).ThenInclude(c => c.Categories)
                .SingleOrDefaultAsync(c => c.ContactID == id);


            if (contact == null)
            {
                return NotFound();
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "Name", contact.CompanyID);
            PopulateAssignedCategoryData(contact);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Edit(int id, [Bind("ContactID,FirstName,LastName,JobTitle,CellPhone,WorkPhone,Email,Active,Notes,CompanyID")] Contact contact,
            string[] selectedOptions)
        {
            var contactToUpdate = await _context.Contacts
                .Include(c => c.ContactCategories).ThenInclude(c => c.Categories)
                .SingleOrDefaultAsync(c => c.ContactID == id);

            if (id != contact.ContactID)
            {
                return NotFound();
            }

            UpdateContactCategory(selectedOptions, contactToUpdate); // Updateing categories

            if (await TryUpdateModelAsync<Contact>(contactToUpdate, "", c => c.FirstName, c => c.LastName, c => c.JobTitle,
                                                    c => c.CellPhone, c => c.WorkPhone, c => c.Email, c => c.Active, c => c.CompanyID))
            {
                try
                {
                    _context.Update(contactToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Ineffectual save changes after multiple attempts. Try again. If the problem persists, please contact your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "Name", contact.CompanyID);
            PopulateAssignedCategoryData(contactToUpdate);
            return View(contactToUpdate);
        }

        // GET: Contacts/Delete/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Delete(int? id, string ContactType)
        {
            ViewData["ContactType"] = ContactType;

            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.ContactID == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: Redirect using company name to Company's Details page
        public async Task<IActionResult> CompanyDetails(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            return RedirectToAction("Details", "Companies", new { id });
        }

        // Categories Multi Select
        private void PopulateAssignedCategoryData(Contact contact)
        {
            var allOpt = _context.Categories;
            var currentOptionsIDs = new HashSet<int>(contact.ContactCategories.Select(c => c.CategoriesID));
            var checkBoxes = new List<OptionVM>();
            // Looping through options
            foreach (var option in allOpt)
            {
                checkBoxes.Add(new OptionVM
                {
                    ID = option.ID,
                    DisplayText = option.Category,
                    Assigned = currentOptionsIDs.Contains(option.ID)
                });
            }
            ViewData["CategoryOptions"] = checkBoxes;
        }

        // For Updating categories
        private void UpdateContactCategory(string[] selectedOptions, Contact contactToUpdate)
        {
            if (selectedOptions == null) // Openning with a check
            {
                contactToUpdate.ContactCategories = new List<ContactCategories>();
                return;
            }
            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var contactCategoriesHS = new HashSet<int>
                (contactToUpdate.ContactCategories.Select(c => c.CategoriesID)); // current category selected

            foreach (var option in _context.Categories)
            {
                if (selectedOptionsHS.Contains(option.ID.ToString()))
                {
                    if (!contactCategoriesHS.Contains(option.ID))
                    {
                        contactToUpdate.ContactCategories.Add(new ContactCategories { ContactID = contactToUpdate.ContactID, CategoriesID = option.ID });
                    }
                }
                else
                {
                    if (contactCategoriesHS.Contains(option.ID))
                    {
                        ContactCategories categoryToRemove = contactToUpdate.ContactCategories.SingleOrDefault(c => c.CategoriesID == option.ID);
                        _context.Remove(categoryToRemove);
                    }
                }
            }
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactID == id);
        }
    }
}
