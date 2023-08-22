using readerzone_api.Data;
using readerzone_api.Dtos;
using readerzone_api.Exceptions;
using readerzone_api.Models;
using readerzone_api.Services.AuthorService;
using readerzone_api.Services.GenreService;
using readerzone_api.Services.PublisherService;

namespace readerzone_api.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly ReaderZoneContext _readerZoneContext;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;        

        public BookService(ReaderZoneContext readerZoneContext, IGenreService genreService, 
                           IAuthorService authorService, IPublisherService publisherService)
        {
            _readerZoneContext = readerZoneContext;
            _genreService = genreService;
            _authorService = authorService;
            _publisherService = publisherService;            
        }

        public Book AddBook(BookDto bookDto)
        {
            var book = _readerZoneContext.Books.FirstOrDefault(b => b.ISBN == bookDto.ISBN);
            if (book == null)
            {
                book = new Book()
                {
                    ISBN = bookDto.ISBN,
                    Title = bookDto.Title,
                    PublishingDate = DateTime.ParseExact(bookDto.PublishingDate, "dd.MM.yyyy.", null, System.Globalization.DateTimeStyles.None),
                    Stocks = bookDto.Stocks,
                    Pages = bookDto.Pages,
                    Language = bookDto.Language,
                    Weight = bookDto.Weight,
                    Height = bookDto.Height,
                    Width = bookDto.Width,
                    Price = bookDto.Price,
                    ImageUrl = bookDto.ImageUrl,
                    Genres = GetGenresByName(bookDto.Genres),
                    Authors = GetAuthorsById(bookDto.AuthorIds),
                    Publisher = _publisherService.GetPublisherById(bookDto.PublisherId)
                };
                _readerZoneContext.Books.Add(book);
                _readerZoneContext.SaveChanges();
                return book;
            } else
            {
                throw new NotCreatedException($"Book with ISBN {bookDto.ISBN} already exists.");
            }         
        }
      
        private ICollection<Author> GetAuthorsById(List<int> ids)
        {
            ICollection<Author> authors = new List<Author>();
            foreach (var id in ids)
            {
                authors.Add(_authorService.GetAuthorById(id));
            }
            return authors;
        }

        private ICollection<Genre> GetGenresByName(List<string> names)
        {
            ICollection<Genre> genres = new List<Genre>();
            foreach (var name in names)
            {
                genres.Add(_genreService.GetGenreByName(name));
            }
            return genres;
        }
    }
}
