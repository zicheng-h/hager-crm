using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using hager_crm.Models.FilterConfig;
using hager_crm.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hager_crm.Helpers
{
    public static class GridFilterHelper
    {
        /*
         * This is actually pretty insane.
         * I could not find information how to create razor helpers and use them everywhere.
         * So I decided to create an html helper.
         * Due to huge html snippet I decided to to string interpolation instead of TabBuilder.
         * Hrefs in links are not implemented and being used by jQuery to compose final url for filtering
         */
        public static HtmlString GeneratePagination(this IHtmlHelper html, IGridFilterPaginatable gridFilter)
        {
            var isFirstPage = gridFilter.PageNumber <= 1;
            var result = $@"
                <div class=""d-flex justify-content-center"">
                    <div class=""form-group d-flex mr-2"">
                        <label for=""grid-page-size"" style=""min-width:75px"">Page Size:</label>
                        <select id=""grid-page-size"" class=""form-control"">
                            {string.Join('\n', new [] {5, 10, 20, 50}
                            .Select(p => $@"<option value=""{p}"" {(p == gridFilter.PageSize ? "selected" : "")}>{p}</option>"))
                            }
                        </select>
                    </div>
                    <nav aria-label=""Table pagination"">
                        <ul class=""pagination"">
                            <li class=""page-item {(isFirstPage ? "disabled" : "")}"">
                                <a class=""page-link grid-filter-page""
                                   data-page=""{(isFirstPage ? 1 : gridFilter.PageNumber - 1)}""
                                   href=""#"">
                                    Previous
                                </a>
                            </li>
                            { (!isFirstPage ? 
                                $@"<li class=""page-item"">
                                    <a class=""page-link grid-filter-page""
                                       data-page=""{gridFilter.PageNumber - 1}""
                                       href=""#"">
                                        {gridFilter.PageNumber - 1}
                                    </a>
                                </li>" : "")}
                            <li class=""page-item active""><a class=""page-link grid-filter-page"" href=""#"">{gridFilter.PageNumber}</a></li>
                            { (gridFilter.HasMoreItems ? 
                                $@"<li class=""page-item"">
                                    <a class=""page-link grid-filter-page""
                                       data-page=""{(gridFilter.HasMoreItems ? gridFilter.PageNumber + 1 : gridFilter.PageNumber)}""
                                       href=""#"">
                                        {gridFilter.PageNumber + 1}
                                    </a>
                                </li>" : "")}
                            <li class=""page-item {(gridFilter.HasMoreItems ? "" : "disabled")}"">
                                <a class=""page-link grid-filter-page""
                                   data-page=""{(gridFilter.HasMoreItems ? gridFilter.PageNumber + 1 : gridFilter.PageNumber)}""
                                   href=""#"">
                                    Next
                                </a>
                            </li>
                        </ul>
                    </nav>
                </div>
            ";
            
            return new HtmlString(result);
        }
        
        /*
         * Wanted to implement good auto filter mapper, but realised that I don't have enough time for that.
         * In general, grid filter should be rewritten to use data annotations and metadata from EF.
         * I would also use reflection singleton with all configs.
         */
        private static HtmlString GetFilteringOption(this IHtmlHelper html, BaseFilterRule rule)
        {
            
            var result = $@"
                <div class=""data-filter-rule col-12 col-md-6 mb-2 d-flex flex-row"">
                    <label class=""data-filter-label control-label mr-2 col-5"" 
                           for=""{rule.FieldName + "FilterRule"}"">{rule.DisplayName}: </label>
                    {rule.GetHtml(html.ViewContext)}
                </div>
            ";
            return new HtmlString(result);
        }
        
        public static HtmlString GenerateFiltering(this IHtmlHelper html, IGridFilterFilterable gridFilter)
        {
            var result = $@"
                <button class=""btn btn-outline-primary my-2"" 
                        type=""button"" 
                        data-toggle=""collapse"" 
                        data-target=""#filter-data-collapse"" 
                        aria-expanded=""false"" 
                        aria-controls=""filter-data-collapse"">
                    Open Filtering Menu
                </button>
                <div class=""grid-filter-panel my-3"">
                    <div id=""filter-data-collapse"" class=""card card-body collapse"">
                        <div class=""row"">
                            {string.Join('\n', gridFilter.FilterRules.Select(html.GetFilteringOption))}
                        </div>
                        <div class=""row"">
                            <div class=""col"">
                                <div class=""form-group mt-2"">
                                    <button class=""btn btn-primary data-search-btn"">Filter</button>
                                    <button class=""btn btn-primary data-reset-btn ml-2"">Reset</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            ";
            return new HtmlString(result);
        }

        public static HtmlString GenerateFiltered(this IHtmlHelper html, IGridFilterFilterable gridFilter)
        {
            var result = $@"
                <button class=""btn btn-warning my-2"" 
                        type=""button"" 
                        data-toggle=""collapse"" 
                        data-target=""#filter-data-collapse"" 
                        aria-expanded=""false"" 
                        aria-controls=""filter-data-collapse"">
                    Open Filtering Menu
                </button>
                <div class=""grid-filter-panel my-3"">
                    <div id=""filter-data-collapse"" class=""card card-body collapse"">
                        <div class=""row"">
                            {string.Join('\n', gridFilter.FilterRules.Select(html.GetFilteringOption))}
                        </div>
                        <div class=""row"">
                            <div class=""col"">
                                <div class=""form-group mt-2"">
                                    <button class=""btn btn-primary data-search-btn"">Filter</button>
                                    <button class=""btn btn-primary data-reset-btn ml-2"">Reset</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            ";
            return new HtmlString(result);
        }
    }
}
