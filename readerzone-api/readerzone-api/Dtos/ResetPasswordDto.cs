using System.ComponentModel.DataAnnotations;

namespace readerzone_api.Dtos
{
    public class ResetPasswordDto   
    {
        [Required, MinLength(8), MaxLength(30)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
