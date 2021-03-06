using hager_crm.Models.FilterConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Utils
{

    public interface IGridFilterPaginatable
    {
        public bool HasMoreItems { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
    }
    
    public interface IGridFilterFilterable
    {
        public Dictionary<string, string> OuterSorts { get; }
        public List<BaseFilterRule> FilterRules { get; }
    }
    
    public class GridFilter<TSource> : IGridFilterPaginatable, IGridFilterFilterable
    {

        /*
         * Pretty basic implementation of a grid filters based of model config.
         * C# is more restricted in types than I thought, 
         * that's why implementation may be not so good.
         * Known bugs: no type validation for incoming query.
         * Solution: Adjust config to check type or ignore
         */
        protected readonly IModelConfig<TSource> config;
        public List<BaseFilterRule> FilterRules => config.GetFilteringRules();
        
        protected int pageSize;
        public int PageSize => pageSize;
        
        protected int pageNumber = 1;
        public int PageNumber => pageNumber;
        
        protected bool hasMoreItems = false;
        public bool HasMoreItems => hasMoreItems;
        
        protected Dictionary<string, string> outerSorts;
        public Dictionary<string, string> OuterSorts => outerSorts;
        
        protected Dictionary<string, string> outerFields;
        public Dictionary<string, string> OuterFields => outerFields;
        
        protected readonly string[] orderDirs = new string[] { "DESC", "ASC" };

        public static GridFilter<TSource> CreateInstance<TSource>(IModelConfig<TSource> modelConfig, int pgSize = 20)
        {
            return new GridFilter<TSource>(modelConfig, pgSize);
        }
        
        public GridFilter(IModelConfig<TSource> modelConfig, int pgSize = 20)
        {
            config = modelConfig;
            pageSize = pgSize;
        }

        public void ParseQuery(HttpContext context)
        {
            var getQuery = context.Request.Query;
            outerFields = ExtractFields(getQuery);
            outerSorts = ExtractSorts(getQuery);
            pageNumber = ExtractPageNumber(getQuery);
            pageSize = ExtractPageSize(context.Request.Cookies);

        }

        private int ExtractPageSize(IRequestCookieCollection cookies)
        {
            if (int.TryParse(cookies["gridPageSize"], out int pSize) && pSize < 100 && pSize > 5)
                return pSize;
            return pageSize;
        }

        public async Task<List<TSource>> GetFilteredData(IQueryable<TSource> query)
        {
            query = ApplyFilter(query);
            query = ApplySort(query);
            query = ApplyPaging(query);
            var data = await query.ToListAsync();
            if (data.Count > PageSize)
            {
                hasMoreItems = true;
                data.RemoveAt(data.Count - 1);
            }
            return data;
        }

        protected virtual IQueryable<TSource> ApplyFilter(IQueryable<TSource> query)
        {
            var actions = config.GetActions();
            foreach (var field in outerFields.Keys)
                try
                {
                    query = actions[field].OnFilter(outerFields[field], query);
                }
                catch
                {
                    // ignored
                }

            return query;
        }

        protected virtual IQueryable<TSource> ApplySort(IQueryable<TSource> query)
        {
            var actions = config.GetActions();
            return actions[outerSorts["OrderField"]].OnSort(outerSorts["OrderDir"], query);
        }

        protected virtual IQueryable<TSource> ApplyPaging(IQueryable<TSource> query)
        {
            return query.Skip(PageSize * (PageNumber - 1)).Take(PageSize + 1);
        }

        public Dictionary<string, string> ExtractFields(IQueryCollection getQuery)
        {
            var result = new Dictionary<string, string>();
            foreach (var rule in config.GetActions().Keys)
                if (getQuery.ContainsKey(rule))
                    result[rule] = getQuery[rule];
            return result;
        }

        public int ExtractPageNumber(IQueryCollection getQuery)
        {
            return getQuery.ContainsKey("Page") && Int32.TryParse(getQuery["Page"], out int result) ? result : 1;
        }

        public Dictionary<string, string> ExtractSorts(IQueryCollection getQuery)
        {
            var types = config.GetActions();
            var result = new Dictionary<string, string>
            {
                { "OrderField", "ID"},
                { "OrderDir", "ASC"},
            };

            if (getQuery.ContainsKey("OrderField") && types.ContainsKey(getQuery["OrderField"]))
                result["OrderField"] = getQuery["OrderField"];

            if (getQuery.ContainsKey("OrderDir") && orderDirs.Any(i => i == getQuery["OrderDir"].ToString().ToUpper()))
                result["OrderDir"] = getQuery["OrderDir"].ToString().ToUpper();

            return result;
        }
    }
}
