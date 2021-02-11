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
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a Last Name.")]
        [StringLength(50, ErrorMessage = "Please enter a Last Name with less than 50 characters.")]
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
        public ICollection<ContactCategories> ContactCategories { get; set; }
        public bool Active { get; set; }
        [StringLength(200)]
        public string Notes { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        [Display(Name = "Contact")]
        public string FullName
        {
            get
            {
                return FirstName + LastName;
            }
        }

    }
}
