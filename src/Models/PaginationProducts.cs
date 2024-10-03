using Microsoft.EntityFrameworkCore;

namespace src.Models
{
    public class PaginationProducts<T>
    {
        public int TotalItems   { get; set; }

        public int PageSize     { get; set; }

        public List<T> items    { get; set; }

        public int PageIndex    { get; set; }

        public int TotalPages   { get; private set; }


        public PaginationProducts(List<T> items, int count, int pageIndex, int pageSize) 
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            this.items = items;
        }

        public bool HasPreviousPage => (PageIndex > 1);

        public bool HasNextPage => (PageIndex < TotalPages);

        public int FirstItemIndex => (PageIndex - 1) * PageSize + 1;

        public int LastItemIndex => Math.Min(PageIndex * PageSize, TotalItems);


        public static async Task<PaginationProducts<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex -1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginationProducts<T>(items, count, pageIndex, pageSize);
        }
    }
}
