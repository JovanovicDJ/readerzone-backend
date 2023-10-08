using readerzone_api.Dtos;
using readerzone_api.Models;

namespace readerzone_api.Services.BookService
{
    public interface IBookService
    {
        public Book AddBook(BookDto bookDto);
        public List<Book> GetBooks(PaginationQuery paginationQuery, out int totalBooks);
        public Book GetBook(string isbn);
        public List<Book> GetRecommendedBooks();
        public void UpdateBook(BookDto bookDto);
        public List<Review> GetBookReviews(string isbn);
    }
}
