using readerzone_api.Dtos;
using System.Text.Json.Serialization;

namespace readerzone_api.Models
{
    public class Book
    {        
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime PublishingDate { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public int Stocks { get; set; }
        public int Pages { get; set; }
        public string Language { get; set; } = string.Empty;
        public double Weight { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Price { get; set; }
        public ICollection<Author> Authors { get; set; } = new List<Author>();
        public Publisher Publisher { get; set; } = null!;
        [JsonIgnore]
        public ICollection<PurchasedBook> PurchasedBooks { get; set; } = new List<PurchasedBook>();
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public string ImageUrl { get; set; } = string.Empty;
        public double AverageRating { get; set; } = 3.5;
        public int Discount { get; set; } = 0;               
    }
}
