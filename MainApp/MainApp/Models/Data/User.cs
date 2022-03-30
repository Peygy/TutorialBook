using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class User : IdentityUser
    {
        [Key]
        public int ID { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
