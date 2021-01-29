using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.ViewModels
{
    public class EmployeeVM
    {
        public EmployeeVM()
        {
            Roles = new List<RoleVM>();
        }

        public int EmployeeID { get; set; }
        public string Fullname { get; set; }
        public string UserID { get; set; }

        public List<RoleVM> Roles { get; set; }
    }
}
