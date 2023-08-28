using System.ComponentModel.DataAnnotations;

namespace readerzone_api.Dtos
{
    public class UpdateCustomerDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Surname { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Street { get; set; } = string.Empty;        
        public string Number { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
