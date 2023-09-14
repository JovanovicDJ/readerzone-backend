using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace readerzone_api.Models
{
    public abstract class Post
    {
        public int Id { get; set; }
        public DateTime PostingTime { get; set; }        
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public Post()
        {
            
        }

        public Post(DateTime postingTime)
        {
            PostingTime = postingTime;            
        }
    }
}
