using System.Collections.Generic;
using System.Linq;
using hager_crm.Utils;

namespace hager_crm.ViewModels
{
    public class LookupVM
    {
        public string LookupName = "";
        public string GetHumanLookupName() => LookupName.Replace('_', ' ').Capitalize();
        public List<ILookupManage> Items;
    }
}