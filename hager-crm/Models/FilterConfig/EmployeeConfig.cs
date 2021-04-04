using System;
using System.Collections.Generic;
using System.Linq;

namespace hager_crm.Models.FilterConfig
{
    public class EmployeeConfig : IModelConfig<Employee>
    {
        public List<BaseFilterRule> GetFilteringRules()
        {
            return new List<BaseFilterRule>
            {
                new InputFilterRule
                {
                    DisplayName = "Full Name",
                    FieldName = "FullName",
                    FieldType = "text"
                },
                new InputFilterRule
                {
                    FieldName = "Email",
                    FieldType = "email"
                },
                // It is stupid to filter by integer imho
                // new InputFilterRule
                // {
                //     DisplayName = "Cell Phone",
                //     FieldName = "CellPhone",
                //     FieldType = "tel"
                // },
                new DropdownFilterRule
                {
                    DisplayName = "Employment Type",
                    FieldName = "EmploymentTypeID",
                },
                new DropdownFilterRule
                {
                    DisplayName = "Job Position",
                    FieldName = "JobPositionID",
                },
                new RadioboxFilterRule
                {
                    DisplayName = "Is User",
                    FieldName = "IsUser",
                },
                new RadioboxFilterRule
                {
                    DisplayName = "Is Active",
                    FieldName = "Active",
                }

            };
        }

        public Dictionary<string, ConfigAction<Employee>> GetActions()
        {
            return new Dictionary<string, ConfigAction<Employee>>
            {
                {
                    "ID", new ConfigAction<Employee>
                    {
                        OnSort = (p, q) =>
                            (string) p == "DESC"
                                ? q.OrderByDescending(i => i.EmployeeID)
                                : q.OrderBy(i => i.EmployeeID),
                        OnFilter = (p, q) => q.Where(i => i.EmployeeID == (int) p)
                    }
                },
                {
                    "FullName", new ConfigAction<Employee>
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
                    "Email", new ConfigAction<Employee>
                    {
                        OnSort = (p, q) =>
                            (string) p == "DESC" ? q.OrderByDescending(i => i.Email) : q.OrderBy(i => i.Email),
                        OnFilter = (p, q) => q.Where(i => i.Email.Contains((string) p))
                    }
                },
                {
                    "CellPhone", new ConfigAction<Employee>
                    {
                        OnSort = (p, q) =>
                            (string) p == "DESC" ? q.OrderByDescending(i => i.CellPhone) : q.OrderBy(i => i.CellPhone),
                        OnFilter = (p, q) => 
                            long.TryParse((string) p, out var res) ? q.Where(i => i.CellPhone == res) : q
                    }
                },
                {
                    "EmploymentTypeID", new ConfigAction<Employee>
                    {
                        OnSort = (p, q) => q,
                        OnFilter = (p, q) => 
                            int.TryParse((string) p, out var res) ? q.Where(i => i.EmploymentTypeID == res) : q
                    }
                },
                {
                    "JobPositionID", new ConfigAction<Employee>
                    {
                        OnSort = (p, q) => 
                            (string) p == "DESC" ? q.OrderByDescending(i => i.JobPosition.Position) : 
                                q.OrderBy(i => i.JobPosition.Position),
                        OnFilter = (p, q) => 
                            int.TryParse((string) p, out var res) ? q.Where(i => i.JobPositionID == res) : q
                    }
                },
                {
                    "IsUser", new ConfigAction<Employee>
                    {
                        OnSort = (p, q) => q,
                        OnFilter = (p, q) =>
                            q.Where(i => ((string) p == "off") ||
                                ((string) p == "false" ? i.UserId == null : i.UserId != null))
                    }
                },
                {
                    "Active", new ConfigAction<Employee>
                    {
                        OnSort = (p, q) => q,
                        OnFilter = (p, q) =>
                            q.Where(i => ((string) p == "off") ||
                                ((string) p == "false" ? !i.Active : i.Active))
                    }
                }
            };
        }
    }
}