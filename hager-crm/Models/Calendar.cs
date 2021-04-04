using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class Calendar : IValidatableObject
    {
        public int CalendarId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Date.Date < DateTime.Today)
            {
                yield return new ValidationResult("Calendar date cannot be in the past.", new[] { "Date" });
            }
        }
    }
}
