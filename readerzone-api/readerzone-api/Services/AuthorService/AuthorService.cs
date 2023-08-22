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
    }
}
