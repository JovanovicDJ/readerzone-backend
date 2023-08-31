using System.ComponentModel.DataAnnotations;

namespace readerzone_api.Dtos
{
    public class CommentPostDto
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public int PostId { get; set; }
    }
}
