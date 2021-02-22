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

namespace hager_crm.Controllers
{
    public class ContactsController : Controller
    {
        private readonly HagerContext _context;

        public ContactsController(HagerContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var hagerContext = _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.ContactCategories).ThenInclude(c => c.Categories);
            return View(await hagerContext.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
        public IActionResult Create()
        {
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
        public async Task<IActionResult> Edit(int? id)
        {
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactID,FirstName,LastName,JobTitle,CellPhone,WorkPhone,Email,Active,Notes,CompanyID")] Contact contact,
            string[] selectedOptions)
        {
            var contactToUpdate = await _context.Contacts
                .Include(c => c.ContactCategories)
                .ThenInclude(c => c.Categories)
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
        public async Task<IActionResult> Delete(int? id)
        {
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
                        contactToUpdate.ContactCategories.Add(new ContactCategories{ ContactID = contactToUpdate.ContactID, CategoriesID = option.ID });
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
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactID == id);
        }
    }
}
