using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace readerzone_api.Models
{
    public class Comment 
    {
        public int Id { get; set; }
        public DateTime PostingTime { get; set; }        
        public string Text { get; set; } = string.Empty;
        [JsonIgnore]
        public Post Post { get; set; } = null!;
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; } = null!;
        public bool Deleted { get; set; } = false;
    }
}
