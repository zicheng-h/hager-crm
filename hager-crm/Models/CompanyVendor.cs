using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class CompanyVendor
    {
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public int VendorTypeID { get; set; }
        public VendorType VendorType { get; set; }
    }
}
