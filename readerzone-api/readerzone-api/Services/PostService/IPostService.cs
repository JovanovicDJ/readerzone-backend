﻿using readerzone_api.Models;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.PostService
{
    public interface IPostService
    {
        public void GeneratePurchasedBookPost(Customer customer, ICollection<Book> books);

        public void GenerateReview(Customer customer, PurchasedBook book, string title, string text, int rating);

        public void GenerateChangedBookStatusPost(Customer customer, PurchasedBook book, BookStatus status);
    }
}