using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace readerzone_api.Models
{
    public class Review : Post
    {                
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Rating { get; set; }
        [ForeignKey("PurchasedBook")]
        public int PurchasedBookId { get; set; }
        [JsonIgnore]
        public PurchasedBook PurchasedBook { get; set; } = null!;        

        public Review(DateTime postingTime, string title, string text, int rating) : base(postingTime)
        {
            Title = title;
            Text = text;
            Rating = rating;
        }

        public Review()
        {

        }
    }
}
