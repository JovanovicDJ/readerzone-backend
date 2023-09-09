using readerzone_api.Models;

namespace readerzone_api.Services.AuthorService
{
    public interface IAuthorService
    {        
        public Author GetAuthorById(int id);
        public List<Author> GetAuthors();
        public Author AddAuthor(string name, string surname);
    }
}
