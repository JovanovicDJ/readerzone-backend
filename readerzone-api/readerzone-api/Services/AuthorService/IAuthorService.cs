﻿using readerzone_api.Models;

namespace readerzone_api.Services.AuthorService
{
    public interface IAuthorService
    {
        public Author GetAuthorById(int id);

    }
}
