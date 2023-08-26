﻿using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using readerzone_api.Data;
using readerzone_api.Dtos;
using readerzone_api.Exceptions;
using readerzone_api.Models;
using readerzone_api.Services.BookService;
using readerzone_api.Services.CustomerService;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly ReaderZoneContext _readerZoneContext;
        private readonly IBookService _bookService;
        private readonly ICustomerService _customerService;

        public OrderService(ReaderZoneContext readerZoneContext, IBookService bookService, ICustomerService customerService)
        {
            _readerZoneContext = readerZoneContext;
            _bookService = bookService;
            _customerService = customerService;
        }

        public void AddOrder(OrderDto orderDto)
        {
            var order = new Order()
            {
                Name = orderDto.Name,
                Surname = orderDto.Surname,
                Email = orderDto.Email,
                PhoneNumber = orderDto.PhoneNumber,
                Street = orderDto.Street,
                Number = orderDto.Number,
                City = orderDto.City,
                PostalCode = orderDto.PostalCode,
                Country = orderDto.Country,
                Price = orderDto.Price,
                OrderStatus = OrderStatus.Pending,
                Books = GetBooksByIsbn(orderDto.Books)
            };
            _readerZoneContext.Orders.Add(order);
            _readerZoneContext.SaveChanges();
        }

        public void CompleteOrder(int id)
        {
            var order = _readerZoneContext.Orders
                               .Include(o => o.Books)                              
                               .Where(o => o.Id == id && o.OrderStatus.Equals(OrderStatus.Pending))
                               .FirstOrDefault();
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {id} was not found or it was already completed!");
            }

            order.OrderStatus = OrderStatus.Completed;
            _readerZoneContext.SaveChanges();
            _customerService.AddPurchasedBooks(order.Email, order.Price, order.Books);
        }

        private ICollection<Book> GetBooksByIsbn(List<string> isbns)
        {
            ICollection<Book> books = new List<Book>();
            foreach (var isbn in isbns)
            {
                books.Add(_bookService.GetBook(isbn));
            }
            return books;
        }
    }
}