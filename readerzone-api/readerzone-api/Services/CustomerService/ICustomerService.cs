using readerzone_api.Dtos;
using readerzone_api.Models;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.CustomerService
{
    public interface ICustomerService
    {
        public Customer GetCustomerByEmail(string email);
        public Customer GetCustomerWithPassword(string email);
        public Customer GetCustomerById(int id);
        public void AddPurchasedBooks(string email, double price, ICollection<Book> books);
        public List<PurchasedBook> GetPurchasedBooksByCustomerId(int customerId, BookStatus status);
        public void UpdatePurchasedBookStatus(int purchasedBookId, BookStatus status);
        public void AddReview(string title, string text, int rating, int purchasedBookId);
        public void UpdateCustomer(UpdateCustomerDto updateCustomerDto);
        public List<BookData> GetBooksDataByCustomerId(int id);
    }
}
