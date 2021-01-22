using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class JobPosition
    {
        public JobPosition()
        {
            Employees = new HashSet<Employee>();
        }

        public int JobPositionID { get; set; }
        [Display(Name = "Job Position")]
        [StringLength(40, ErrorMessage = "Please enter a Position with less than 40 characters")]
        public string Position { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
