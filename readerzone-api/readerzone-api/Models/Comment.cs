using System.ComponentModel.DataAnnotations.Schema;

namespace readerzone_api.Models
{
    public class Comment 
    {
        public int Id { get; set; }
        public DateTime PostingTime { get; set; }
        public int Likes { get; set; }
        public string Text { get; set; } = string.Empty;
        public Post Post { get; set; } = null!;
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }
}
