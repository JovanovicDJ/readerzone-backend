using static readerzone_api.Enums.Enums;

namespace readerzone_api.Dtos
{
    public class BookData
    {
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public BookStatus BookStatus { get; set; }
        public string Author { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
