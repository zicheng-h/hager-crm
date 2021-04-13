using hager_crm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using hager_crm.Data;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using hager_crm.Utils;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Storage;

namespace hager_crm.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly HagerContext _context;
        
        public HomeController(HagerContext context, ILogger<HomeController> logger)
        {
            _context = context;

            _logger = logger;
        }

        private int GetSimillarCompaniesCount(List<Company> companies)
        {
            int duplicates = 0;
            var pairs = new List<Tuple<Company, Company>>();
            for (int i = 0; i < companies.Count; i++)
            {
                for (int j = i + 1; j < companies.Count; j++)
                    pairs.Add(new Tuple<Company, Company>(companies[i], companies[j]));
            }

            foreach (var pair in pairs)
            {
                if (LevenshteinDistance.Compute(pair.Item1.Name, pair.Item2.Name) <= 2)
                    duplicates += 1;
            }

            return duplicates;
        }

        private string GetEmployee(List<Employee> emp)
        {
            return emp.Count.ToString();
        }

        private string GetCompany(List<Company> comp)
        {
            return comp.Count.ToString();
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Company"] = await _context.Companies.OrderBy(c => c.Name).ToListAsync();
            var expireDate = DateTime.Now + TimeSpan.FromDays(14);
            ViewData["ContractsToExpire"] = await _context.CompanyContractors
                .CountAsync(cc => 
                    cc.ExpiryDate != null 
                    && cc.ExpiryDate <  expireDate
                    && cc.ExpiryDate > DateTime.Now
                    );
            ViewData["ContractsExpired"] = await _context.CompanyContractors
                .CountAsync(cc => cc.ExpiryDate != null && cc.ExpiryDate < DateTime.Now);
            ViewData["DuplicationCompany"] = GetSimillarCompaniesCount(await _context.Companies.OrderBy(c => c.CompanyID).Take(20).ToListAsync());
            ViewData["ActiveEmployee"] = GetEmployee(await _context.Employees.Where(e => e.Active == true).ToListAsync());
            ViewData["ActiveCompany"] = GetCompany(await _context.Companies.Where(e => e.Active == true).ToListAsync());
            ViewData["AllEmployees"] = GetEmployee(await _context.Employees.ToListAsync());
            ViewData["AllCompanies"] = GetCompany(await _context.Companies.ToListAsync());
            var newDate = DateTime.Now - TimeSpan.FromDays(30);
            ViewData["NewCompanies"] = await _context.Companies.CountAsync(c => c.CreatedAt > newDate);
            ViewData["Birthday"] = Statistics.Birthdays(await _context.Employees.OrderByDescending(e => e.DOB).ToListAsync());
            ViewData["Event"] = Statistics.Event(await _context.Calendars.OrderBy(e => e.Date).ToListAsync());
            return View();
        }

        public IActionResult Privacy()
        {
            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
