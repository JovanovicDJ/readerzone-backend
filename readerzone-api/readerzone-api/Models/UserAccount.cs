using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;       
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public byte[] PasswordHash { get; set; } = new byte[32];
        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;
        public Role Role { get; set; }
        public bool Active { get; set; } = false;
        public bool Blocked { get; set; } = false;

        public UserAccount(string username, string email, Role role, bool active, bool blocked)
        {
            Username = username;
            Email = email;            
            Role = role;
            Active = active;
            Blocked = blocked;
        }
    }
}
