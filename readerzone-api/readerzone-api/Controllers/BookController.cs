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

        [HttpPost]
        public ActionResult<Book> AddBook(BookDto bookDto)
        {
            var book = _bookService.AddBook(bookDto);
            return Ok(book);
        }
    }
}