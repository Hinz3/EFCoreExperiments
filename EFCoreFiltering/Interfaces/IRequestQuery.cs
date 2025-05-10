using EFCoreFiltering.Models;

namespace EFCoreFiltering.Interfaces;

public interface IRequestQuery
{
    int Page { get; }
    int PageSize { get; }
    List<Filter> Filters { get; }
    List<Sort> Sorts { get; }
}
