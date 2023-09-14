namespace readerzone_api.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public DateTime PostingTime { get; set; }        
        public int CustomerId { get; set; }
        public string CustomerUsername { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerSurname { get; set; } = string.Empty;
        public string CustomerImageUrl { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string Text { get; set; } = string.Empty;
        public int? Rating { get; set; }
        public int? PurchasedBookId { get; set; }
        public string? Isbn { get; set; } = string.Empty;
        public string? BookTitle { get; set; } = string.Empty;
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; } = string.Empty;
        public string? AuthorSurname { get; set; } = string.Empty;
        public string? BookImageUrl { get; set; } = string.Empty;
        public ICollection<CommentDto>? Comments { get; set; }

    }
}
