using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    //Bruno Vidal
    //2021-01-22
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Please enter a First Name.")]
        [StringLength(50, ErrorMessage = "Please enter a First Name with less than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a Last Name.")]
        [StringLength(50, ErrorMessage = "Please enter a Last Name with less than 50 characters.")]
        public string LastName { get; set; }

        [Display(Name = "Job Position")]
        [Required(ErrorMessage = "Please select a Job Position.")]
        public int JobPositionID { get; set; }

        [Display(Name = "Job Position")]
        public JobPosition JobPosition { get; set; }

        [Display(Name = "Employment Type")]
        [Required(ErrorMessage = "Please select an Employment Type.")]
        public int EmploymentTypeID { get; set; }

        [Display(Name = "Employment Type")]
        public EmploymentType EmploymentType { get; set; }

        [Display(Name = "Employee Address 1")]
        [Required(ErrorMessage = "Please enter a Employee Address.")]
        [StringLength(100, ErrorMessage = "Please enter a Employee Address with less then 100 characters.")]
        public string EmployeeAddress1 { get; set; }

        [Display(Name = "Employee Address 2")]
        [StringLength(50, ErrorMessage = "Please enter a Employee Address with less then 50 characters.")]
        public string EmployeeAddress2 { get; set; }

        [Required(ErrorMessage = "Please select a Employee Province.")]
        [Display(Name = "Employee Province")]
        public int EmployeeProvinceID { get; set; }

        [Display(Name = "Employee Province")]
        public Province EmployeeProvince { get; set; }

        [Required(ErrorMessage = "Please enter a Employee Postal Code.")]
        [Display(Name = "Postal Code")]
        [RegularExpression("^[A-Z][0-9][A-Z][0-9][A-Z][0-9]$", ErrorMessage = "Please enter a proper Postal Code.")]
        public string EmployeePostalCode { get; set; }

        [Required(ErrorMessage = "Please select a Employee Country")]
        [Display(Name = "Employee Country")]
        public int EmployeeCountryID { get; set; }

        [Display(Name = "Employee Country")]
        public Country EmployeeCountry { get; set; }

        [Display(Name = "Cell Phone")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 CellPhone { get; set; }

        [Display(Name = "Work Phone")]
        [Required(ErrorMessage = "Please enter a Work Phone Number.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 WorkPhone { get; set; }

        [Required(ErrorMessage = "Please enter an e-mail address.")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Please enter a Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Please enter a Wage for the employee.")]
        [DataType(DataType.Currency)]
        public decimal Wage { get; set; }

        [Required(ErrorMessage = "Please enter a Expense for the employee.")]
        [DataType(DataType.Currency)]
        public decimal Expense { get; set; }

        [Display(Name = "Date Joined")]
        [Required(ErrorMessage = "Please enter the Date Joined")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateJoined { get; set; }

        [Display(Name = "Key Fob #")]
        [Required(ErrorMessage = "Please enter the Key Fob #.")]
        [DisplayFormat(DataFormatString = "{0:###:####}", ApplyFormatInEditMode = false)]
        [StringLength(7, ErrorMessage = "Please enter a Key Fob with just 7 characters.")]
        public string KeyFob { get; set; }

        [Display(Name = "Emergency Contact Name")]
        [Required(ErrorMessage = "Please enter an Emergency Contact Name.")]
        [StringLength(50, ErrorMessage = "Please enter a Emergency Contact Name with less than 50 characters.")]
        public string EmergencyContactName { get; set; }

        [Display(Name = "Emergency Contact Phone")]
        [Required(ErrorMessage = "Please enter a Emergency Contact Phone Number.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 EmergencyContactPhone { get; set; }
        public bool Active { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        public bool IsUser => !string.IsNullOrEmpty(UserId);

        public string UserId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => FirstName + " " + LastName;
    }
}
