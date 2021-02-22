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
            Contacts = new HashSet<Contact>();
        }
        public int CompanyID { get; set; }
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please enter the Company Name")]
        [StringLength(50, ErrorMessage = "Please enter a Company Name with less than 50 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the Location")]
        [StringLength(50, ErrorMessage = "Please enter a Location with less than 50 characters.")]
        public string Location { get; set; }
        [Display(Name = "Credit Check")]
        public bool CreditCheck { get; set; }
        [Display(Name = "Last Checked")]
        public DateTime DateChecked { get; set; }
        public int BillingTermID { get; set; }
        public BillingTerm BillingTerm { get; set; }
        public int CurrencyID { get; set; }
        public Currency Currency { get; set; }
        [Required(ErrorMessage = "Please enter a Phone Number.")]
        [RegularExpression("^\\d{10}$",ErrorMessage = "Please enter a proper Phone number with 10 digits without spaces.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 Phone { get; set; }
        [StringLength(50, ErrorMessage = "Please enter the Company's Website.")]
        public string Website { get; set; }
        [Display(Name = "Billing Address 1")]
        [Required(ErrorMessage = "Please enter a Billing Address.")]
        [StringLength(100, ErrorMessage = "Please enter a Billing Address with less then 100 characters.")]
        public string BillingAddress1 { get; set; }
        [Display(Name = "Billing Address 2")]
        [StringLength(50, ErrorMessage = "Please enter a Billing Address with less then 50 characters.")]
        public string BillingAddress2 { get; set; }
        [Required(ErrorMessage = "Please select a Billing Province.")]
        public int BillingProvinceID { get; set; }
        [Display(Name = "Billing Province")]
        public Province BillingProvince { get; set; }
        [Required(ErrorMessage = "Please enter a Billing Postal Code.")]
        [RegularExpression("^[A-Z][0-9][A-Z][0-9][A-Z][0-9]$", ErrorMessage = "Please enter a proper Postal Code.")]
        public string BillingPostalCode { get; set; }
        [Required(ErrorMessage = "Please select a Billing Country")]
        public int BillingCountryID { get; set; }
        [Display(Name = "Billing Country")]
        public Country BillingCountry { get; set; }
        [Display(Name = "Shipping Address 1")]
        [Required(ErrorMessage = "Please enter a Shipping Address.")]
        [StringLength(100, ErrorMessage = "Please enter a Shipping Address with less then 100 characters.")]
        public string ShippingAddress1 { get; set; }
        [Display(Name = "Shipping Address 2")]
        [StringLength(50, ErrorMessage = "Please enter a Shipping Address with less then 50 characters.")]
        public string ShippingAddress2 { get; set; }
        [Required(ErrorMessage = "Please select a Shipping Province.")]
        public int ShippingProvinceID { get; set; }
        [Display(Name = "Shipping Province")]
        public Province ShippingProvince { get; set; }
        [Required(ErrorMessage = "Please enter a Shipping Postal Code.")]
        [RegularExpression("^[A-Z][0-9][A-Z][0-9][A-Z][0-9]$", ErrorMessage = "Please enter a proper Postal Code.")]
        public string ShippingPostalCode { get; set; }
        [Required(ErrorMessage = "Please select a Shipping Country")]
        public int ShippingCountryID { get; set; }
        [Display(Name = "Shipping Country")]
        public Country ShippingCountry { get; set; }
        public bool Customer { get; set; }
        public int? CustomerTypeID { get; set; }
        [Display(Name = "Customer Type.")]
        public CustomerType CustomerType { get; set; }
        public bool Vendor { get; set; }
        public int? VendorTypeID { get; set; }
        [Display(Name = "Vendor Type.")]
        public VendorType VendorType { get; set; }
        public bool Contractor { get; set; }
        public int? ContractorTypeID { get; set; }
        [Display(Name = "Contractor Type.")]
        public ContractorType ContractorType { get; set; }
        public bool Active { get; set; }
        [StringLength(200)]
        public string Notes { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
