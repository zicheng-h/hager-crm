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
        [Required(ErrorMessage = "Please enter the Province name")]
        [StringLength(30, ErrorMessage = "Please enter a Province with less than 30 characters")]
        public string ProvinceName { get; set; }

        [Display(Name = "Province Abbreviation")]
        [Required(ErrorMessage = "Please enter the Province abbreviation")]
        [StringLength(10, ErrorMessage = "Please enter an Abbreviation with less than 10 characters")]
        public string ProvinceAbbr { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please select the Country")]
        public int CountryID { get; set; }

        public ICollection<Company> Companies { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
