using readerzone_api.Models;

namespace readerzone_api.Services.GenreService
{
    public interface IGenreService
    {
        public Genre GetGenreByName(string name);

    }
}
