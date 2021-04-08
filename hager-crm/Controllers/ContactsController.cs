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
using hager_crm.Models.FilterConfig;
//Filters


namespace hager_crm.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly HagerContext _context;
        private readonly GridFilter<Contact> _gridFilter;


        public ContactsController(HagerContext context)
        {
            _context = context;
            _gridFilter = new GridFilter<Contact>(new ContactConfig(), 5);
        }
        private SelectList GetCategoriesSelectList(object selectedValue = null) =>
            new SelectList(_context.Categories.OrderBy(i => i.Order), "ID", "Category", selectedValue);

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            //Clear the sort/filter/paging URL Cookie
            CookieHelper.CookieSet(HttpContext, "ContactsURL", "", -1);
            IQueryable<Contact> query = _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.ContactCategories).ThenInclude(c => c.Categories)
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName);

            _gridFilter.ParseQuery(HttpContext);
            ViewBag.gridFilter = _gridFilter;
            ViewData["CategoriesID"] = GetCategoriesSelectList();

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

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Contacts");

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
        public IActionResult Create()
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Contacts");

            ////Get the URL of the page that send us here
            //if (String.IsNullOrEmpty(returnURL))
            //{
            //    returnURL = Request.Headers["Referer"].ToString();
            //}
            //ViewData["returnURL"] = returnURL;

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
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Contacts");

            //ViewData["returnURL"] = returnURL;
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
        public async Task<IActionResult> Edit(int? id)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Contacts");

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
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Contacts");

            //ViewData["returnURL"] = returnURL;
            var contactToUpdate = await _context.Contacts
                .Include(c => c.ContactCategories).ThenInclude(c => c.Categories)
                .SingleOrDefaultAsync(c => c.ContactID == id);

            if (id != contact.ContactID)
            {
                return NotFound();
            }

            UpdateContactCategory(selectedOptions, contactToUpdate); // Updateing categories

            if (await TryUpdateModelAsync<Contact>(contactToUpdate, "", c => c.FirstName, c => c.LastName, c => c.JobTitle,
                    c => c.CellPhone, c => c.WorkPhone, c => c.Email, c => c.Active, c => c.Notes, c => c.CompanyID))
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

            ViewData["CompanyID"] = new SelectList(_context.Companies, "CompanyID", "Name", contact.CompanyID);
            PopulateAssignedCategoryData(contactToUpdate);
            return View(contactToUpdate);
        }

        // GET: Contacts/Delete/5
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Contacts");

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
            //Get the URL with the last filter, sort and page parameters
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "Contacts");

            //ViewData["returnURL"] = returnURL;
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
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
            var contactCategoriesHS = new HashSet<int>(contactToUpdate.ContactCategories.Select(c => c.CategoriesID)); // current category selected

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
