using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.ViewModels
{
    public class RoleVM
    {
        public RoleVM()
        {
            Employees = new List<EmployeeVM>();
            EmployeesNotInRole = new List<EmployeeVM>();
        }

        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public List<EmployeeVM> Employees { get; set; }
        public List<EmployeeVM> EmployeesNotInRole { get; set; }
    }
}
