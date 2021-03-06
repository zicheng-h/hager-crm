using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace hager_crm.ViewModels
{
    public interface ILookupManage
    {
        public int GetId();
        public Task<List<ILookupManage>> GetAll(DbContext context);
        public string DisplayName { get; set; }
        public int? Order { get; set; }
        public string GetLookupName();
        public Task<int> AddLookup(DbContext context, string displayName);
        public Task<bool> UpdateLookup(DbContext context, int lookupId, string displayName);
        public Task<bool> DeleteLookup(DbContext context, int lookupId);
        public Task<bool> UpdateLookupOrder(DbContext context, int[] order);
    }
}