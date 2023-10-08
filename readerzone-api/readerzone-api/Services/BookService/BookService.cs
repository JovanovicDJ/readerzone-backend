using Microsoft.EntityFrameworkCore;
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

        public Book GetBook(string isbn)
        {
            var book = _readerZoneContext.Books
                               .Include(b => b.Publisher)
                               .ThenInclude(p => p.Address)
                               .Include(b => b.Genres)
                               .Include(b => b.Authors)
                               .Where(book => book.ISBN.Equals(isbn))
                               .FirstOrDefault();
            return book == null ? throw new NotFoundException($"Book with ISBN {isbn} was not found!") : book;
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

        public void UpdateBook(BookDto bookDto)
        {
            var book = _readerZoneContext.Books.Include(b => b.Authors).Include(b => b.Genres).FirstOrDefault(b => b.ISBN == bookDto.ISBN);
            if (book != null)
            {
                book.Authors.Clear();
                book.Genres.Clear();
                _readerZoneContext.SaveChanges();
                book.ISBN = bookDto.ISBN;
                book.Title = bookDto.Title;
                book.PublishingDate = DateTime.ParseExact(bookDto.PublishingDate, "dd.MM.yyyy.", null, System.Globalization.DateTimeStyles.None);
                book.Stocks = bookDto.Stocks;
                book.Pages = bookDto.Pages;
                book.Language = bookDto.Language;
                book.Weight = bookDto.Weight;
                book.Height = bookDto.Height;
                book.Width = bookDto.Width;
                book.Price = bookDto.Price;
                book.ImageUrl = bookDto.ImageUrl;
                book.Genres = GetGenresByName(bookDto.Genres);
                book.Authors = GetAuthorsById(bookDto.AuthorIds);
                book.Publisher = _publisherService.GetPublisherById(bookDto.PublisherId);
                book.Discount = bookDto.Discount;                                
                _readerZoneContext.SaveChanges();                
            }
            else
            {
                throw new NotFoundException($"Book with ISBN {bookDto.ISBN} was not found.");
            }
        }

        public List<Book> GetBooks(PaginationQuery pq, out int totalBooks)
        {
            int booksToSkip = (pq.PageNumber - 1) * pq.PageSize;

            var query = _readerZoneContext.Books
                               .Include(b => b.Publisher)
                               .ThenInclude(p => p.Address)
                               .Include(b => b.Genres)
                               .Include(b => b.Authors)
                               .Where(book =>
                                     (string.IsNullOrEmpty(pq.SearchKeyword) ||
                                      book.Title.Contains(pq.SearchKeyword) ||
                                      book.Authors.Any(author => author.Name.Contains(pq.SearchKeyword) || author.Surname.Contains(pq.SearchKeyword)) ||
                                      book.Publisher.Name.Contains(pq.SearchKeyword)) &&

                                      (pq.SelectedGenres.Count == 0 ||
                                      book.Genres.Any(g => pq.SelectedGenres.Contains(g.Name))) &&

                                      (book.Price >= pq.MinPrice && book.Price <= pq.MaxPrice))
                               .AsSplitQuery()
                               .OrderByDescending(item => item.Id);                               

            totalBooks = query.Count();

            var booksOnPage = query
                               .Skip(booksToSkip)
                               .Take(pq.PageSize)
                               .ToList();

            return booksOnPage;
        }

        public List<Review> GetBookReviews(string isbn)
        {
            var reviews = _readerZoneContext.Reviews.Include(r => r.Customer).Where(r => r.PurchasedBook.Book.ISBN == isbn).OrderByDescending(r => r.Id).Take(5).ToList();
            return reviews;
        }

        public List<Book> GetRecommendedBooks()
        {
            var books = _readerZoneContext.Books
                               .Include(b => b.Publisher)
                               .ThenInclude(p => p.Address)
                               .Include(b => b.Genres)
                               .Include(b => b.Authors)
                               .OrderBy(book => Guid.NewGuid())
                               .Take(5)
                               .ToList();
            return books;
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
