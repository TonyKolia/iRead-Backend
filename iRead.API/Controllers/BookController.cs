using iRead.API.Models;
using iRead.API.Models.Books;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : CustomControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository _bookRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._bookRepository = _bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponse>>> Get()
        {
            var books = await _bookRepository.GetAllBooks();
            return ReturnIfNotEmpty(books, "No books found.", false);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BookResponse>> Get(int id)
        {
            var book = await _bookRepository.GetBook(id);
            return ReturnIfNotEmpty(book, $"Book with id {id} not found.", false);
        }

        [HttpGet]
        [Route("Multiple/{ids}")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> Get(string ids)
        {
            try
            {
                var books = await _bookRepository.GetBooksByIds(ids.Split('-').ContertToInteger());
                return ReturnIfNotEmpty(books, "No books found for the provided ids.", false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        [HttpGet]
        [Route("Category/{id}")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksByCategory(int id)
        {
            var books = await _bookRepository.GetBooksByCategory(id);
            return ReturnIfNotEmpty(books, "No books found for this category", false);
        }

        [HttpGet]
        [Route("Authors/{authors}")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksByAuthors(string authors)
        {
            var books = await _bookRepository.GetBooksByAuthors(authors.Split('-').ContertToInteger());
            return ReturnIfNotEmpty(books, "No books found for these filters.", false);
        }

        [HttpGet]
        [Route("Publishers/{publishers}")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksByPublishers(string publishers)
        {
            var books = await _bookRepository.GetBooksByPublishers(publishers.Split('-').ContertToInteger());
            return ReturnIfNotEmpty(books, "No books found for these filters.", false);
        }

        [HttpPost]
        [Route("GetByFilters")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksByFilters([FromBody] BookFilters filters)
        {
            try
            {
                var searchItems = !string.IsNullOrEmpty(filters.SearchString) ? filters.SearchString.Split(' ').ToList() : new List<string>(); 
                var books = await _bookRepository.GetBooksByFilters(filters.Authors, filters.Publishers, filters.MinYear, filters.MaxYear, filters.CategoryId, searchItems, filters.Type, filters.UserId);
                return ReturnIfNotEmpty(books, "No books found for these filters.", false);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }

        }
    }
}
