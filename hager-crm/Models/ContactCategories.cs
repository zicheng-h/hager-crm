using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class ContactCategories
    {
        [Required(ErrorMessage = "Please select a Categories.")]
        [Display(Name = "Categories")]
        public int CategoriesID { get; set; }
        public Categories Categories { get; set; }
        [Required(ErrorMessage = "Please select an Contact.")]
        [Display(Name = "Contact")]
        public int ContactID { get; set; }
        public Contact Contact { get; set; }
    }
}
