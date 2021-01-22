using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class BillingTerm
    {
        public BillingTerm()
        {
            Companies = new HashSet<Company>();
        }
        public int BillingID { get; set; }
        [Display(Name = "Billing Terms")]
        [StringLength(50, ErrorMessage = "Please enter a Term with less than 50 characters")]
        public string Terms { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}
