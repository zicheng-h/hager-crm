using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    //Bruno Vidal
    //2021-01-22
    public class Company
    {
        public Company()
        {
            CreatedAt = DateTime.Now;
            Contacts = new HashSet<Contact>();
            Calendars = new HashSet<Calendar>();
            CompanyCustomers = new HashSet<CompanyCustomer>();
            CompanyContractors = new HashSet<CompanyContractor>();
            CompanyVendors = new HashSet<CompanyVendor>();
        }
        public int CompanyID { get; set; }
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please enter the Company Name")]
        [StringLength(50, ErrorMessage = "Please enter a Company Name with less than 50 characters.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Please enter the Company Location with less than 50 characters.")]
        public string Location { get; set; }
        [Display(Name = "Credit Check")]
        public bool CreditCheck { get; set; }
        [Display(Name = "Last Checked")]
        public DateTime? DateChecked { get; set; }
        [Display(Name = "Billing Term")]
        public int? BillingTermID { get; set; }
        [Display(Name="Billing Term")]
        public BillingTerm BillingTerm { get; set; }
        [Display(Name="Currency")]
        public int? CurrencyID { get; set; }
        public Currency Currency { get; set; }

        [RegularExpression("^\\d{10}$",ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64? Phone { get; set; }
        [StringLength(50, ErrorMessage = "Please enter the Company's Website.")]
        public string Website { get; set; }

        [Display(Name = "Billing Address 1")]
        [StringLength(100, ErrorMessage = "Please enter a Billing Address with less then 100 characters.")]
        public string BillingAddress1 { get; set; }
        [Display(Name = "Billing Address 2")]
        [StringLength(50, ErrorMessage = "Please enter a Billing Address with less then 50 characters.")]
        public string BillingAddress2 { get; set; }

        [Display(Name="Billing City")]
        public string BillingCity { get; set; }

        [Display(Name = "Billing Province")]
        public int? BillingProvinceID { get; set; }
        [Display(Name = "Billing Province")]
        public Province BillingProvince { get; set; }

        [Display(Name="Billing Postal Code")]
        [RegularExpression("^[A-Z][0-9][A-Z][0-9][A-Z][0-9]$", ErrorMessage = "Please enter a proper Postal Code.")]
        public string BillingPostalCode { get; set; }
        [Display(Name = "Billing Country")]
        public int? BillingCountryID { get; set; }
        [Display(Name = "Billing Country")]
        public Country BillingCountry { get; set; }

        [Display(Name = "Shipping Address 1")]
        [StringLength(100, ErrorMessage = "Please enter a Shipping Address with less then 100 characters.")]
        public string ShippingAddress1 { get; set; }
        [Display(Name = "Shipping Address 2")]
        [StringLength(50, ErrorMessage = "Please enter a Shipping Address with less then 50 characters.")]
        public string ShippingAddress2 { get; set; }

        [Display(Name ="Shipping City")]
        public string ShippingCity { get; set; }
        [Display(Name = "Shipping Province")]
        public int? ShippingProvinceID { get; set; }

        [Display(Name = "Shipping Province")]
        public Province ShippingProvince { get; set; }

        [Display(Name = "Shipping Postal Code")]
        [RegularExpression("^[A-Z][0-9][A-Z][0-9][A-Z][0-9]$", ErrorMessage = "Please enter a proper Postal Code.")]
        public string ShippingPostalCode { get; set; }
        [Display(Name = "Shipping Country")]
        public int? ShippingCountryID { get; set; }
        [Display(Name = "Shipping Country")]
        public Country ShippingCountry { get; set; }

        public bool Active { get; set; }
        [StringLength(200)]
        public string Notes { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Calendar> Calendars { get; set; }

        // For multiselect list
        public ICollection<CompanyCustomer> CompanyCustomers { get; set; }
        public ICollection<CompanyContractor> CompanyContractors { get; set; }
        public ICollection<CompanyVendor> CompanyVendors { get; set; }
    }
}
