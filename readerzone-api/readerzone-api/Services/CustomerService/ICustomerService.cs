﻿using readerzone_api.Models;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.CustomerService
{
    public interface ICustomerService
    {
        public Customer GetCustomerByEmail(string email);
        public Customer GetCustomerWithPassword(string email);
        public void AddPurchasedBooks(string email, double price, ICollection<Book> books);
        public List<PurchasedBook> GetPurchasedBooksByCustomerId(int customerId, BookStatus status);
        public void UpdatePurchasedBookStatus(int purchasedBookId, BookStatus status);

    }
}
