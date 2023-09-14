using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Models
{
    public class PurchasedBook
    {        
        public int Id { get; set; }
        public BookStatus BookStatus { get; set; }
        public Book Book { get; set; } = null!;
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; } = null!;
        public Review Review { get; set; } = null!;        

    }
}
