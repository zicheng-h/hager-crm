using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using hager_crm.Data;
using hager_crm.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace hager_crm.Models
{
    public abstract class BaseLookup<TEntity>: ILookupManage where TEntity: BaseLookup<TEntity>, new()
    {
        public abstract int GetId();
        
        [NotMapped]
        public abstract string DisplayName { get; set; }
        public int? Order { get; set; }

        public abstract string GetLookupName();

        public async Task<List<ILookupManage>> GetAll(DbContext context)
        {
            return await context.Set<TEntity>()
                .Select(i => (ILookupManage)i)
                .OrderBy(i => i.Order ?? 0)
                .ToListAsync();
        }
        
        public async Task<int> AddLookup(DbContext context, string displayName)
        {
            var entity = new TEntity
            {
                DisplayName = displayName,
                Order = context.Set<TEntity>().Count()
            };
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.GetId();
        }

        public async Task<bool> UpdateLookup(DbContext context, int lookupId, string displayName)
        {
            var entity = await context.FindAsync<TEntity>(lookupId);
            if (entity == null)
                return false;
            entity.DisplayName = displayName;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLookup(DbContext context, int lookupId)
        {
            var entity = await context.FindAsync<TEntity>(lookupId);
            if (entity == null)
                return false;
            context.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateLookupOrder(DbContext context, int[] order)
        {
            for (int i = 0; i < order.Length; i++)
            {
                var entity = await context.FindAsync<TEntity>(order[i]);
                if (entity == null)
                    return false;
                entity.Order = i;
            }
            await context.SaveChangesAsync();
            return true;
        }
    }
}