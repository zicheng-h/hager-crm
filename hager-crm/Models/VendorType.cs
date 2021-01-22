using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class VendorType
    {
        public VendorType()
        {
            Companies = new HashSet<Company>();

        }
        public int VendorTypeID { get; set; }
        [Display(Name = "Vendor Type")]
        [StringLength(40, ErrorMessage = "Please enter a Type with less than 40 characters")]
        public string Type { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}
