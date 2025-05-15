namespace Infrastructure.Utils.Models
{
    public class PageCommit<T> where T : class
    {
        public T? Filter { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
