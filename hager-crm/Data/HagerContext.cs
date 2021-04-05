using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hager_crm.Controllers;
using hager_crm.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using hager_crm.Utils;
using hager_crm.ViewModels;

namespace hager_crm.Data
{
    public class HagerContext : DbContext
    {
        private DatabaseExceptionHandler dbExcHandler;

        public HagerContext (DbContextOptions<HagerContext> options)
            : base(options)
        {
            dbExcHandler = new DatabaseExceptionHandler(this);
        }

        public void HandleDatabaseException(Exception dex, ModelStateDictionary modelState)
            => dbExcHandler.HandleDatabaseException(dex, modelState);

        public DbSet<BillingTerm> BillingTerms { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactCategories> ContactCategories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<ContractorType> ContractorTypes { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }
        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
        public DbSet<CompanyContractor> CompanyContractors { get; set; }
        public DbSet<CompanyVendor> CompanyVendors { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementEmployee> AnnouncementEmployees { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        
        public List<ILookupManage> GetLookups() => new List<ILookupManage>
        {
            new BillingTerm(),
            new CustomerType(),
            new VendorType(),
            new ContractorType(),
            new Categories(),
            new JobPosition(),
            new EmploymentType(),
            new Currency()
        };
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Default Schema
            //modelBuilder.HasDefaultSchema("HG");

            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();

            modelBuilder.Entity<ContactCategories>().HasKey(c => new { c.CategoriesID, c.ContactID });

            modelBuilder.Entity<Announcement>().HasMany(a => a.EmployeesUnread)
                .WithOne(a => a.Announcement).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>().HasMany(e => e.UnreadAnnouncements)
                .WithOne(a => a.Employee).OnDelete(DeleteBehavior.Cascade);

            //From Company to Calendar
            modelBuilder.Entity<Company>()
                .HasMany<Calendar>(c => c.Calendars)
                .WithOne(c => c.Company)
                .OnDelete(DeleteBehavior.Cascade);

            //Prevent Cascading Delete
            //From Company to Contacts.
            modelBuilder.Entity<Company>()
                .HasMany<Contact>(d => d.Contacts)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyID)
                .OnDelete(DeleteBehavior.Restrict);

            //From Billing Term to Company.
            modelBuilder.Entity<BillingTerm>()
                .HasMany<Company>(d => d.Companies)
                .WithOne(p => p.BillingTerm)
                .HasForeignKey(p => p.BillingTermID)
                .OnDelete(DeleteBehavior.Restrict);

            //From Country to Company and Country to Employee.
            modelBuilder.Entity<Country>()
                .HasMany<Company>(d => d.Companies)
                .WithOne(p => p.BillingCountry)
                .HasForeignKey(p => p.BillingCountryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Country>()
                .HasMany<Employee>(d => d.Employees)
                .WithOne(p => p.EmployeeCountry)
                .HasForeignKey(p => p.EmployeeCountryID)
                .OnDelete(DeleteBehavior.Restrict);

            //From Province to Company and Province to Employee.
            modelBuilder.Entity<Province>()
                .HasMany<Company>(d => d.Companies)
                .WithOne(p => p.BillingProvince)
                .HasForeignKey(p => p.BillingProvinceID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Province>()
                .HasMany<Employee>(d => d.Employees)
                .WithOne(p => p.EmployeeProvince)
                .HasForeignKey(p => p.EmployeeProvinceID)
                .OnDelete(DeleteBehavior.Restrict);

            //From Job Position to Employee.
            modelBuilder.Entity<JobPosition>()
                .HasMany<Employee>(d => d.Employees)
                .WithOne(p => p.JobPosition)
                .HasForeignKey(p => p.JobPositionID)
                .OnDelete(DeleteBehavior.Restrict);

            //From Job Position to Employee.
            modelBuilder.Entity<EmploymentType>()
                .HasMany<Employee>(d => d.Employees)
                .WithOne(p => p.EmploymentType)
                .HasForeignKey(p => p.EmploymentTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            //From Categories to Contact Categories.
            modelBuilder.Entity<Categories>()
                .HasMany<ContactCategories>(d => d.ContactCategories)
                .WithOne(p => p.Categories)
                .HasForeignKey(p => p.CategoriesID)
                .OnDelete(DeleteBehavior.Restrict);

            //////////////////////////////////////////////////////////////////
            // Many to Many Companies type PK
            modelBuilder.Entity<CompanyCustomer>()
                .HasKey(t => new { t.CompanyID, t.CustomerTypeID});

            modelBuilder.Entity<CompanyContractor>()
                .HasKey(t => new { t.CompanyID, t.ContractorTypeID });

            modelBuilder.Entity<CompanyVendor>()
                .HasKey(t => new { t.CompanyID, t.VendorTypeID});

            // Cascade Delete Customer Type
            modelBuilder.Entity<CustomerType>()
                .HasMany<CompanyCustomer>(c => c.CompanyCustomers)
                .WithOne(c => c.CustomerType)
                .HasForeignKey(c => c.CustomerTypeID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>()
                .HasMany<CompanyCustomer>(c => c.CompanyCustomers)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade Delete Contractor Type
            modelBuilder.Entity<ContractorType>()
                .HasMany<CompanyContractor>(c => c.CompanyContractors)
                .WithOne(c => c.ContractorType)
                .HasForeignKey(c => c.ContractorTypeID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>()
                .HasMany<CompanyContractor>(c => c.CompanyContractors)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade Delete Vendor Type
            modelBuilder.Entity<VendorType>()
                .HasMany<CompanyVendor>(c => c.CompanyVendors)
                .WithOne(c => c.VendorType)
                .HasForeignKey(c => c.VendorTypeID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>()
                .HasMany<CompanyVendor>(c => c.CompanyVendors)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
