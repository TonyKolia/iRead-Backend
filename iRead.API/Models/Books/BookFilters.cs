namespace iRead.API.Models.Books
{
    public class BookFilters
    {
        public int? CategoryId { get; set; }
        public IEnumerable<int>? Authors { get; set; }
        public IEnumerable<int>? Publishers { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear{ get; set; }
        public string? SearchString { get; set; }
        public string? Type { get; set; }
        public int? UserId { get; set; }
    }
}
