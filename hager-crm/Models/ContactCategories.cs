using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Models
{
    public class ContactCategories
    {
        public int ContactID { get; set; }
        public Contact Contact { get; set; }

        public int CategoriesID { get; set; }
        public Categories Categories { get; set; }

        
    }
}
