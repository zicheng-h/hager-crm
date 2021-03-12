using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class Country
    {
        public Country()
        {
            Companies = new HashSet<Company>();
            Employees = new HashSet<Employee>();
        }

        public int CountryID { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please enter the Country name")]
        [StringLength(40, ErrorMessage = "Please enter a Country with less than 40 characters")]
        public string CountryName { get; set; }

        [Display(Name = "Country Abbreviation")]
        [Required(ErrorMessage = "Please enter the Country abbreviation")]
        [StringLength(10, ErrorMessage = "Please enter an Abbreviation with less than 10 characters")]
        public string CountryAbbr { get; set; }

        public ICollection<Company> Companies { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
