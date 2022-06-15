using iRead.API.Models;
using iRead.API.Models.Recommendation;

namespace iRead.API.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookResponse>> GetAllBooks();
        Task<BookResponse> GetBook(int id);
        Task<IEnumerable<BookResponse>> GetBooksByCategory(int categoryId);
        Task<IEnumerable<BookResponse>> GetBooksByFilters(IEnumerable<int> authors, IEnumerable<int> publishers, int? minYear, int? maxYear, int? categoryId, IEnumerable<string> searchItems, string type, int? userId);
        Task<IEnumerable<BookResponse>> GetBooksByAuthors(IEnumerable<int> authors);
        Task<IEnumerable<BookResponse>> GetBooksByPublishers(IEnumerable<int> publishers);
        Task<IEnumerable<BookResponse>> GetBooksByIds(IEnumerable<int> ids);
        Task<RelatedBookRecommendations> GetRecommendedByUserAndBook(int bookId, int userId);
        Task<RelatedBookRecommendations> GetRecommendedByBook(int bookId);
        Task UpdateBookStock(IEnumerable<int> books);
        Task<int> GetBookStock(int bookId);
        Task<int> GetMinPublishYear();
        Task<int> GetMaxPublishYear();

    }
}
