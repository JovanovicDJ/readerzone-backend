using System.ComponentModel.DataAnnotations;

namespace readerzone_api.Dtos
{
    public class ReviewDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public int Rating { get; set; }
        [Required]
        public int PurchasedBookId { get; set; }

    }
}
