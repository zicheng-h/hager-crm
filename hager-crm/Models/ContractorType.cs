using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class ContractorType : BaseLookup<ContractorType>
    {
        public ContractorType()
        {
            Companies = new HashSet<Company>();
            CompanyContractors = new HashSet<CompanyContractor>();
        }
        
        public int ContractorTypeID { get; set; }
        [Display(Name = "Contractor Type")]
        [StringLength(40, ErrorMessage = "Please enter a Type with less than 40 characters")]
        public string Type { get; set; }

        // For Multiselect list
        public ICollection<CompanyContractor> CompanyContractors { get; set; }
        public ICollection<Company> Companies { get; set; }

        public override int GetId() => ContractorTypeID;
        
        [NotMapped]
        public override string DisplayName
        {
            get => Type;
            set => Type = value;
        }

        public override string GetLookupName() => "contractor_type";
    }
}
