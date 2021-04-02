using hager_crm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace hager_crm.Data
{
    public static class HagerSeedData
    {
        public static void Seed(HagerContext context, ApplicationDbContext idcontext)
        {
            //Prepare Random
            Random random = new Random();

            #region LookUpValues
            if (!context.CustomerTypes.Any())
            {
                context.CustomerTypes.AddRange(
                    new CustomerType
                    {
                        Type = "Poultry",
                        Order = 1
                    },
                    new CustomerType
                    {
                        Type = "Beef",
                        Order = 2
                    },
                    new CustomerType
                    {
                        Type = "Pork",
                        Order = 3
                    },
                    new CustomerType
                    {
                        Type = "Bakery",
                        Order = 4
                    },
                    new CustomerType
                    {
                        Type = "Vegetarian",
                        Order = 5
                    },
                    new CustomerType
                    {
                        Type = "Vegetables & Produce",
                        Order = 6
                    },
                    new CustomerType
                    {
                        Type = "Other Food",
                        Order = 7
                    },
                    new CustomerType
                    {
                        Type = "Compressed Gas",
                        Order = 8
                    },
                    new CustomerType
                    {
                        Type = "Cryogenic Pipe",
                        Order = 9
                    },
                    new CustomerType
                    {
                        Type = "Custom Fabrication",
                        Order = 10
                    },
                    new CustomerType
                    {
                        Type = "IQF Exhaust",
                        Order = 11
                    },
                    new CustomerType
                    {
                        Type = "NFPA Exhaust",
                        Order = 12
                    },
                    new CustomerType
                    {
                        Type = "Construction",
                        Order = 13
                    },
                    new CustomerType
                    {
                        Type = "Conveyors",
                        Order = 14
                    },
                    new CustomerType
                    {
                        Type = "Manifolds",
                        Order = 15
                    },
                    new CustomerType
                    {
                        Type = "Plumbing",
                        Order = 16
                    },
                    new CustomerType
                    {
                        Type = "Beverage",
                        Order = 17
                    },
                    new CustomerType
                    {
                        Type = "HPP",
                        Order = 18
                    }
                );
                context.SaveChanges();
            }

            if (!context.VendorTypes.Any())
            {
                context.VendorTypes.AddRange(
                    new VendorType
                    {
                        Type = "Conveyor & Fabrication",
                        Order = 1
                    },
                    new VendorType
                    {
                        Type = "Professional Service",
                        Order = 2
                    },
                    new VendorType
                    {
                        Type = "Office Supplies",
                        Order = 3
                    },
                    new VendorType
                    {
                        Type = "Shop Supplies",
                        Order = 4
                    },
                    new VendorType
                    {
                        Type = "Cryogenic",
                        Order = 5
                    },
                    new VendorType
                    {
                        Type = "Plumbing / Piping",
                        Order = 6
                    },
                    new VendorType
                    {
                        Type = "Transportation",
                        Order = 7
                    },
                    new VendorType
                    {
                        Type = "HVAC & Exhaust Systems",
                        Order = 8
                    },
                    new VendorType
                    {
                        Type = "Outsourced Fabrication & Services",
                        Order = 9
                    },
                    new VendorType
                    {
                        Type = "Electrical Components",
                        Order = 10
                    }
                );
                context.SaveChanges();
            }

            if (!context.ContractorTypes.Any())
            {
                context.ContractorTypes.AddRange(
                    new ContractorType
                    {
                        Type = "Welding",
                        Order = 1
                    },
                    new ContractorType
                    {
                        Type = "Plumbing",
                        Order = 2
                    },
                    new ContractorType
                    {
                        Type = "Electrical",
                        Order = 3
                    },
                    new ContractorType
                    {
                        Type = "Engineering",
                        Order = 4
                    },
                    new ContractorType
                    {
                        Type = "Fabrication",
                        Order = 5
                    },
                    new ContractorType
                    {
                        Type = "General Contractor",
                        Order = 7
                    },
                    new ContractorType
                    {
                        Type = "Metal Forming",
                        Order = 8
                    },
                    new ContractorType
                    {
                        Type = "Metal Cutting",
                        Order = 9
                    }
                );
                context.SaveChanges();
            }

            if (!context.BillingTerms.Any())
            {
                context.BillingTerms.AddRange(
                    new BillingTerm
                    {
                        Terms = "40% down, balance net 15",
                        Order = 1
                    },
                    new BillingTerm
                    {
                        Terms = "40% down, balance net 30",
                        Order = 2
                    },
                    new BillingTerm
                    {
                        Terms = "40% down, balance net 45",
                        Order = 3
                    },
                    new BillingTerm
                    {
                        Terms = "40% down, balance net 90",
                        Order = 4
                    },
                    new BillingTerm
                    {
                        Terms = "Due on receipt",
                        Order = 5
                    },
                    new BillingTerm
                    {
                        Terms = "Net 15",
                        Order = 6
                    },
                    new BillingTerm
                    {
                        Terms = "Net 30",
                        Order = 7
                    },
                    new BillingTerm
                    {
                        Terms = "Net 45",
                        Order = 8
                    },
                    new BillingTerm
                    {
                        Terms = "Net 90",
                        Order = 9
                    }
                );
                context.SaveChanges();
            }

            if (!context.JobPositions.Any())
            {
                context.JobPositions.AddRange(
                    new JobPosition
                    {
                        Position = "Jr. Fabricator",
                        Order = 1
                    },
                    new JobPosition
                    {
                        Position = "Fabricator",
                        Order = 2
                    },
                    new JobPosition
                    {
                        Position = "Sr. Fabricator",
                        Order = 3
                    },
                    new JobPosition
                    {
                        Position = "Foreman",
                        Order = 4
                    },
                    new JobPosition
                    {
                        Position = "Apprentice Plumber",
                        Order = 5
                    },
                    new JobPosition
                    {
                        Position = "Plumber",
                        Order = 6
                    },
                    new JobPosition
                    {
                        Position = "Field Supervisor",
                        Order = 7
                    },
                    new JobPosition
                    {
                        Position = "General Labourer",
                        Order = 8
                    },
                    new JobPosition
                    {
                        Position = "Shipping Receiving",
                        Order = 9
                    },
                    new JobPosition
                    {
                        Position = "Controller",
                        Order = 10
                    },
                    new JobPosition
                    {
                        Position = "President",
                        Order = 11
                    },
                    new JobPosition
                    {
                        Position = "Vice President",
                        Order = 12
                    },
                    new JobPosition
                    {
                        Position = "Jr. Draftsperson",
                        Order = 13
                    },
                    new JobPosition
                    {
                        Position = "Mechanical Designer",
                        Order = 14
                    },
                    new JobPosition
                    {
                        Position = "Professional Engineer",
                        Order = 15
                    },
                    new JobPosition
                    {
                        Position = "Engineering Manager",
                        Order = 16
                    },
                    new JobPosition
                    {
                        Position = "Mechanical Estimator/Purchaser",
                        Order = 17
                    },
                    new JobPosition
                    {
                        Position = "Estimator",
                        Order = 18
                    },
                    new JobPosition
                    {
                        Position = "Sales Manager",
                        Order = 19
                    }
                );
                context.SaveChanges();
            }

            if (!context.EmploymentTypes.Any())
            {
                context.EmploymentTypes.AddRange(
                    new EmploymentType
                    {
                        Type = "Full-Time",
                        Order = 1
                    },
                    new EmploymentType
                    {
                        Type = "Part-Time",
                        Order = 2
                    },
                    new EmploymentType
                    {
                        Type = "Contract",
                        Order = 3
                    },
                    new EmploymentType
                    {
                        Type = "Seasonal",
                        Order = 4
                    },
                    new EmploymentType
                    {
                        Type = "Co-op Student",
                        Order = 5
                    }
                );
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Categories
                    {
                        Category = "Christmas Card",
                        Order = 1
                    },
                    new Categories
                    {
                        Category = "Marketing Material",
                        Order = 2
                    },
                    new Categories
                    {
                        Category = "Newsletter",
                        Order = 3
                    }
                );
                context.SaveChanges();
            }

            if (!context.Countries.Any())
            {
                context.Countries.AddRange(
                    new Country
                    {
                        CountryName = "Canada",
                        CountryAbbr = "CA"
                    },
                    new Country
                    {
                        CountryName = "United States",
                        CountryAbbr = "US"
                    },
                    new Country
                    {
                        CountryName = "Mexico",
                        CountryAbbr = "MX"
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
                        ProvinceAbbr = "ON",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Quebec",
                        ProvinceAbbr = "QC",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Nova Scotia",
                        ProvinceAbbr = "NS",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "New Brunswick",
                        ProvinceAbbr = "NB",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Manitoba",
                        ProvinceAbbr = "MB",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "British Columbia",
                        ProvinceAbbr = "BC",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Prince Edward Island",
                        ProvinceAbbr = "PE",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Saskatchewan",
                        ProvinceAbbr = "SK",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Alberta",
                        ProvinceAbbr = "AB",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Newfoundland and Labrador",
                        ProvinceAbbr = "NL",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Northwest Territories",
                        ProvinceAbbr = "NT",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Yukon",
                        ProvinceAbbr = "YT",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Nunavut",
                        ProvinceAbbr = "NU",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Alabama",
                        ProvinceAbbr = "AL",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Alaska",
                        ProvinceAbbr = "AK",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Arizona",
                        ProvinceAbbr = "AZ",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Arkansas",
                        ProvinceAbbr = "AR",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "California",
                        ProvinceAbbr = "CA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Colorado",
                        ProvinceAbbr = "CO",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Connecticut",
                        ProvinceAbbr = "CT",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Delaware",
                        ProvinceAbbr = "DE",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Florida",
                        ProvinceAbbr = "FL",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Georgia",
                        ProvinceAbbr = "GA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Hawaii",
                        ProvinceAbbr = "HI",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Idaho",
                        ProvinceAbbr = "ID",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Illinois",
                        ProvinceAbbr = "IL",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Indiana",
                        ProvinceAbbr = "IN",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Iowa",
                        ProvinceAbbr = "IA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Kansas",
                        ProvinceAbbr = "KS",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Kentucky",
                        ProvinceAbbr = "KY",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Louisiana",
                        ProvinceAbbr = "LA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Maine",
                        ProvinceAbbr = "ME",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Maryland",
                        ProvinceAbbr = "MD",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Massachusetts",
                        ProvinceAbbr = "MA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Michigan",
                        ProvinceAbbr = "MI",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Minnesota",
                        ProvinceAbbr = "MN",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Mississippi",
                        ProvinceAbbr = "MS",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Missouri",
                        ProvinceAbbr = "MO",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Montana",
                        ProvinceAbbr = "MT",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Nebraska",
                        ProvinceAbbr = "NE",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Nevada",
                        ProvinceAbbr = "NV",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "New Hampshire",
                        ProvinceAbbr = "NH",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "New Jersey",
                        ProvinceAbbr = "NJ",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "New Mexico",
                        ProvinceAbbr = "NM",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "New York",
                        ProvinceAbbr = "NY",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "North Carolina",
                        ProvinceAbbr = "NC",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "North Dakota",
                        ProvinceAbbr = "ND",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Ohio",
                        ProvinceAbbr = "OH",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Oklahoma",
                        ProvinceAbbr = "OK",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Oregon",
                        ProvinceAbbr = "OR",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Pennsylvania",
                        ProvinceAbbr = "PA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Rhode Island",
                        ProvinceAbbr = "RI",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "South Carolina",
                        ProvinceAbbr = "SC",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "South Dakota",
                        ProvinceAbbr = "SD",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Tennessee",
                        ProvinceAbbr = "TN",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Texas",
                        ProvinceAbbr = "TX",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Utah",
                        ProvinceAbbr = "UT",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Vermont",
                        ProvinceAbbr = "VT",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Virginia",
                        ProvinceAbbr = "VA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Washington",
                        ProvinceAbbr = "WA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "West Virginia",
                        ProvinceAbbr = "WV",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Wisconsin",
                        ProvinceAbbr = "WI",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Wyoming",
                        ProvinceAbbr = "WY",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "United States").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Aguascalientes",
                        ProvinceAbbr = "AG",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Baja California",
                        ProvinceAbbr = "BC",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Baja California Sur",
                        ProvinceAbbr = "BS",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Campeche",
                        ProvinceAbbr = "CM",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Chiapas",
                        ProvinceAbbr = "CS",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Chihuahua",
                        ProvinceAbbr = "CH",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Coahuila",
                        ProvinceAbbr = "CO",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Colima",
                        ProvinceAbbr = "CL",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Mexico City",
                        ProvinceAbbr = "DF",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Durango",
                        ProvinceAbbr = "DG",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Guanajuato",
                        ProvinceAbbr = "GT",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Guerrero",
                        ProvinceAbbr = "GR",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Hidalgo",
                        ProvinceAbbr = "HG",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Jalisco",
                        ProvinceAbbr = "JA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "México",
                        ProvinceAbbr = "EM",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Michoacán",
                        ProvinceAbbr = "MI",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Morelos",
                        ProvinceAbbr = "MO",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Nayarit",
                        ProvinceAbbr = "NA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Nuevo León",
                        ProvinceAbbr = "NL",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Oaxaca",
                        ProvinceAbbr = "OA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Puebla",
                        ProvinceAbbr = "PU",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Querétaro",
                        ProvinceAbbr = "QT",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Quintana Roo",
                        ProvinceAbbr = "QR",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "San Luis Potosí",
                        ProvinceAbbr = "SL",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Sinaloa",
                        ProvinceAbbr = "SI",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Sonora",
                        ProvinceAbbr = "SO",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Tabasco",
                        ProvinceAbbr = "TB",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Tamaulipas",
                        ProvinceAbbr = "TM",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Tlaxcala",
                        ProvinceAbbr = "TL",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Veracruz",
                        ProvinceAbbr = "VE",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Yucatán",
                        ProvinceAbbr = "YU",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    },
                    new Province
                    {
                        ProvinceName = "Zacatecas",
                        ProvinceAbbr = "ZA",
                        CountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Mexico").CountryID
                    }
                );
                context.SaveChanges();
            }

            if (!context.Currencies.Any())
            {
                context.Currencies.AddRange(
                    new Currency
                    {
                        CurrencyName = "CAD",
                        Order = 1
                    },
                    new Currency
                    {
                        CurrencyName = "USD",
                        Order = 2
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
                        FirstName = "Jeremy",
                        LastName = "Hager",
                        JobPositionID = context.JobPositions.FirstOrDefault(p => p.Position == "Vice President").JobPositionID,
                        EmploymentTypeID = context.EmploymentTypes.FirstOrDefault(t => t.Type == "Full-Time").EmploymentTypeID,
                        EmployeeAddress1 = "895 Maple Ave",
                        EmployeeCity = "Burlington",
                        EmployeeProvinceID = context.Provinces.FirstOrDefault(p => p.ProvinceName == "Ontario").ProvinceID,
                        EmployeePostalCode = "L7S2H7",
                        EmployeeCountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID,
                        WorkPhone = 2892142825,
                        Email = "admin1@outlook.com",
                        DOB = DateTime.Parse("1980-03-30"),
                        Wage = 200000m,
                        Expense = 0m,
                        DateJoined = DateTime.Parse("2019-01-02"),
                        KeyFob = 1234567,
                        EmergencyContactName = "Emergency",
                        EmergencyContactPhone = 1234567899,
                        Active = true
                    },
                    new Employee
                    {
                        FirstName = "Darryl",
                        LastName = "Zych",
                        JobPositionID = context.JobPositions.FirstOrDefault(p => p.Position == "Engineering Manager").JobPositionID,
                        EmploymentTypeID = context.EmploymentTypes.FirstOrDefault(t => t.Type == "Full-Time").EmploymentTypeID,
                        EmployeeAddress1 = "91 2nd St.",
                        EmployeeCity = "Stoney Creek",
                        EmployeeProvinceID = context.Provinces.FirstOrDefault(p => p.ProvinceName == "Ontario").ProvinceID,
                        EmployeePostalCode = "L8G1Z2",
                        EmployeeCountryID = context.Countries.FirstOrDefault(c => c.CountryName == "Canada").CountryID,
                        WorkPhone = 1234567890,
                        Email = "super1@outlook.com",
                        DOB = DateTime.Parse("1974-04-04"),
                        Wage = 200000m,
                        Expense = 0m,
                        DateJoined = DateTime.Parse("2004-03-20"),
                        KeyFob = 1234567,
                        EmergencyContactName = "Emergency",
                        EmergencyContactPhone = 1234567899,
                        Active = true
                    },
                    new Employee
                    {
                        FirstName = "Padraic",
                        LastName = "Cronnelly",
                        JobPositionID = context.JobPositions.FirstOrDefault(p => p.Position == "Shipping Receiving").JobPositionID,
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

            #region Companies
            if (!context.Companies.Any())
            {
                context.Companies.AddRange(
                    new Company
                    {
                        Name = "Eco Focus",
                        Location = "Niagara Falls",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
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
                        Active = true
                    },
                    new Company
                    {
                        Name = "Meat lovers",
                        Location = "Montreal",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
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
                        Active = true
                    },
                    new Company
                    {
                        Name = "Bacon lovers",
                        Location = "Halifax",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
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
                        Active = true
                    },
                    new Company
                    {
                        Name = "Tele-Transporters",
                        Location = "Toronto",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
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
                        Active = true
                    },
                    new Company
                    {
                        Name = "Mr. All Services",
                        Location = "Quebec City",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
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
                        Active = true
                    },
                    new Company
                    {
                        Name = "Dunder Mifflin",
                        Location = "Victoria",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "British Columbia").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "British Columbia").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Active = true
                    },
                    new Company
                    {
                        Name = "IronMen",
                        Location = "Niagara Falls",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
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
                        Active = true
                    },
                    new Company
                    {
                        Name = "Mario Brothers",
                        Location = "Vancouver",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "British Columbia").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "British Columbia").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Active = true
                    },
                    new Company
                    {
                        Name = "Static Shock",
                        Location = "Winnipeg",
                        DateChecked = DateTime.Now,
                        BillingTermID = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15").BillingTermID,
                        //BillingTerm = context.BillingTerms.FirstOrDefault(b => b.Terms == "40% down, balance net 15"),
                        CurrencyID = context.Currencies.FirstOrDefault(b => b.CurrencyName == "CAD").CurrencyID,
                        BillingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Manitoba").ProvinceID,
                        Phone = 1234567890,
                        BillingAddress1 = "First Avenue",
                        BillingPostalCode = "L3C1A1",
                        BillingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        ShippingAddress1 = "First Avenue",
                        ShippingProvinceID = context.Provinces.FirstOrDefault(b => b.ProvinceName == "Manitoba").ProvinceID,
                        ShippingPostalCode = "L2C1A1",
                        ShippingCountryID = context.Countries.FirstOrDefault(b => b.CountryName == "Canada").CountryID,
                        Active = true
                    }
                    );
                context.SaveChanges();
            }
            #endregion

            #region CompaniesTypes
            int n = context.Companies.Count();

            if (!context.CompanyContractors.Any())
            {
                var cntTypes = context.ContractorTypes.ToList();
                foreach (var company in context.Companies.Take(n / 3))
                {
                    context.CompanyContractors.Add(new CompanyContractor
                    {
                        CompanyID = company.CompanyID,
                        ContractorTypeID = cntTypes[random.Next(cntTypes.Count)].ContractorTypeID
                    });
                }
                context.SaveChanges();
            }

            if (!context.CompanyVendors.Any())
            {
                var vndTypes = context.VendorTypes.ToList();
                foreach (var company in context.Companies.Skip(n / 3).Take(n / 3))
                {
                    context.CompanyVendors.Add(new CompanyVendor()
                    {
                        CompanyID = company.CompanyID, 
                        VendorTypeID = vndTypes[random.Next(vndTypes.Count)].VendorTypeID
                    });
                }
                context.SaveChanges();
            }

            if (!context.CompanyCustomers.Any())
            {
                var cstTypes = context.CustomerTypes.ToList();
                foreach (var company in context.Companies.Skip(n / 3 * 2).Take(n - n / 3 * 2))
                {
                    context.CompanyCustomers.Add(new CompanyCustomer
                    {
                        CompanyID = company.CompanyID,
                        CustomerTypeID = cstTypes[random.Next(cstTypes.Count)].CustomerTypeID
                    });
                }
                context.SaveChanges();
            }
            
            #endregion

            #region Contact
            if (!context.Contacts.Any())
            {
                context.Contacts.AddRange(
                    new Contact // Customer Contact 1
                    {
                        FirstName = "Harley",
                        LastName = "Alford",
                        JobTitle = "Employee",
                        CellPhone = 6920192523,
                        WorkPhone = 5489454130,
                        Email = "harleyalford@example.com",
                        Active = true,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Eco Focus").CompanyID
                    },

                    new Contact // Customer Contact 2
                    {
                        FirstName = "Elicia",
                        LastName = "Storey",
                        JobTitle = "Employee",
                        CellPhone = 4880498113,
                        WorkPhone = 3194071784,
                        Email = "elicia@example.com",
                        Active = true,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Meat lovers").CompanyID
                    },

                    new Contact // Customer Contact 3
                    {
                        FirstName = "Blanka",
                        LastName = "Ramsay",
                        JobTitle = "Employee",
                        CellPhone = 1236548520,
                        WorkPhone = 2583691478,
                        Email = "ramsay@example.com",
                        Active = true,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Bacon lovers").CompanyID
                    },

                    new Contact // Vendor Contact 1
                    {
                        FirstName = "Jean",
                        LastName = "Cano",
                        JobTitle = "Employee",
                        CellPhone = 1235210484,
                        WorkPhone = 2179801100,
                        Email = "jean@example.com",
                        Active = false,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Tele-Transporters").CompanyID
                    },

                    new Contact // Vendor Contact 2
                    {
                        FirstName = "Rahima",
                        LastName = "Molina",
                        JobTitle = "General Manager",
                        CellPhone = 3214659878,
                        WorkPhone = 2179802220,
                        Email = "molina@example.com",
                        Active = false,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Mr. All Services").CompanyID
                    },

                    new Contact // Vendor Contact 3
                    {
                        FirstName = "Orson",
                        LastName = "Mays",
                        JobTitle = "Employee",
                        CellPhone = 4568124645,
                        WorkPhone = 2179801111,
                        Email = "mays@example.com",
                        Active = false,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Dunder Mifflin").CompanyID
                    },

                    new Contact // Contractor Contact 1
                    {
                        FirstName = "Sameera",
                        LastName = "Avalos",
                        JobTitle = "Employee",
                        CellPhone = 5552725851,
                        WorkPhone = 1771362746,
                        Email = "sameera@example.com",
                        Active = true,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "IronMen").CompanyID
                    },

                    new Contact // Contractor Contact 2
                    {
                        FirstName = "Raja",
                        LastName = "Avalos",
                        JobTitle = "General Manager",
                        CellPhone = 5552725851,
                        WorkPhone = 2179801190,
                        Email = "avalos@example.com",
                        Active = true,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Mario Brothers").CompanyID
                    },

                    new Contact // Contractor Contact 3
                    {
                        FirstName = "Cerys",
                        LastName = "Rowland",
                        JobTitle = "Employee",
                        CellPhone = 9876543210,
                        WorkPhone = 3179801100,
                        Email = "rowland@example.com",
                        Active = true,
                        Notes = "Just an example of text. This text is just a random note for this example.",
                        CompanyID = context.Companies.FirstOrDefault(c => c.Name == "Static Shock").CompanyID
                    }

                );
                context.SaveChanges();
            }
            #endregion

            #region Contact Categories
            if (!context.ContactCategories.Any())
            {
                context.ContactCategories.AddRange(
                    // Customer Contact
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Christmas Card").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Alford" && c.FirstName == "Harley").ContactID
                    },
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Newsletter").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Storey" && c.FirstName == "Elicia").ContactID
                    },
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Marketing Material").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Ramsay" && c.FirstName == "Blanka").ContactID
                    },
                    // Vendor Contact.
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Marketing Material").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Cano" && c.FirstName == "Jean").ContactID
                    },
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Newsletter").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Molina" && c.FirstName == "Rahima").ContactID
                    },
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Newsletter").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Mays" && c.FirstName == "Orson").ContactID
                    },
                    // Contractor Contact
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Christmas Card").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Avalos" && c.FirstName == "Sameera").ContactID
                    },
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Newsletter").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Avalos" && c.FirstName == "Raja").ContactID
                    },
                    new ContactCategories
                    {
                        CategoriesID = context.Categories.FirstOrDefault(c => c.Category == "Marketing Material").ID,
                        ContactID = context.Contacts.FirstOrDefault(c => c.LastName == "Rowland" && c.FirstName == "Cerys").ContactID
                    });
                context.SaveChanges();
            }
            #endregion

            #region Announcements

            if (!context.Announcements.Any())
            {
                var announcement = new Announcement
                {
                    Title = "Test Message", 
                    Message = "Test Message. How are you?",
                    Severity = AnnouncementSeverity.Info,
                    PostedAt = DateTime.Now
                };
                context.Announcements.Add(announcement);
                context.SaveChanges();
                foreach (var employee in context.Employees)
                {
                    context.AnnouncementEmployees.Add(new AnnouncementEmployee
                    {
                        EmployeeID = employee.EmployeeID, 
                        AnnouncementID = announcement.AnnouncementID
                    });
                }
                context.SaveChanges();
            }

            #endregion
        }
    }
}