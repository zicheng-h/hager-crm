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
    public class EmploymentType : BaseLookup<EmploymentType>
    {
        public EmploymentType()
        {
            Employees = new HashSet<Employee>();
        }

        public int EmploymentTypeID { get; set; }

        [Display(Name = "Employment Type")]
        [StringLength(40, ErrorMessage = "Please enter a Type with less than 40 characters")]
        public string Type { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public override int GetId() => EmploymentTypeID;
        
        [NotMapped]
        public override string DisplayName
        {
            get => Type;
            set => Type = value;
        }

        public override string GetLookupName() => "employment_type";
        
    }
}
