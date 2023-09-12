using System.ComponentModel.DataAnnotations;

namespace readerzone_api.Dtos
{
    public class BookDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public List<int> AuthorIds { get; set; } = new List<int>();
        [Required]
        public string ISBN { get; set; } = string.Empty;
        [Required]
        public string PublishingDate { get; set; } = string.Empty;
        [Required]
        public List<string> Genres { get; set; } = new List<string>();
        [Required]
        public int Stocks { get; set; }
        [Required]
        public int Pages { get; set; }
        [Required]
        public string Language { get; set; } = string.Empty;
        [Required]
        public double Weight { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int PublisherId { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public int Discount { get; set; }
    }
}
