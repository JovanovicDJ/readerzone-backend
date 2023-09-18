using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Dtos;
using readerzone_api.Exceptions;
using readerzone_api.Models;
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

        public List<PostDto> GetCustomerPosts(int pageNumber, int pageSize, int customerId, out int totalPosts)
        {
            List<PostDto> posts = new();           
            var automaticPosts = _readerZoneContext.AutomaticPosts
                                                   .Include(ap => ap.Customer)
                                                   .ThenInclude(c => c.UserAccount)
                                                   .Include(ap => ap.Comments)
                                                   .ThenInclude(c => c.Customer)
                                                   .ThenInclude(c => c.UserAccount)
                                                   .Where(post => post.CustomerId == customerId)
                                                   .Select(post => new PostDto
                                                   {
                                                       Id = post.Id,
                                                       PostingTime = post.PostingTime,                                                       
                                                       CustomerId = post.CustomerId,
                                                       CustomerUsername = post.Customer.UserAccount.Username,
                                                       CustomerName = post.Customer.Name,
                                                       CustomerSurname = post.Customer.Surname,
                                                       CustomerImageUrl = post.Customer.ImageUrl,
                                                       Type = "Automatic",
                                                       Text = post.Text,
                                                       Comments = post.Comments
                                                                      .Where(comment => !comment.Deleted)
                                                                      .Select(comment => new CommentDto
                                                                      {
                                                                          Id = comment.Id,
                                                                          PostingTime = comment.PostingTime,                                                           
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
                                                .Where(post => post.CustomerId == customerId)
                                                .Select(post => new PostDto
                                                {
                                                    Id = post.Id,
                                                    PostingTime = post.PostingTime,                                                    
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
                                                    Comments = post.Comments
                                                                   .Where(comment => !comment.Deleted)
                                                                   .Select(comment => new CommentDto
                                                                   {
                                                                       Id = comment.Id,
                                                                       PostingTime = comment.PostingTime,                                                        
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

        public List<PostDto> GetFriendsPosts(int pageNumber, int pageSize, out int totalPosts)
        {
            var friendIds = GetFriendsIds(GetLoggedUserId());
            List<PostDto> posts = new();
            var automaticPosts = _readerZoneContext.AutomaticPosts.Include(ap => ap.Customer).ThenInclude(c => c.UserAccount)
                                                   .Include(ap => ap.Comments).ThenInclude(c => c.Customer).ThenInclude(c => c.UserAccount)
                                                   .Where(post => friendIds.Contains(post.CustomerId))
                                                   .Select(post => new PostDto
                                                   {
                                                       Id = post.Id,
                                                       PostingTime = post.PostingTime,                                                       
                                                       CustomerId = post.CustomerId,
                                                       CustomerUsername = post.Customer.UserAccount.Username,
                                                       CustomerName = post.Customer.Name,
                                                       CustomerSurname = post.Customer.Surname,
                                                       CustomerImageUrl = post.Customer.ImageUrl,
                                                       Type = "Automatic",
                                                       Text = post.Text,
                                                       Comments = post.Comments
                                                                      .Where(comment => !comment.Deleted)
                                                                      .Select(comment => new CommentDto
                                                                      {
                                                                          Id = comment.Id,
                                                                          PostingTime = comment.PostingTime,                                                                          
                                                                          Text = comment.Text,
                                                                          CustomerId = comment.CustomerId,
                                                                          CustomerUsername = comment.Customer.UserAccount.Username,
                                                                          CustomerName = comment.Customer.Name,
                                                                          CustomerSurname = comment.Customer.Surname,
                                                                          CustomerImageUrl = comment.Customer.ImageUrl
                                                                      }).ToList()
                                                   })
                                                   .ToList();

            var reviewPosts = _readerZoneContext.Reviews.Include(ap => ap.Customer).ThenInclude(c => c.UserAccount)
                                                .Include(ap => ap.Comments).ThenInclude(c => c.Customer).ThenInclude(c => c.UserAccount)
                                                .Include(r => r.PurchasedBook).ThenInclude(pb => pb.Book).ThenInclude(b => b.Authors)
                                                .Where(post => friendIds.Contains(post.CustomerId))
                                                .Select(post => new PostDto
                                                {
                                                    Id = post.Id,
                                                    PostingTime = post.PostingTime,                                                   
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
                                                    Comments = post.Comments
                                                                   .Where(comment => !comment.Deleted)
                                                                   .Select(comment => new CommentDto
                                                                   {
                                                                       Id = comment.Id,
                                                                       PostingTime = comment.PostingTime,                                                                       
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

        public CommentDto CommentPost(int postId, string text)
        {
            var loggedUserId = GetLoggedUserId();
            var customer = _readerZoneContext.Customers.Include(c => c.UserAccount)                                                       
                                                       .FirstOrDefault(c => c.Id == loggedUserId);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {loggedUserId} could have not commented post with ID {postId}.");
            }
            var post = _readerZoneContext.Posts.FirstOrDefault(p => p.Id == postId);
            if (post == null)
            {
                throw new NotFoundException($"Post with ID {postId} was not found.");
            }
            var comment = new Comment()
            {
                PostingTime = DateTime.Now,                
                Text = text,
                Post = post,
                Customer = customer,
                Deleted = false
            };
            _readerZoneContext.Comments.Add(comment);
            _readerZoneContext.SaveChanges();

            if (loggedUserId != post.CustomerId)
            {
                SendCommentOnPostNotification(customer, post);
            }

            var commentDto = new CommentDto()
            {                            
                Id = comment.Id,
                PostingTime = comment.PostingTime,                
                Text = comment.Text,
                CustomerId = customer.Id,
                CustomerUsername = customer.UserAccount.Username,
                CustomerName = customer.Name,
                CustomerSurname = customer.Surname,
                CustomerImageUrl = customer.ImageUrl
            };            
            return commentDto;
        }

        private void SendCommentOnPostNotification(Customer loggedCustomer, Post post)
        {
            var notification = new Notification()
            {
                CustomerId = post.CustomerId,
                FromCustomerId = loggedCustomer.Id,
                FromCustomerName = loggedCustomer.Name,
                FromCustomerSurname = loggedCustomer.Surname,
                FromCustomerUsername = loggedCustomer.UserAccount.Username,
                Text = $"{loggedCustomer.Name} {loggedCustomer.Surname} commented your post.",
                SendingTime = DateTime.Now,
                NotificationType = NotificationType.CommentOnPost,
                Deleted = false
            };
            _readerZoneContext.Notifications.Add(notification);
            _readerZoneContext.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            var comment = _readerZoneContext.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with ID {comment} was not found.");
            }
            comment.Deleted = true;
            _readerZoneContext.SaveChanges();
        }

        private int GetLoggedUserId()
        {
            var result = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            return result;
        }

        private List<int> GetFriendsIds(int customerId)
        {
            var friendIds = new List<int>();
            var customer = _readerZoneContext.Customers.Include(c => c.Friends).FirstOrDefault(customer => customer.Id == customerId) ?? throw new NotFoundException($"Customer with ID {customerId} was not found!");
            foreach (var friend in customer.Friends)
            {
                friendIds.Add(friend.Id);
            }
            return friendIds;
        }
    }
}
