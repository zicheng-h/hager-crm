using System.Collections.Generic;

namespace hager_crm.ViewModels
{
    public class CompanyTypesDto
    {
        public int TypeID { get; set; }
        public string SecondaryInfo = "";
        public string DisplayName { get; set; }
    }
    
    public class CompanyTypesVM
    {
        public CompanyTypesVM()
        {
            TypesIn = new HashSet<CompanyTypesDto>();
            TypesAvailable = new HashSet<CompanyTypesDto>();
        }
        public IEnumerable<CompanyTypesDto> TypesIn { get; set; }
        public IEnumerable<CompanyTypesDto> TypesAvailable { get; set; }
        public string Category { get; set; }
    }
}
