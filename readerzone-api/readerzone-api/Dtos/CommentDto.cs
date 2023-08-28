namespace readerzone_api.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime PostingTime { get; set; }
        public int Likes { get; set; }
        public string Text { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerUsername { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerSurname { get; set; } = string.Empty;
        public string CustomerImageUrl { get; set; } = string.Empty;

    }
}
