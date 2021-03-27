using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class CompanyCustomer
    {
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public int CustomerTypeID { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
