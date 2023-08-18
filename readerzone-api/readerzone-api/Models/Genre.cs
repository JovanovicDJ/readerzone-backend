using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Text.Json.Serialization;

namespace readerzone_api.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();

        public Genre(string name)
        {
            Name = name;
        }
    }
}
