using readerzone_api.Dtos;
using readerzone_api.Models;

namespace readerzone_api.Services.BookService
{
    public interface IBookService
    {
        public Book AddBook(BookDto bookDto);

    }
}
