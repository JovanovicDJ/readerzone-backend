using readerzone_api.Data;
using readerzone_api.Models;
using readerzone_api.Services.CustomerService;

namespace readerzone_api.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly ReaderZoneContext _readerZoneContext;
        private readonly ICustomerService _customerService;

        public PostService(ReaderZoneContext readerZoneContext, ICustomerService customerService)
        {
            _readerZoneContext = readerZoneContext;
            _customerService = customerService;
        }

        public void GeneratePurchasedBookPost(string email, ICollection<Book> books)
        {
            var customer = _customerService.GetCustomerWithPassword(email);
            foreach (var book in books)
            {
                var text = $"{customer.Name} {customer.Surname} (@{customer.UserAccount.Username}) " +
                           $"wants to read '{book.Title}' by {book.Authors.First().Name} {book.Authors.First().Surname}.";
                var post = new AutomaticPost()
                {
                    PostingTime = DateTime.Now,
                    Likes = 0,
                    Customer = customer,
                    Text = text
                };
                _readerZoneContext.AutomaticPosts.Add(post);
                _readerZoneContext.SaveChanges();
            }
        }
    }
}
