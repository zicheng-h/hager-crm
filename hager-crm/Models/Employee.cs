using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace hager_crm.Models
{
    //Bruno Vidal
    //2021-01-22
    public class Employee
    {

        public Employee()
        {
            UnreadAnnouncements = new HashSet<AnnouncementEmployee>();
        }
        
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Please enter a First Name.")]
        [StringLength(50, ErrorMessage = "Please enter a First Name with less than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a Last Name.")]
        [StringLength(50, ErrorMessage = "Please enter a Last Name with less than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Job Position")]
        public int? JobPositionID { get; set; }

        [Display(Name = "Job Position")]
        public JobPosition JobPosition { get; set; }

        [Display(Name = "Employment Type")]
        public int? EmploymentTypeID { get; set; }

        [Display(Name = "Employment Type")]
        public EmploymentType EmploymentType { get; set; }

        [Display(Name = "Employee Address 1")]
        [StringLength(100, ErrorMessage = "Please enter a Employee Address with less then 100 characters.")]
        public string EmployeeAddress1 { get; set; }

        [Display(Name = "Employee Address 2")]
        [StringLength(50, ErrorMessage = "Please enter a Employee Address with less then 50 characters.")]
        public string EmployeeAddress2 { get; set; }

        [Display(Name = "Employee City")]
        public string EmployeeCity { get; set; }

        [Display(Name = "Employee Province")]
        public int? EmployeeProvinceID { get; set; }

        [Display(Name = "Employee Province")]
        public Province EmployeeProvince { get; set; }

        [Display(Name = "Postal Code")]
        [RegularExpression("^[A-Z][0-9][A-Z][0-9][A-Z][0-9]$", ErrorMessage = "Please enter a proper Postal Code.")]
        public string EmployeePostalCode { get; set; }

        [Display(Name = "Employee Country")]
        public int? EmployeeCountryID { get; set; }

        [Display(Name = "Employee Country")]
        public Country EmployeeCountry { get; set; }

        [Display(Name = "Cell Phone")]
        [RegularExpression("^$|^\\d{10}$", ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
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

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Wage { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Expense { get; set; }

        [Display(Name = "Date Joined")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateJoined { get; set; }

        [Display(Name = "Key Fob #")]
        [RegularExpression("^\\d{7}$", ErrorMessage = "Please enter a proper key fob number with 7 digits without spaces.")]
        [DisplayFormat(DataFormatString = "{0:####:#####}", ApplyFormatInEditMode = false)]
        public Int32? KeyFob { get; set; }

        [Display(Name = "Contact Name")]
        [StringLength(50, ErrorMessage = "Please enter a Emergency Contact Name with less than 50 characters.")]
        public string EmergencyContactName { get; set; }

        [Display(Name = "Contact Phone")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64? EmergencyContactPhone { get; set; }
        public bool Active { get; set; }

        [StringLength(200, ErrorMessage = "No more than 200 characters for notes.")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "Is User")]
        public bool IsUser => !string.IsNullOrEmpty(UserId);

        public string UserId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => FirstName + " " + LastName;
        
        public ICollection<AnnouncementEmployee> UnreadAnnouncements { get; set; }
    }
}
