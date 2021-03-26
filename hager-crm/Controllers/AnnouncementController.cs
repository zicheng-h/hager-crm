using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using hager_crm.Data;
using hager_crm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace hager_crm.Controllers
{
    [Authorize]
    public class AnnouncementController : Controller
    {
        private HagerContext _context;
        
        public AnnouncementController(HagerContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUnreadAnnouncements()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound();
            
            var announcements = await _context.Announcements
                .Include(a => a.EmployeesUnread).ThenInclude(a => a.Employee)
                .Where(a => a.EmployeesUnread.Any(e => e.Employee.UserId == userId))
                .OrderByDescending(a => a.PostedAt)
                .ToListAsync();
            return PartialView("~/Views/Home/Announcement/_GetUnreadAnnouncements.cshtml", announcements);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> GetAnnouncements()
        {
            var announcements = await _context.Announcements
                .OrderByDescending(a => a.PostedAt)
                .ToListAsync();
            return PartialView("~/Views/Home/Announcement/_GetAnnouncements.cshtml", announcements);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> PostAnnouncement([Bind("Title", "Message", "Severity")] Announcement announcement)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound();
            
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.Announcements.AddAsync(announcement);
                    await _context.SaveChangesAsync();
                    await NotifyAllEmployees(announcement);
                    return Json(new {status = 200, id = announcement.AnnouncementID});
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unable to create new record.");
                
            }
            return BadRequest(ModelState);
        }
        
        [HttpPost]
        public async Task<IActionResult> ReadAnnouncement(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound();
            
            var announcementEmployee = await _context.AnnouncementEmployees
                .Include(ae => ae.Employee)
                .Where(ae => ae.Employee.UserId == userId && ae.AnnouncementID == id)
                .FirstAsync();
            
            if (announcementEmployee == null)
                return NotFound();
            
            try
            {
                _context.AnnouncementEmployees.Remove(announcementEmployee);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                
            }
            return BadRequest(ModelState);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> UpdateAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
                return NotFound();
            
            if (await TryUpdateModelAsync(announcement,"", 
                a => a.Title, 
                a => a.Message,
                a => a.Severity
                )
            )
            {
                announcement.PostedAt = DateTime.Now;
                try
                {
                    await _context.SaveChangesAsync();
                    await NotifyAllEmployees(announcement);
                    return Ok();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Error", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return BadRequest(ModelState);
            
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
                return NotFound();
            
            try
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return BadRequest(ModelState);
        }
            
        private async Task NotifyAllEmployees(Announcement announcement)
        {
            var employeesToNotify = await _context.Employees.Where(e =>
                e.UnreadAnnouncements.All(a => a.AnnouncementID != announcement.AnnouncementID))
                .ToListAsync();
            foreach (var employee in  employeesToNotify)
            {
                employee.UnreadAnnouncements.Add(new AnnouncementEmployee
                {
                    EmployeeID = employee.EmployeeID, 
                    AnnouncementID = announcement.AnnouncementID
                });
            }
            
            await _context.SaveChangesAsync();
        }
    }
}