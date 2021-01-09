using Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Api.Utility.Pagination
{
    public class DynamicPagination
    {
        public class DataTableAjaxPostModel
        {
            // properties are not capital due to json mapping
            public int draw { get; set; }
            public int start { get; set; }
            public int length { get; set; }
            public List<Column> columns { get; set; }
            public Search search { get; set; }
            public List<Order> order { get; set; }
        }

        public class Column
        {
            public string data { get; set; }
            public string name { get; set; }
            public bool searchable { get; set; }
            public bool orderable { get; set; }
            public Search search { get; set; }
        }

        public class Search
        {
            public string value { get; set; }
            public string regex { get; set; }
        }

        public class Order
        {
            public int column { get; set; }
            public string dir { get; set; }
        }

        public static List<dynamic> GetDataFromDbaseAsync(DbContext _context, Type entity, string searchBy, int take, int skip, string sortBy, string sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchQuery = "";
            if (String.IsNullOrEmpty(searchBy))
            {
                searchQuery = "@0 == @0";
                searchBy = "1";
            }
            else
            {
                searchQuery = "";
                var properties = entity.GetProperties().ToList();
                foreach (var item in properties)
                {
                    if (item.PropertyType == typeof(DateTime?))
                    {
                        DateTime date = new DateTime();
                        switch (searchBy.Length)
                        {
                            case 4:
                                if (DateTime.TryParse(searchBy+"-01-01", out date))
                                {
                                    searchQuery += $"{item.Name}.Value.Year == {date.Year} || ";
                                }
                                break;
                            case 7:
                                if (DateTime.TryParse(searchBy + "-01", out date))
                                {
                                    searchQuery += $"({item.Name}.Value.Year == {date.Year} && " +
                                                   $"{item.Name}.Value.Month == {date.Month}) || ";
                                }
                                break;
                            case 10:
                                if (DateTime.TryParse(searchBy, out date))
                                {
                                    searchQuery += $"({item.Name}.Value.Year == {date.Year} && " +
                                                   $"{item.Name}.Value.Month == {date.Month} && " +
                                                   $"{item.Name}.Value.Day == {date.Day}) || ";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (item.PropertyType == typeof(string))
                    {
                        searchQuery += item.Name;
                        searchQuery += ".Contains(@0) || ";
                    }
                }
                searchQuery = searchQuery.Substring(0, searchQuery.Length - 4);
            }    

            List<dynamic> result = new List<dynamic>();

            result = _context.Query(entity)
                           .OrderBy(sortBy + " " + sortDir) // have to give a default order when skipping .. so use the PK
                           .Where(searchQuery, searchBy)
                           .Skip(skip)
                           .Take(take)
                           .ToDynamicList();

            filteredResultsCount = _context.Query(entity)
                           .Where(searchQuery, searchBy)
                           .Count();

            totalResultsCount = _context.Query(entity)
                           .Count();

            return result;
        }

        public static List<dynamic> DSearchFunc(DbContext _context, Type entity, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "date";
            string sortDir = "asc";

            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
            }

            // search the dbase taking into consideration table sorting and paging
            var result = GetDataFromDbaseAsync(_context, entity, searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                // empty collection...
                return new List<dynamic>();
            }
            return result;
        }
    }
}
