using EFCoreFiltering.Configurations;
using EFCoreFiltering.Interfaces;
using EFCoreFiltering.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace EFCoreFiltering.Implementations;

public class RequestQuery(IHttpContextAccessor contextAccessor, FilteringConfiguration configuration) : IRequestQuery
{
    private readonly HttpContext context = contextAccessor?.HttpContext;
    private readonly FilteringConfiguration configuration = configuration;

    public int Page
    {
        get
        {
            var query = GetParameter("Page");
            if (string.IsNullOrEmpty(query) || !int.TryParse(query, out var page)) return 1;
            return page;
        }
    }
    public int PageSize
    {
        get
        {
            var query = GetParameter("PageSize");
            if (string.IsNullOrEmpty(query) || !int.TryParse(query, out var page)) return configuration.DefaultPageCount;
            return page > configuration.MaxPageSize ? configuration.MaxPageSize : page;
        }
    }
    public List<Filter> Filters
    {
        get
        {
            var query = GetParameter("Filter");
            if (string.IsNullOrEmpty(query)) return [];

            return JsonSerializer.Deserialize<List<Filter>>(query);
        }
    }

    public List<Sort> Sorts
    {
        get
        {
            var query = GetParameter("Sort");
            if (string.IsNullOrEmpty(query)) return [];

            return JsonSerializer.Deserialize<List<Sort>>(query);
        }
    }


    private string GetParameter(string parameterName)
    {
        foreach (var query in context?.Request?.Query?.ToList() ?? [])
        {
            if (query.Key.Equals(parameterName, StringComparison.OrdinalIgnoreCase))
                return query.Value;
        }

        return null;
    }
}
