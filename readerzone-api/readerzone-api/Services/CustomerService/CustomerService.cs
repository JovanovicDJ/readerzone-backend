using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Dtos;
using readerzone_api.Exceptions;
using readerzone_api.Models;
using readerzone_api.Services.PostService;
using System.Security.Claims;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly ReaderZoneContext _readerZoneContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostService _postService;

        public CustomerService(ReaderZoneContext readerZoneContext, IHttpContextAccessor httpContextAccessor,
                               IPostService postService)
        {
            _readerZoneContext = readerZoneContext;
            _httpContextAccessor = httpContextAccessor;
            _postService = postService;
        }        

        public Customer GetCustomerByEmail(string email)
        {
            var userAccount = _readerZoneContext.UserAccounts.Include(ua => ua.User)
                                                             .ThenInclude(u => u.Address)                                                             
                                                             .FirstOrDefault(ua => ua.Email == email);
            if (userAccount != null && userAccount.Role.Equals(Role.Customer))
            {
                userAccount.PasswordHash = new byte[32];
                userAccount.PasswordSalt = new byte[32];
                return (Customer)userAccount.User;
            }
            else
            {
                throw new NotFoundException($"User with email {email} does not exist.");
            }
        }

        public Customer GetCustomerById(int id)
        {
            var customer = _readerZoneContext.Customers.Include(c => c.UserAccount)
                                                       .Include(c => c.Address)
                                                       .FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                customer.UserAccount.PasswordSalt = new byte[32];
                customer.UserAccount.PasswordHash = new byte[32];
                return customer;
            }
            else
            {
                throw new NotFoundException($"Customer with ID {id} does not exist");
            }
        }

        public void AddPurchasedBooks(string email, double price, ICollection<Book> books)
        {
            var customer = GetCustomerWithPassword(email);
            foreach (var book in books)
            {
                var purchasedBook = new PurchasedBook()
                {
                    BookStatus = BookStatus.WantToRead,
                    Book = book,
                    Customer = customer                    
                };
                _readerZoneContext.PurchasedBooks.Add(purchasedBook);
                _readerZoneContext.SaveChanges();
            }
            AddPoints(customer, price);
        }

        public Customer GetCustomerWithPassword(string email)  // In function GetCustomerByEmail I need to keep password safe, here I don't do that
        {
            var userAccount = _readerZoneContext.UserAccounts.Include(ua => ua.User)
                                                             .ThenInclude(u => u.Address)
                                                             .FirstOrDefault(ua => ua.Email == email);
            if (userAccount != null && userAccount.Role.Equals(Role.Customer))
            {                
                return (Customer)userAccount.User;
            }
            else
            {
                throw new NotFoundException($"User with email {email} does not exist.");
            }
        }

        public List<PurchasedBook> GetPurchasedBooksByCustomerId(int customerId, BookStatus status)
        {            
            var books = _readerZoneContext.PurchasedBooks.Include(pb => pb.Book)
                                                         .ThenInclude(b => b.Authors)
                                                         .Include(pb => pb.Book.Publisher)
                                                         .Include(pb => pb.Review)
                                                         .Where(pb => pb.CustomerId  == customerId && pb.BookStatus.Equals(status))
                                                         .ToList();
            return books;
        }

        public void UpdatePurchasedBookStatus(int purchasedBookId, BookStatus status)
        {
            var result = String.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var customer = GetCustomerWithPassword(result);
            var book = _readerZoneContext.PurchasedBooks.Include(pb => pb.Book)
                                                        .ThenInclude(b => b.Authors)
                                                        .Include(pb => pb.Book.Publisher)
                                                        .FirstOrDefault(pb => pb.Id == purchasedBookId && pb.CustomerId == customer.Id);
            if (book == null)
            {
                throw new NotFoundException($"Purchased book with ID {purchasedBookId} was not found.");
            }
            book.BookStatus = status;
            _readerZoneContext.SaveChanges();
            _postService.GenerateChangedBookStatusPost(customer, book, status);
        }

        public void AddReview(string title, string text, int rating, int purchasedBookId)
        {
            var result = String.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var customer = GetCustomerWithPassword(result);
            var book = _readerZoneContext.PurchasedBooks.Include(pb => pb.Book).FirstOrDefault(pb => pb.Id == purchasedBookId && pb.CustomerId == customer.Id);
            if (book == null)
            {
                throw new NotFoundException($"Purchased book with ID {purchasedBookId} was not found.");
            }
            _postService.GenerateReview(customer, book, title, text, rating);
        }        

        private void AddPoints(Customer customer, double price)
        {
            customer.Points = Math.Round(customer.Points + price, 2);
            if (customer.Points >= 0 && customer.Points < 200)
            {
                customer.Tier = Tier.Bronze;
            }
            else if (customer.Points >= 200 && customer.Points < 400)
            {
                customer.Tier = Tier.Silver;
            }
            else if (customer.Points >= 400 && customer.Points < 600)
            {
                customer.Tier = Tier.Gold;
            }
            else if (customer.Points >= 600)
            {
                customer.Tier = Tier.Platinum;
            }
            _readerZoneContext.SaveChanges();
        }

        public void UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            var result = String.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var customer = GetCustomerWithPassword(result);
            customer.UserAccount.Username = updateCustomerDto.Username;
            customer.Name = updateCustomerDto.Name;
            customer.Surname = updateCustomerDto.Surname;
            customer.PhoneNumber = updateCustomerDto.PhoneNumber;
            customer.ImageUrl = updateCustomerDto.ImageUrl;
            customer.Address.Street = updateCustomerDto.Street;
            customer.Address.Number = updateCustomerDto.Number;
            customer.Address.City = updateCustomerDto.City;
            customer.Address.PostalCode = updateCustomerDto.PostalCode;
            customer.Address.Country = updateCustomerDto.Country;
            _readerZoneContext.SaveChanges();
        }

        public List<BookData> GetBooksDataByCustomerId(int id)
        {
            List<BookData> booksData = new();
            var books = _readerZoneContext.PurchasedBooks.Include(pb => pb.Book)
                                                         .ThenInclude(b => b.Authors)                                                                                                                  
                                                         .Where(pb => pb.CustomerId == id)
                                                         .ToList();
            if (books != null)
            {
                foreach(var book in books)
                {
                    var sb = new BookData()
                    {
                        Isbn = book.Book.ISBN,
                        Title = book.Book.Title,
                        BookStatus = book.BookStatus,
                        Author = book.Book.Authors.First().Name + " " + book.Book.Authors.First().Surname,
                        AuthorId = book.Book.Authors.First().Id,
                        ImageUrl = book.Book.ImageUrl
                    };
                    booksData.Add(sb);
                }
            }
            return booksData;
        }
    }
}
