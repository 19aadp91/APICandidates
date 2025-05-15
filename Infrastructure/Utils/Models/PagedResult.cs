namespace Infrastructure.Utils.Models
{
    public class PagedResult<T> where T : class
    {
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public IEnumerable<T> Items { get; set; } = [];
    }
}
