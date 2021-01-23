using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class Province
    {
        public Province()
        {
            Companies = new HashSet<Company>();
            Employees = new HashSet<Employee>();
        }

        public int ProvinceID { get; set; }
        [Display(Name = "Province")]
        [StringLength(30, ErrorMessage = "Please enter a Province with less than 30 characters")]
        public string ProvinceName { get; set; }
        public ICollection<Company> Companies { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
