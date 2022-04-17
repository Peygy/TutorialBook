using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        // Role for authorization 
        public string? Role { get; set; } = "";
    }
}
