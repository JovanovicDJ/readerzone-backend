using readerzone_api.Data;
using readerzone_api.Exceptions;
using readerzone_api.Models;

namespace readerzone_api.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private readonly ReaderZoneContext _readerZoneContext;

        public GenreService(ReaderZoneContext readerZoneContext)
        {
            _readerZoneContext = readerZoneContext;
        }
        public Genre GetGenreByName(string name)
        {
            var genre = _readerZoneContext.Genres.FirstOrDefault(g => g.Name == name);
            return genre == null ? throw new NotFoundException($"Genre with name {name} has not been found.") : genre;
        }
    }
}
