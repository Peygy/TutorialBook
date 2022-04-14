using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Admin model
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указан логин")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        public string? Password { get; set; }
        public string Role { get; set; } = null!;
    }
}
