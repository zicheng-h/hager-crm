using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class BillingTerm : BaseLookup<BillingTerm>
    {
        public BillingTerm()
        {
            Companies = new HashSet<Company>();
        }
        public int BillingTermID { get; set; }
        [Display(Name = "Billing Terms")]
        [Required(ErrorMessage = "Please enter a Term")]
        [StringLength(50, ErrorMessage = "Please enter a Term with less than 50 characters")]
        public string Terms { get; set; }
        public ICollection<Company> Companies { get; set; }

        [NotMapped]
        public override string DisplayName
        {
            get => Terms;
            set => Terms = value;
        }

        public override int GetId() => BillingTermID;

        public override string GetLookupName() => "billing_term";
    }
}
