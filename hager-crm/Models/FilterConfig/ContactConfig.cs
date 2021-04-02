using System;
using System.Collections.Generic;
using System.Linq;

namespace hager_crm.Models.FilterConfig
{
    public class ContactConfig : IModelConfig<Contact>
    {
        public List<BaseFilterRule> GetFilteringRules()
        {
            return new List<BaseFilterRule>
            {
                new InputFilterRule
                {
                    DisplayName = "Contact Name",
                    FieldName = "ContactName",
                    FieldType = "text"
                },
                new InputFilterRule
                {
                    DisplayName = "Company Name",
                    FieldName = "CompanyName",
                    FieldType = "text"
                },
                new InputFilterRule
                {
                    DisplayName = "Job Title",
                    FieldName = "JobTitle",
                    FieldType = "text"
                },
                new DropdownFilterRule
                {
                    DisplayName = "Category",
                    FieldName = "CategoriesID",
                },
                new RadioboxFilterRule
                {
                    DisplayName = "Is Active",
                    FieldName = "Active",
                }
            };
        }

        public Dictionary<string, ConfigAction<Contact>> GetActions()
        {
            return new Dictionary<string, ConfigAction<Contact>>
            {
                {
                    "ID", new ConfigAction<Contact>
                    {
                        OnSort = (p, q) =>
                            (string) p == "DESC"
                                ? q.OrderByDescending(i => i.ContactID)
                                : q.OrderBy(i => i.ContactID),
                        OnFilter = (p, q) => q.Where(i => i.ContactID == (int) p)
                    }
                },
                {
                    "ContactName", new ConfigAction<Contact>
                    {
                        OnSort = (p, q) => (string) p == "DESC"
                            ? q.OrderByDescending(i => i.LastName)
                                .ThenByDescending(i => i.FirstName)
                            : q.OrderBy(i => i.LastName)
                                .ThenBy(i => i.FirstName),
                        OnFilter = (p, q) => q.Where(i =>
                            i.LastName.Contains((string) p) ||
                            i.FirstName.Contains((string) p))
                    }
                },
                {
                    "CompanyName", new ConfigAction<Contact>
                    {
                        OnSort = (p, q) => (string) p == "DESC"
                            ? q.OrderByDescending(i => i.Company.Name)
                            : q.OrderBy(i => i.Company.Name),
                        OnFilter = (p, q) => q.Where(i =>
                            i.Company.Name.Contains((string) p))
                    }
                },
                {
                    "JobTitle", new ConfigAction<Contact>
                    {
                        OnFilter = (p, q) => q.Where(i =>
                            i.JobTitle.Contains((string) p))
                    }
                },
                {
                    "CategoriesID", new ConfigAction<Contact>
                    {
                        OnFilter = (p, q) =>
                            int.TryParse((string) p, out var res) ?
                                q.Where(i => i.ContactCategories
                                        .Select(c => c.CategoriesID)
                                        .Contains(res)
                                    ) : q
                    }
                },
                {
                    "Active", new ConfigAction<Contact>
                    {
                        OnSort = (p, q) => q,
                        OnFilter = (p, q) =>
                            q.Where(i => ((string) p == "off") ||
                                ((string) p == "false" ? !i.Active : i.Active))
                    }
                },
                {
                    "CompanyID", new ConfigAction<Contact>
                    {
                        OnFilter = (p, q) =>
                            int.TryParse((string) p, out var res) ? q.Where(i => i.CompanyID == res) : q
                    }
                },
                {
                    "CType", new ConfigAction<Contact>
                    {
                        OnFilter = (p, q) =>
                            q.Where(i => (((string) p == "Customer" && i.Company.CompanyCustomers.Count > 0) ||
                                ((string) p == "Vendor" && i.Company.CompanyVendors.Count > 0) ||
                                ((string) p == "Contractor" && i.Company.CompanyContractors.Count > 0) ||
                                ((string) p == "All")
                            ))
                    }
                }
            };
        }
    }
}
