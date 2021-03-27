using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class VendorType : BaseLookup<VendorType>
    {
        public VendorType()
        {
            Companies = new HashSet<Company>();
            CompanyVendors = new HashSet<CompanyVendor>();

        }
        public int VendorTypeID { get; set; }

        [Display(Name = "Vendor Type")]
        [StringLength(40, ErrorMessage = "Please enter a Type with less than 40 characters")]
        public string Type { get; set; }

        // For Multiselect list
        public ICollection<CompanyVendor> CompanyVendors { get; set; }
        public ICollection<Company> Companies { get; set; }
        
        [NotMapped]
        public override string DisplayName
        {
            get => Type;
            set => Type = value;
        }

        public override int GetId() => VendorTypeID;

        public override string GetLookupName() => "vendor_type";
    }
}
