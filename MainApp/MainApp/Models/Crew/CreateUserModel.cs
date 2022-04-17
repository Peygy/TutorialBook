using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Model for user registration
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Не указан логин!")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Длина логина должна быть от 4 до 20 символов")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Не указан пароль!")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Длина пароля должна быть от 7 до 20 символов")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Не подтвержден пароль!")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string? ConfirmPassword { get; set; }
    }
}
