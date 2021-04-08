// Elzo Honorato Neto
// 02/15/2021
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class Contact
    {
        public Contact()
        {
            ContactCategories = new HashSet<ContactCategories>();
        }

        public int ContactID { get; set; }
        [Required(ErrorMessage = "Please enter a First Name.")]
        [StringLength(50, ErrorMessage = "Please enter a First Name with less than 50 characters.")]
        [Display(Name = "First Name")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a Last Name.")]
        [StringLength(50, ErrorMessage = "Please enter a Last Name with less than 50 characters.")]
        [Display(Name = "Last Name")]

        public string LastName { get; set; }
        [Display(Name = "Job Title")]
        [StringLength(50, ErrorMessage = "Please enter a Job Title with less than 50 characters.")]
        public string JobTitle { get; set; }

        [Display(Name = "Cell Phone")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64? CellPhone { get; set; }

        [Display(Name = "Work Phone")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64? WorkPhone { get; set; }

        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Category")]
        public ICollection<ContactCategories> ContactCategories { get; set; }
        public bool Active { get; set; }

        [StringLength(200, ErrorMessage = "No more than 200 characters for notes.")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "Company")]
        [Required(ErrorMessage = "Please select a Company.")]
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        [Display(Name = "Contact")]
        public string FullName => FirstName + " " + LastName;
    }
}
