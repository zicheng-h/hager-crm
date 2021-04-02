using System;
using System.Collections.Generic;
using System.Linq;

namespace hager_crm.Models.FilterConfig
{
    public class CompanyConfig : IModelConfig<Company>
    {
        public List<BaseFilterRule> GetFilteringRules()
        {
            return new List<BaseFilterRule>
            {
                new InputFilterRule
                {
                    DisplayName = "Company Name",
                    FieldName = "Name",
                    FieldType = "text"
                },
                new ProvinceDropdownFilterRule
                {
                    DisplayName = "Province",
                    FieldName = "ProvinceID",
                },
                new CountryDropdownFilterRule
                {
                    DisplayName = "Country",
                    FieldName = "CountryID",
                },
                new RadioboxFilterRule
                {
                    DisplayName = "Is Active",
                    FieldName = "Active",
                }
            };
        }

        public Dictionary<string, ConfigAction<Company>> GetActions()
        {
            return new Dictionary<string, ConfigAction<Company>>
            {
                {
                    "ID", new ConfigAction<Company>
                    {
                        OnSort = (p, q) =>
                            (string) p == "DESC"
                                ? q.OrderByDescending(i => i.CompanyID)
                                : q.OrderBy(i => i.CompanyID),
                        OnFilter = (p, q) => q.Where(i => i.CompanyID == (int) p)
                    }
                },
                {
                    "Name", new ConfigAction<Company>
                    {
                        OnSort = (p, q) => (string) p == "DESC"
                            ? q.OrderByDescending(i => i.Name)
                            : q.OrderBy(i => i.Name),
                        OnFilter = (p, q) => q.Where(i =>
                            i.Name.Contains((string) p))
                    }
                },
                {
                    "ProvinceID", new ConfigAction<Company>
                    {
                        OnSort = (p, q) =>  (string) p == "DESC"
                            ? q.OrderByDescending(i => i.BillingProvince.ProvinceName)
                            : q.OrderBy(i => i.BillingProvince.ProvinceName),
                        OnFilter = (p, q) =>
                            int.TryParse((string) p, out var res) ? q.Where(i => i.BillingProvinceID == res) : q
                    }
                },
                {
                    "CountryID", new ConfigAction<Company>
                    {
                        OnSort = (p, q) =>
                            (string) p == "DESC" ? q.OrderByDescending(i => i.BillingCountry.CountryName) :
                                q.OrderBy(i => i.BillingCountry.CountryName),
                        OnFilter = (p, q) =>
                            int.TryParse((string) p, out var res) ? q.Where(i => i.BillingCountryID == res) : q
                    }
                },
                {
                    "Active", new ConfigAction<Company>
                    {
                        OnSort = (p, q) => q,
                        OnFilter = (p, q) =>
                            q.Where(i => ((string) p == "off") ||
                                ((string) p == "false" ? !i.Active : i.Active))
                    }
                },
                {
                    "CType", new ConfigAction<Company>
                    {
                        OnFilter = (p, q) =>
                            q.Where(i => ((string) p == "Customer" && i.CompanyCustomers.Count > 0) ||
                                ((string) p == "Vendor" && i.CompanyVendors.Count > 0) ||
                                ((string) p == "Contractor" && i.CompanyContractors.Count > 0) ||
                                ((string) p == "All")
                            )
                    }
                }
            };
        }
    }
}