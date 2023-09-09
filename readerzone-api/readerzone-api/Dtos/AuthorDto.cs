using System.ComponentModel.DataAnnotations;

namespace readerzone_api.Dtos
{
    public class AuthorDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Surname { get; set; } = string.Empty;
    }
}
