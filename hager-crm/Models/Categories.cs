using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class Categories : BaseLookup<Categories>
    {
        public Categories()
        {
            ContactCategories = new HashSet<ContactCategories>();
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a Category.")]
        [StringLength(40, ErrorMessage = "Please enter a Category with less than 40 characters.")]
        [Display(Name = "Categories")]
        public string Category { get; set; }

        public ICollection<ContactCategories> ContactCategories { get; set; }

        [NotMapped]
        public override string DisplayName
        {
            get => Category;
            set => Category = value;
        }

        public override int GetId() => ID;

        public override string GetLookupName() => "contact_category";
    }
}
