using System.ComponentModel.DataAnnotations;

namespace MainApp.Tests.Models.Crew
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Login { get; set; }
        [Required]
        public string? Password { get; set; }
        // Role for authorization
        public string Role { get; set; } = "admin";
    }
}