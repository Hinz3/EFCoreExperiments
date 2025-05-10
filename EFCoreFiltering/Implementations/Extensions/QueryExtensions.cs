using EFCoreFiltering.Attributes;
using EFCoreFiltering.Implementations.Helpers;
using EFCoreFiltering.Interfaces;
using EFCoreFiltering.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EFCoreFiltering.Implementations.Extensions;

public static class QueryExtensions
{
    /// <summary>
    /// Get paged response
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <param name="query">EF Core query</param>
    /// <param name="requestQuery">Filtering, sorting and pagination parameters from the API request query</param>
    /// <param name="addFilter">Add filtering based on the API request query parameters</param>
    /// <param name="addSort">Add sorting based on the API request query parameters</param>
    /// <returns>Paged response with list of data and properties set</returns>
    public static async Task<PagedResponse<TEntity>> ToListPagedAsync<TEntity>(this IQueryable<TEntity> query, IRequestQuery requestQuery, bool addFilter = true, bool addSort = true)
    {
        var pageCount = requestQuery.Page;
        var pageSize = requestQuery.PageSize;

        if (addFilter) query = query.AddFilters(requestQuery.Filters);

        var total = await query.CountAsync();

        if (addSort) query = query.AddOrders(requestQuery.Sorts);
        var data = await query.Skip((pageCount - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

        return new PagedResponse<TEntity>(data, pageCount, pageSize, total);
    }

    /// <summary>
    /// Add filter expressions by property name
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <param name="query">EF Core query</param>
    /// <param name="filters">List of filters</param>
    /// <returns>Query with filter</returns>
    public static IQueryable<TEntity> AddFilters<TEntity>(this IQueryable<TEntity> query, List<Filter> filters)
    {
        var entityType = typeof(TEntity);
        if (entityType.CheckHasAttribute<Unfilterable>())
        {
            return query;
        }

        foreach (var filter in filters)
        {
            if (string.IsNullOrEmpty(filter.Property) || string.IsNullOrEmpty(filter.Value)) { continue; }

            var property = entityType.GetProperty(filter.Property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property.CheckHasAttribute<Unfilterable>()) { continue; }

            query = query.Where(GetFilterExpressions<TEntity>(filter));
        }

        return query;
    }

    /// <summary>
    /// Add order by by expressions property name 
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <param name="query">EF Core query</param>
    /// <param name="sorts">List of sorts</param>
    /// <returns>Query with order</returns>
    public static IQueryable<TEntity> AddOrders<TEntity>(this IQueryable<TEntity> query, List<Sort> sorts)
    {
        var entityType = typeof(TEntity);
        if (entityType.CheckHasAttribute<Unfilterable>())
        {
            return query;
        }

        foreach (var sort in sorts)
        {
            if (string.IsNullOrEmpty(sort.Property)) { continue; }

            var property = entityType.GetProperty(sort.Property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property.CheckHasAttribute<Unfilterable>()) { continue; }

            query = sort.IsAscending
                ? query.OrderBy(GetSortExpression<TEntity>(sort.Property))
                : query.OrderByDescending(GetSortExpression<TEntity>(sort.Property));
        }

        return query;
    }

    private static Expression<Func<T, bool>> GetFilterExpressions<T>(Filter filter)
    {
        var paramter = Expression.Parameter(typeof(T));
        var propName = Expression.PropertyOrField(paramter, filter.Property);
        var targetType = propName.Type;
        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            targetType = Nullable.GetUnderlyingType(targetType);
        var constExpression = Expression.Constant(Convert.ChangeType(filter.Value, targetType), propName.Type);

        Expression filterExpression;
        switch (filter.Operator.ToLower())
        {
            case Operator.Equal:
                filterExpression = Expression.Equal(propName, constExpression);
                break;
            case Operator.GreaterThanOrEqual:
                filterExpression = Expression.GreaterThanOrEqual(propName, constExpression);
                break;
            case Operator.LessThanOrEqual:
                filterExpression = Expression.LessThanOrEqual(propName, constExpression);
                break;
            case Operator.GreaterThan:
                filterExpression = Expression.GreaterThan(propName, constExpression);
                break;
            case Operator.LessThan:
                filterExpression = Expression.LessThan(propName, constExpression);
                break;
            case Operator.NotEqual:
                filterExpression = Expression.NotEqual(propName, constExpression);
                break;
            case Operator.Contains:
                if (filter.Value.GetType() != typeof(string))
                    throw new InvalidFilterCriteriaException();
                var containsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);
                filterExpression = Expression.Call(propName, containsMethodInfo, constExpression);
                break;
            default:
                throw new InvalidOperationException();
        }

        return Expression.Lambda<Func<T, bool>>(filterExpression, paramter);
    }

    private static Expression<Func<TSource, object>> GetSortExpression<TSource>(string propertyName)
    {
        var param = Expression.Parameter(typeof(TSource), "x");
        Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(object));
        return Expression.Lambda<Func<TSource, object>>(conversion, param);
    }
}
