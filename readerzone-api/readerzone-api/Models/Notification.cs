using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [ForeignKey("ToCustomer")]
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer ToCustomer { get; set; } = null!;
        public string FromCustomerName { get; set; } = string.Empty;
        public string FromCustomerSurname { get; set; } = string.Empty;
        public string FromCustomerUsername { get; set; } = string.Empty;
        public int FromCustomerId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime SendingTime { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool Deleted { get; set; } = false;

    }
}
