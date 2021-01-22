﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hager_crm.Models;

namespace hager_crm.Data
{
    public class HagerContext : DbContext
    {
        public HagerContext (DbContextOptions<HagerContext> options)
            : base(options)
        {

        }

        public DbSet<BillingTerm> BillingTerms { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactCategories> ContactCategories { get; set; }
        public DbSet<ContractorType> ContractorTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Default Schema
            modelBuilder.HasDefaultSchema("HG");

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

            //From Contractor Type to Company.
            modelBuilder.Entity<ContractorType>()
                .HasMany<Company>(d => d.Companies)
                .WithOne(p => p.ContractorType)
                .HasForeignKey(p => p.ContractorTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            //From Customer Type to Company.
            modelBuilder.Entity<CustomerType>()
                .HasMany<Company>(d => d.Companies)
                .WithOne(p => p.CustomerType)
                .HasForeignKey(p => p.CustomerTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            //From Vendor Type to Company.
            modelBuilder.Entity<VendorType>()
                .HasMany<Company>(d => d.Companies)
                .WithOne(p => p.VendorType)
                .HasForeignKey(p => p.VendorTypeID)
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
        }
    }
}