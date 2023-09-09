using System.ComponentModel.DataAnnotations;

namespace readerzone_api.Dtos
{
    public class PublisherDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Street { get; set; } = string.Empty;
        [Required]
        public string Number { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string Established { get; set; } = string.Empty;

    }
}
