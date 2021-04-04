using System;
using System.Collections.Generic;
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
    public class CalendarController : Controller
    {
        private HagerContext _context;

        public CalendarController(HagerContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetEventDates()
        //{
        //    var calendars = await _context.Calendars
        //        .Include(c=>c.Company)
        //        .OrderByDescending(a => a.Date)
        //        .ToListAsync();
        //    return PartialView("~/Views/Home/Calendar/_GetCalendar.cshtml", calendars);
        //}

        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> PostCalendar([Bind("Title", "Description", "Date", "CompanyId")] Calendar calendar)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound();
            var title = calendar.Title;
            var desc = calendar.Description;
            var date = calendar.Date;
            var company = calendar.CompanyId;

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.Calendars.AddAsync(calendar);
                    await _context.SaveChangesAsync();
                    return Json(new { status = 200, id = calendar.CalendarId });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Unable to create new event.");

            }
            return BadRequest(ModelState);
        }

        //[HttpPost]
        //public async Task<IActionResult> ReadEvent(int id)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (userId == null)
        //        return NotFound();

        //    var announcementEmployee = await _context.AnnouncementEmployees
        //        .Include(ae => ae.Employee)
        //        .Where(ae => ae.Employee.UserId == userId && ae.AnnouncementID == id)
        //        .FirstAsync();

        //    if (announcementEmployee == null)
        //        return NotFound();

        //    try
        //    {
        //        _context.AnnouncementEmployees.Remove(announcementEmployee);
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {
        //        ModelState.AddModelError("Error", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

        //    }
        //    return BadRequest(ModelState);
        //}

        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public async Task<IActionResult> UpdateEvent(int id)
        {
            var calendar = await _context.Calendars.FindAsync(id);
            if (calendar == null)
                return NotFound();

            if (await TryUpdateModelAsync(calendar, "",
                a => a.Title,
                a => a.Description,
                a => a.CompanyId,
                a => a.Date
                )
            )
            {
                try
                {
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteCalendar(int id)
        {
            var calendar = await _context.Calendars.FindAsync(id);
            if (calendar == null)
                return NotFound();

            try
            {
                _context.Calendars.Remove(calendar);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return BadRequest(ModelState);
        }

        //private async Task NotifyAllEmployees(Announcement announcement)
        //{
        //    var employeesToNotify = await _context.Employees.Where(e =>
        //        e.UnreadAnnouncements.All(a => a.AnnouncementID != announcement.AnnouncementID))
        //        .ToListAsync();
        //    foreach (var employee in employeesToNotify)
        //    {
        //        employee.UnreadAnnouncements.Add(new AnnouncementEmployee
        //        {
        //            EmployeeID = employee.EmployeeID,
        //            AnnouncementID = announcement.AnnouncementID
        //        });
        //    }

        //    await _context.SaveChangesAsync();
        //}
    }
}
