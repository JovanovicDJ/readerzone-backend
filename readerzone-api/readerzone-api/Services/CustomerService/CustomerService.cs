using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Exceptions;
using readerzone_api.Models;
using System.Linq.Expressions;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly ReaderZoneContext _readerZoneContext;

        public CustomerService(ReaderZoneContext readerZoneContext)
        {
            _readerZoneContext = readerZoneContext;
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

        public void AddPurchasedBooks(string email, double price, ICollection<Book> books)
        {
            var customer = GetCustomerWithPassword(email);
            foreach (var book in books)
            {
                var purchasedBook = new PurchasedBook()
                {
                    BookStatus = BookStatus.WantToRead,
                    Book = book,
                    Customer = customer,
                    FinalPrice = price
                };
                _readerZoneContext.PurchasedBooks.Add(purchasedBook);
                _readerZoneContext.SaveChanges();
            }
            AddPoints(customer, price);
        }

        public Customer GetCustomerWithPassword(string email)  // In function GetCustomerByEmail I need to keep password safe, here I don't to that
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

        private void AddPoints(Customer customer, double price)
        {
            customer.Points = customer.Points + price;
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
    }
}
