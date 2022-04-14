using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainApp.Models
{
    // User model
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указан логин!")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Не указан пароль!")]
        public string? Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string? ConfirmPassword { get; set; }
        public string Role { get; set; } = null!;
    }
}
