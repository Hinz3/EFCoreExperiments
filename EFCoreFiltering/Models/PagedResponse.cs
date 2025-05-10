namespace EFCoreFiltering.Models
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; }


        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalCount)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (decimal)pageSize);
        }

        public PagedResponse() { }
    }
}
