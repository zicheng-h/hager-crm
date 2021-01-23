using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class EmploymentType
    {
        public EmploymentType()
        {
            Employees = new HashSet<Employee>();
        }

        public int EmploymentTypeID { get; set; }
        [Display(Name = "Employment Type")]
        [StringLength(40, ErrorMessage = "Please enter a Type with less than 40 characters")]
        public string Type { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
