using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Dtos;
using readerzone_api.Models;
using readerzone_api.Services.CustomerService;
using System.Security.Claims;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly ReaderZoneContext _readerZoneContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(ReaderZoneContext readerZoneContext, IHttpContextAccessor httpContextAccessor)
        {
            _readerZoneContext = readerZoneContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public void GenerateReview(Customer customer, PurchasedBook book, string title, string text, int rating)
        {
            var review = new Review()
            {
                PostingTime = DateTime.Now,
                Likes = 0,
                Customer = customer,
                Text = text,
                Title = title,
                Rating = rating,
                PurchasedBook = book
            };
            _readerZoneContext.Reviews.Add(review);
            _readerZoneContext.SaveChanges();
            SetNewAverageRating(book);
        }

        public void GeneratePurchasedBookPost(Customer customer, ICollection<Book> books)
        {            
            foreach (var book in books)
            {                
                var post = new AutomaticPost()
                {
                    PostingTime = DateTime.Now,
                    Likes = 0,
                    Customer = customer,
                    Text = GetPostText(BookStatus.WantToRead, customer, book.Title, book.Authors)
                };
                _readerZoneContext.AutomaticPosts.Add(post);
                _readerZoneContext.SaveChanges();
            }
        }

        public void GenerateChangedBookStatusPost(Customer customer, PurchasedBook book, BookStatus status)
        {
            var post = new AutomaticPost()
            {
                PostingTime = DateTime.Now,
                Likes = 0,
                Customer = customer,
                Text = GetPostText(status, customer, book.Book.Title, book.Book.Authors)
            };
            _readerZoneContext.AutomaticPosts.Add(post);
            _readerZoneContext.SaveChanges();
        }

        private string GetPostText(BookStatus status, Customer customer, string title, ICollection<Author> authors)
        {
            switch(status)
            {
                case BookStatus.WantToRead:
                    return $"{customer.Name} {customer.Surname} (@{customer.UserAccount.Username}) " +
                           $"wants to read '{title}' by {authors.First().Name} {authors.First().Surname}.";
                case BookStatus.Reading:
                    return $"{customer.Name} {customer.Surname} (@{customer.UserAccount.Username}) " +
                           $"is reading '{title}' by {authors.First().Name} {authors.First().Surname}."; ;
                case BookStatus.Read:
                    return $"{customer.Name} {customer.Surname} (@{customer.UserAccount.Username}) " +
                           $"read '{title}' by {authors.First().Name} {authors.First().Surname}."; ;
                default:
                    return title;

            }
        }

        private void SetNewAverageRating(PurchasedBook purchasedBook)
        {
            var newAverageRating = purchasedBook.Book.AverageRating;
            var purchasedBooks = _readerZoneContext.PurchasedBooks
                                                   .Where(pb => pb.Book.Id == purchasedBook.Book.Id && pb.Review != null)
                                                   .ToList();

            if (purchasedBooks.Count == 0)
            {
                newAverageRating = 3.5;
            } 
            else
            {
                double totalRating = purchasedBooks.Sum(pb => pb.Review.Rating);
                newAverageRating = totalRating / purchasedBooks.Count;
            }
            var book = _readerZoneContext.Books.FirstOrDefault(b => b.Id == purchasedBook.Book.Id);
            if (book != null)
            {
                book.AverageRating = newAverageRating;
                _readerZoneContext.SaveChanges();
            }            
        }

        public List<PostDto> GetCustomerPosts(int pageNumber, int pageSize, out int totalPosts)
        {
            List<PostDto> posts = new();
            var result = -1;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            var automaticPosts = _readerZoneContext.AutomaticPosts
                                                   .Include(ap => ap.Customer)
                                                   .ThenInclude(c => c.UserAccount)
                                                   .Include(ap => ap.Comments)
                                                   .ThenInclude(c => c.Customer)
                                                   .ThenInclude(c => c.UserAccount)
                                                   .Where(post => post.CustomerId == result)
                                                   .Select(post => new PostDto
                                                   {
                                                       Id = post.Id,
                                                       PostingTime = post.PostingTime,
                                                       Likes = post.Likes,
                                                       CustomerId = post.CustomerId,
                                                       CustomerUsername = post.Customer.UserAccount.Username,
                                                       CustomerName = post.Customer.Name,
                                                       CustomerSurname = post.Customer.Surname,
                                                       CustomerImageUrl = post.Customer.ImageUrl,
                                                       Type = "Automatic",
                                                       Text = post.Text,
                                                       Comments = post.Comments.Select(comment => new CommentDto
                                                       {
                                                           Id = comment.Id,
                                                           PostingTime = comment.PostingTime,
                                                           Likes = comment.Likes,
                                                           Text = comment.Text,
                                                           CustomerId = comment.CustomerId,
                                                           CustomerUsername = comment.Customer.UserAccount.Username,
                                                           CustomerName = comment.Customer.Name,
                                                           CustomerSurname = comment.Customer.Surname,
                                                           CustomerImageUrl = comment.Customer.ImageUrl
                                                       }).ToList()
                                                   })
                                                   .ToList();

            var reviewPosts = _readerZoneContext.Reviews
                                                .Include(ap => ap.Customer)
                                                .ThenInclude(c => c.UserAccount)
                                                .Include(ap => ap.Comments)
                                                .ThenInclude(c => c.Customer)
                                                .ThenInclude(c => c.UserAccount)
                                                .Include(r => r.PurchasedBook)
                                                .ThenInclude(pb => pb.Book)
                                                .ThenInclude(b => b.Authors)
                                                .Where(post => post.CustomerId == result)
                                                .Select(post => new PostDto
                                                {
                                                    Id = post.Id,
                                                    PostingTime = post.PostingTime,
                                                    Likes = post.Likes,
                                                    CustomerId = post.CustomerId,
                                                    CustomerUsername = post.Customer.UserAccount.Username,
                                                    CustomerName = post.Customer.Name,
                                                    CustomerSurname = post.Customer.Surname,
                                                    CustomerImageUrl = post.Customer.ImageUrl,
                                                    Type = "Review",
                                                    Text = post.Text,
                                                    Title = post.Title,
                                                    Rating = post.Rating,
                                                    PurchasedBookId = post.PurchasedBookId,
                                                    Isbn = post.PurchasedBook.Book.ISBN,
                                                    BookTitle = post.PurchasedBook.Book.Title,
                                                    AuthorId = post.PurchasedBook.Book.Authors.First().Id,
                                                    AuthorName = post.PurchasedBook.Book.Authors.First().Name,
                                                    AuthorSurname = post.PurchasedBook.Book.Authors.First().Surname,
                                                    BookImageUrl = post.PurchasedBook.Book.ImageUrl,
                                                    Comments = post.Comments.Select(comment => new CommentDto
                                                    {
                                                        Id = comment.Id,
                                                        PostingTime = comment.PostingTime,
                                                        Likes = comment.Likes,
                                                        Text = comment.Text,
                                                        CustomerId = comment.CustomerId,
                                                        CustomerUsername = comment.Customer.UserAccount.Username,
                                                        CustomerName = comment.Customer.Name,
                                                        CustomerSurname = comment.Customer.Surname,
                                                        CustomerImageUrl = comment.Customer.ImageUrl
                                                    }).ToList()
                                                })
                                                .ToList();

            totalPosts = automaticPosts.Concat(reviewPosts).Count();
            
            var combinedPosts = automaticPosts.Concat(reviewPosts)
                                              .OrderByDescending(post => post.PostingTime)
                                              .Skip((pageNumber - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToList();

            return combinedPosts;
        
        }
    }
}
