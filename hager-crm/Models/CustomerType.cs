using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using hager_crm.Data;
using hager_crm.ViewModels;

namespace hager_crm.Models
{
    public class CustomerType : BaseLookup<CustomerType>
    {
        public CustomerType()
        {
            Companies = new HashSet<Company>();
            CompanyCustomers = new HashSet<CompanyCustomer>();
        }

        public int CustomerTypeID { get; set; }

        [Display(Name = "Customer Type")]
        [StringLength(40, ErrorMessage = "Please enter a Type with less than 40 characters")]
        public string Type { get; set; }

        // For Multiselect list
        public ICollection<CompanyCustomer> CompanyCustomers { get; set; }
        public ICollection<Company> Companies { get; set; }

        public override int GetId() => CustomerTypeID;
        
        [NotMapped]
        public override string DisplayName
        {
            get => Type;
            set => Type = value;
        }

        public override string GetLookupName() => "customer_type";

    }
}
