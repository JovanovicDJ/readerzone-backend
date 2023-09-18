using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
using readerzone_api.Models;
using readerzone_api.Services.AuthorService;

namespace readerzone_api.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet, Authorize(Roles = "Admin, Manager")]
        public ActionResult<List<Author>> GetAuthors()
        {
            var authors = _authorService.GetAuthors();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(int id)
        {
            var author = _authorService.GetAuthorById(id);
            return Ok(author);
        }

        [HttpPost, Authorize(Roles = "Admin, Manager")]
        public ActionResult<Author> AddAuthor(AuthorDto authorDto)
        {
            var author = _authorService.AddAuthor(authorDto.Name, authorDto.Surname);
            return Ok(author);
        }

        [HttpGet("books/{id}")]
        public ActionResult<List<BookData>> GetAuthorBooks(int id)
        {
            var books = _authorService.GetAuthorBooks(id);
            return Ok(books);
        }
    }
}
