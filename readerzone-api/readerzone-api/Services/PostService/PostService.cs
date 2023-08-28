using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Models;
using readerzone_api.Services.CustomerService;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly ReaderZoneContext _readerZoneContext;        

        public PostService(ReaderZoneContext readerZoneContext)
        {
            _readerZoneContext = readerZoneContext;            
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
                //var text = $"{customer.Name} {customer.Surname} (@{customer.UserAccount.Username}) " +
                //           $"wants to read '{book.Title}' by {book.Authors.First().Name} {book.Authors.First().Surname}.";
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
    }
}
