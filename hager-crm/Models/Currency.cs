using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class Currency : BaseLookup<Currency>
    {
        public Currency()
        {
            Companies = new HashSet<Company>();
        }
        public int CurrencyID { get; set; }
        [Display(Name = "Currency")]
        [StringLength(30, ErrorMessage = "Please enter a Currency with less than 30 characters")]
        public string CurrencyName { get; set; }
        public ICollection<Company> Companies { get; set; }
        
        public override int GetId() => CurrencyID;
        
        [NotMapped]
        public override string DisplayName
        {
            get => CurrencyName;
            set => CurrencyName = value;
        }

        public override string GetLookupName() => "currency";
    }
}
