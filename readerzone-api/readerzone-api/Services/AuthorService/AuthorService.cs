using readerzone_api.Data;
using readerzone_api.Exceptions;
using readerzone_api.Models;

namespace readerzone_api.Services.AuthorService
{
    public class AuthorService : IAuthorService
    {
        private readonly ReaderZoneContext _readerZoneContext;

        public AuthorService(ReaderZoneContext readerZoneContext)
        {
            _readerZoneContext = readerZoneContext;
        }        

        public Author GetAuthorById(int id)
        {
            var author = _readerZoneContext.Authors.FirstOrDefault(a => a.Id == id);
            return author == null ? throw new NotFoundException($"Author with ID {id} has not been found.") : author;
        }

        public List<Author> GetAuthors()
        {
            var authors = _readerZoneContext.Authors.ToList();
            return authors == null ? throw new NotFoundException("Authors not found") : authors;
        }

        public Author AddAuthor(string name, string surname)
        {
            var author = _readerZoneContext.Authors.FirstOrDefault(a => a.Name.Equals(name) && a.Surname.Equals(surname));
            if (author != null)
            {
                throw new NotCreatedException($"Author {name} {surname} already exists");
            }
            author = new Author()
            {
                Name = name,
                Surname = surname
            };
            _readerZoneContext.Authors.Add(author);
            _readerZoneContext.SaveChanges();
            return author;
        }
    }
}
