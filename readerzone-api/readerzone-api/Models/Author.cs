using System.Text.Json.Serialization;

namespace readerzone_api.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
