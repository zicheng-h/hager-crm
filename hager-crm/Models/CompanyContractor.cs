using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class CompanyContractor
    {
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public int ContractorTypeID { get; set; }
        public ContractorType ContractorType { get; set; }
    }
}
