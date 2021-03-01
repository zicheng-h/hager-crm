using hager_crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Data
{
    public static class HagerSeedData
    {
        public static void Seed(HagerContext context, ApplicationDbContext idcontext)
        {
            #region LookUpValues
            if (!context.CustomerTypes.Any())
            {
                context.CustomerTypes.AddRange(
                    new CustomerType
                    {
                        Type = "Poultry"
                    },
                    new CustomerType
                    {
                        Type = "Beef"
                    },
                    new CustomerType
                    {
                        Type = "Pork"
                    },
                    new CustomerType
                    {
                        Type = "Bakery"
                    },
                    new CustomerType
                    {
                        Type = "Vegetarian"
                    },
                    new CustomerType
                    {
                        Type = "Vegetables & Produce"
                    },
                    new CustomerType
                    {
                        Type = "Other Food"
                    },
                    new CustomerType
                    {
                        Type = "Compressed Gas"
                    },
                    new CustomerType
                    {
                        Type = "Cryogenic Pipe"
                    },
                    new CustomerType
                    {
                        Type = "Custom Fabrication"
                    },
                    new CustomerType
                    {
                        Type = "IQF Exhaust"
                    },
                    new CustomerType
                    {
                        Type = "NFPA Exhaust"
                    },
                    new CustomerType
                    {
                        Type = "Construction"
                    },
                    new CustomerType
                    {
                        Type = "Conveyors"
                    },
                    new CustomerType
                    {
                        Type = "Manifolds"
                    },
                    new CustomerType
                    {
                        Type = "Plumbing"
                    },
                    new CustomerType
                    {
                        Type = "Beverage"
                    },
                    new CustomerType
                    {
                        Type = "HPP"
                    }
                );
                context.SaveChanges();
            }

            if (!context.VendorTypes.Any())
            {
                context.VendorTypes.AddRange(
                    new VendorType
                    {
                        Type = "Conveyor & Fabrication"
                    },
                    new VendorType
                    {
                        Type = "Professional Service"
                    },
                    new VendorType
                    {
                        Type = "Office Supplies"
                    },
                    new VendorType
                    {
                        Type = "Shop Supplies"
                    },
                    new VendorType
                    {
                        Type = "Cryogenic"
                    },
                    new VendorType
                    {
                        Type = "Plumbing / Piping"
                    },
                    new VendorType
                    {
                        Type = "Transportation"
                    },
                    new VendorType
                    {
                        Type = "HVAC & Exhaust Systems"
                    },
                    new VendorType
                    {
                        Type = "Outsourced Fabrication & Services"
                    },
                    new VendorType
                    {
                        Type = "Electrical Components"
                    }
                );
                context.SaveChanges();
            }

            if (!context.ContractorTypes.Any())
            {
                context.ContractorTypes.AddRange(
                    new ContractorType
                    {
                        Type = "Welding"
                    },
                    new ContractorType
                    {
                        Type = "Plumbing"
                    },
                    new ContractorType
                    {
                        Type = "Electrical"
                    },
                    new ContractorType
                    {
                        Type = "Engineering"
                    },
                    new ContractorType
                    {
                        Type = "Fabrication"
                    },
                    new ContractorType
                    {
                        Type = "General Contractor"
                    },
                    new ContractorType
                    {
                        Type = "Metal Forming"
                    },
                    new ContractorType
                    {
                        Type = "Metal Cutting"
                    }
                );
                context.SaveChanges();
            }

            if (!context.BillingTerms.Any())
            {
                context.BillingTerms.AddRange(
                    new BillingTerm
                    {
                        Terms = "40% down, balance net 15"
                    },
                    new BillingTerm
                    {
                        Terms = "40% down, balance net 30"
                    },
                    new BillingTerm
                    {
                        Terms = "40% down, balance net 45"
                    },
                    new BillingTerm
                    {
                        Terms = "40% down, balance net 90"
                    },
                    new BillingTerm
                    {
                        Terms = "Due on receipt"
                    },
                    new BillingTerm
                    {
                        Terms = "Net 15"
                    },
                    new BillingTerm
                    {
                        Terms = "Net 30"
                    },
                    new BillingTerm
                    {
                        Terms = "Net 45"
                    },
                    new BillingTerm
                    {
                        Terms = "Net 90"
                    }
                );
                context.SaveChanges();
            }

            if (!context.JobPositions.Any())
            {
                context.JobPositions.AddRange(
                    new JobPosition
                    {
                        Position = "Jr. Fabricator"
                    },
                    new JobPosition
                    {
                        Position = "Fabricator"
                    },
                    new JobPosition
                    {
                        Position = "Sr. Fabricator"
                    },
                    new JobPosition
                    {
                        Position = "Foreman"
                    },
                    new JobPosition
                    {
                        Position = "Apprentice Plumber"
                    },
                    new JobPosition
                    {
                        Position = "Plumber"
                    },
                    new JobPosition
                    {
                        Position = "Field Supervisor"
                    },
                    new JobPosition
                    {
                        Position = "General Labourer"
                    },
                    new JobPosition
                    {
                        Position = "Shipping Receiving"
                    },
                    new JobPosition
                    {
                        Position = "Controller"
                    },
                    new JobPosition
                    {
                        Position = "President"
                    },
                    new JobPosition
                    {
                        Position = "Vice President"
                    },
                    new JobPosition
                    {
                        Position = "Jr. Draftsperson"
                    },
                    new JobPosition
                    {
                        Position = "Mechanical Designer"
                    },
                    new JobPosition
                    {
                        Position = "Professional Engineer"
                    },
                    new JobPosition
                    {
                        Position = "Engineering Manager"
                    },
                    new JobPosition
                    {
                        Position = "Mechanical Estimator/Purchaser"
                    },
                    new JobPosition
                    {
                        Position = "Estimator"
                    },
                    new JobPosition
                    {
                        Position = "Sales Manager"
                    }
                );
                context.SaveChanges();
            }

            if (!context.EmploymentTypes.Any())
            {
                context.EmploymentTypes.AddRange(
                    new EmploymentType
                    {
                        Type = "Full-Time"
                    },
                    new EmploymentType
                    {
                        Type = "Part-Time"
                    },
                    new EmploymentType
                    {
                        Type = "Contract"
                    },
                    new EmploymentType
                    {
                        Type = "Seasonal"
                    },
                    new EmploymentType
                    {
                        Type = "Co-op Student"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Categories
                    {
                        Category = "Christmas Card"
                    },
                    new Categories
                    {
                        Category = "Marketing Material"
                    },
                    new Categories
                    {
                        Category = "Newsletter"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Countries.Any())
            {
                context.Countries.AddRange(
                    new Country
                    {
                        CountryName = "Canada"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Provinces.Any())
            {
                context.Provinces.AddRange(
                    new Province
                    {
                        ProvinceName = "Ontario",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Quebec",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Nova Scotia",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "New Brunswick",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Manitoba",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "British Columbia",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Prince Edward Island",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Saskatchewan",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Alberta",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Newfoundland and Labrador",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Northwest Territories",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Yukon",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Nunavut",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    }
                );
                context.SaveChanges();
            }

            if(!context.Currencies.Any())
            {
                context.Currencies.AddRange(
                    new Currency
                    {
                        CurrencyName = "CAD"
                    },
                    new Currency
                    {
                        CurrencyName = "USD"
                    }
                    );
                context.SaveChanges();
            }

            
            #endregion

            #region Employees
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee
                    {
                        FirstName = "President",
                        LastName = "Surname",
                        JobPositionID = context.JobPositions.FirstOrDefault(p => p.Position == "President").JobPositionID,
                        EmploymentTypeID = context.EmploymentTypes.FirstOrDefault(t => t.Type == "Full-Time").EmploymentTypeID,
                        EmployeeAddress1 = "100 This St",
                        EmployeeProvinceID = context.Provinces.FirstOrDefault(p => p.ProvinceName == "Ontario").ProvinceID,
                        EmployeePostalCode = "A1B2C3",
                        EmployeeCountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID,
                        WorkPhone = 1234567890,
                        Email = "admin1@outlook.com",
                        DOB = DateTime.Parse("1980-04-04"),
                        Wage = 200000m,
                        Expense = 0m,
                        DateJoined = DateTime.Now,
                        KeyFob = 1234567,
                        EmergencyContactName = "Emergency",
                        EmergencyContactPhone = 1234567899,
                        Active = true
                    },
                    new Employee
                    {
                        FirstName = "Vice",
                        LastName = "President",
                        JobPositionID = context.JobPositions.FirstOrDefault(p => p.Position == "Vice President").JobPositionID,
                        EmploymentTypeID = context.EmploymentTypes.FirstOrDefault(t => t.Type == "Full-Time").EmploymentTypeID,
                        EmployeeAddress1 = "100 This St",
                        EmployeeProvinceID = context.Provinces.FirstOrDefault(p => p.ProvinceName == "Ontario").ProvinceID,
                        EmployeePostalCode = "A1B2C3",
                        EmployeeCountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID,
                        WorkPhone = 1234567890,
                        Email = "super1@outlook.com",
                        DOB = DateTime.Parse("1980-04-04"),
                        Wage = 200000m,
                        Expense = 0m,
                        DateJoined = DateTime.Now,
                        KeyFob = 1234567,
                        EmergencyContactName = "Emergency",
                        EmergencyContactPhone = 1234567899,
                        Active = true
                    },
                    new Employee
                    {
                        FirstName = "Sales",
                        LastName = "Manager",
                        JobPositionID = context.JobPositions.FirstOrDefault(p => p.Position == "Sales Manager").JobPositionID,
                        EmploymentTypeID = context.EmploymentTypes.FirstOrDefault(t => t.Type == "Full-Time").EmploymentTypeID,
                        EmployeeAddress1 = "100 This St",
                        EmployeeProvinceID = context.Provinces.FirstOrDefault(p => p.ProvinceName == "Ontario").ProvinceID,
                        EmployeePostalCode = "A1B2C3",
                        EmployeeCountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID,
                        WorkPhone = 1234567890,
                        Email = "employee1@outlook.com",
                        DOB = DateTime.Parse("1980-04-04"),
                        Wage = 200000m,
                        Expense = 0m,
                        DateJoined = DateTime.Now,
                        KeyFob = 1234567,
                        EmergencyContactName = "Emergency",
                        EmergencyContactPhone = 1234567899,
                        Active = true
                    }
                );
                
                context.SaveChanges();
                
                foreach (var employee in context.Employees)
                {
                    var identity = idcontext.Users.FirstOrDefault(u => u.Email == employee.Email);
                    if (identity != null)
                        employee.UserId = identity.Id;
                }
                
                context.SaveChanges();
            }
            #endregion

            //#region Company

            //#endregion

            //#region Contact
            //if (!context.Contacts.Any())
            //{
            //    context.Contacts.AddRange(
            //        new Contact // Customer Contact
            //        {
                        
                        


            //        },

            //        new Contact // Vendor Contact
            //        {
                        



            //        },

            //        new Contact // Contractor Contact
            //        {
                        



            //        }

            //    );
            //    context.SaveChanges();
            //}
            //#endregion
            #region Companies
            if (!context.Companies.Any())
            {
                context.Companies.AddRange(
                    new Company
                    {
                        Name = "Customer1",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Ontario").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Ontario").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = true,
                        //CustomerTypeID = 1,
                        CustomerTypeID = context.CustomerTypes.FirstOrDefault(b => b.Type == "Poultry").CustomerTypeID,
                        //CustomerType = context.CustomerTypes.FirstOrDefault(b => b.Type == "Poultry"),
                        Vendor = false,
                        VendorTypeID = null,
                        Contractor = false,
                        ContractorTypeID = null
                    }
                    ,
                    new Company
                    {
                        Name = "Customer2",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Quebec").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Quebec").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = true,
                        CustomerTypeID = context.CustomerTypes.FirstOrDefault(b => b.Type == "Beef").CustomerTypeID,
                        Vendor = false,
                        Contractor = false
                    },
                    new Company
                    {
                        Name = "Customer2",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Nova Scotia").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Nova Scotia").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = true,
                        CustomerTypeID = context.CustomerTypes.FirstOrDefault(b => b.Type == "Pork").CustomerTypeID,
                        Vendor = false,
                        Contractor = false
                    },
                    new Company
                    {
                        Name = "Vendor1",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Ontario").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Ontario").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = false,
                        Vendor = true,
                        VendorTypeID = context.VendorTypes.FirstOrDefault(b => b.Type == "Conveyor & Fabrication").VendorTypeID,
                        Contractor = false
                    },
                    new Company
                    {
                        Name = "Vendor2",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Quebec").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Quebec").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = false,
                        Vendor = true,
                        VendorTypeID = context.VendorTypes.FirstOrDefault(b => b.Type == "Professional Service").VendorTypeID,
                        Contractor = false
                    },
                    new Company
                    {
                        Name = "Vendor3",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Nova Scotia").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Nova Scotia").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = false,
                        Vendor = true,
                        VendorTypeID = context.VendorTypes.FirstOrDefault(b => b.Type == "Office Supplies").VendorTypeID,
                        Contractor = false
                    },
                    new Company
                    {
                        Name = "Contractor1",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Ontario").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Ontario").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = false,
                        Vendor = false,
                        Contractor = true,
                        ContractorTypeID = context.ContractorTypes.FirstOrDefault(b => b.Type == "Welding").ContractorTypeID
                    },
                    new Company
                    {
                        Name = "Contractor2",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Quebec").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Quebec").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = false,
                        Vendor = false,
                        Contractor = true,
                        ContractorTypeID = context.ContractorTypes.FirstOrDefault(b => b.Type == "Plumbing").ContractorTypeID
                    },
                    new Company
                    {
                        Name = "Contractor3",
                        Location = "Niagara Fall",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Nova Scotia").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Nova Scotia").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Customer = false,
                        Vendor = false,
                        Contractor = true,
                        ContractorTypeID = context.ContractorTypes.FirstOrDefault(b => b.Type == "Electrical").ContractorTypeID
                    }
                    ) ;
                context.SaveChanges();
            }
            #endregion
        }
    }
}
