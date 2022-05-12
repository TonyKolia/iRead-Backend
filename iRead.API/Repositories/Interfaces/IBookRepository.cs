using iRead.API.Models;

namespace iRead.API.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookResponse>> GetAllBooks();
        Task<BookResponse> GetBook(int id);
        Task<IEnumerable<BookResponse>> GetBooksByCategory(int categoryId);
        Task<IEnumerable<BookResponse>> GetBooksByIds(IEnumerable<int> ids);
    }
}
