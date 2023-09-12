using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
using readerzone_api.Models;
using readerzone_api.Services.BookService;

namespace readerzone_api.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{isbn}")]
        public ActionResult<Book> GetBook(string isbn)
        {
            var book = _bookService.GetBook(isbn);
            return Ok(book);
        }             

        [HttpPost]
        public ActionResult<Book> AddBook(BookDto bookDto)
        {
            var book = _bookService.AddBook(bookDto);
            return Ok(book);
        }

        [HttpPost("books")]
        public ActionResult GetBooks(PaginationQuery paginationQuery)
        {
            var books = _bookService.GetBooks(paginationQuery, out int totalBooks);
            return Ok(new { TotalBooks = totalBooks, Books = books });
        }

        [HttpGet("recommended")]
        public ActionResult<List<Book>> GetRecommendedBooks()
        {
            var books = _bookService.GetRecommendedBooks();
            return Ok(books);
        }

        [HttpPut, Authorize(Roles = "Admin, Manager")]
        public ActionResult UpdateBook(BookDto bookDto)
        {
            _bookService.UpdateBook(bookDto);
            return Ok();
        }
    }
}