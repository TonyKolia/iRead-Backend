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
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }
    }
}
