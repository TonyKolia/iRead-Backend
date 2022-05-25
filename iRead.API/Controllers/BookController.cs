using iRead.API.Models;
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

        [HttpGet]
        [Route("Category/{category}/Authors/{authors}/Publishers/{publishers}/MinYear/{minYear}/MaxYear/{maxYear}/SearchString/{searchString}")]
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetBooksByFilters(string category, string authors, string publishers, string minYear, string maxYear, string searchString)
        {
            try
            {
                int? categoryId = null;
                int? minYearValue = null;
                int? maxYearValue = null;
                var authorList = new List<int>();
                var publisherList = new List<int>();
                var searchItems = new List<string>();

                if (!string.IsNullOrEmpty(category) && category != "ALL")
                    categoryId = int.Parse(category);

                if (!string.IsNullOrEmpty(authors) && authors != "ALL")
                    authorList = authors.Split('-').ContertToInteger().ToList();

                if (!string.IsNullOrEmpty(publishers) && publishers != "ALL")
                    publisherList = publishers.Split('-').ContertToInteger().ToList();

                if (!string.IsNullOrEmpty(minYear) && minYear != "ALL")
                    minYearValue = int.Parse(minYear);

                if (!string.IsNullOrEmpty(maxYear) && maxYear != "ALL")
                    maxYearValue = int.Parse(maxYear);

                if (!string.IsNullOrEmpty(searchString) && searchString != "%%%")
                    searchItems = searchString.Split(' ').ToList();

                var books = await _bookRepository.GetBooksByFilters(authorList, publisherList, minYearValue, maxYearValue, categoryId, searchItems);
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
