using System.Text.Json.Serialization;

namespace readerzone_api.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Address Address { get; set; } = null!;
        public DateTime Established { get; set; }
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
