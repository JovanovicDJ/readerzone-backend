
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;        
        public string Email { get; set; } = string.Empty;        
        public string PhoneNumber { get; set; } = string.Empty;        
        public string Street { get; set; } = string.Empty;        
        public string Number { get; set; } = string.Empty;        
        public string City { get; set; } = string.Empty;       
        public string PostalCode { get; set; } = string.Empty;        
        public string Country { get; set; } = string.Empty;        
        public double Price { get; set; }
        public OrderStatus OrderStatus { get; set; }        
        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
