using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace readerzone_api.Models
{
    public class AutomaticPost : Post
    {        
        public string Text { get; set; } = string.Empty;               

        public AutomaticPost(DateTime postingTime, int likes, string text) : base(postingTime, likes)
        {
            Text = text;
        }
    }
}
