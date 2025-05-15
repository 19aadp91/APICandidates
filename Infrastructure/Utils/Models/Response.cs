namespace Infrastructure.Utils.Models
{
    public class Response<T> where T : class
    {
        public T? Body { get; set; }
        public IEnumerable<Error>? Errors { get; set; }
    }
}
