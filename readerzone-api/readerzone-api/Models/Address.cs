namespace readerzone_api.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string? Number { get; set; }
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public Address(string street, string number, string city, string postalCode, string country)
        {
            Street = street;
            Number = number;
            City = city;
            PostalCode = postalCode;
            Country = country;
        }

        public Address(string street, string city, string postalCode, string country)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
        }
    }
}
