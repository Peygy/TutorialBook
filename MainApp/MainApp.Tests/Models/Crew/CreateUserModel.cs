using System.ComponentModel.DataAnnotations;

namespace MainApp.Tests.Models.Crew
{
    public class CreateUserModel
    {
        [Required]
        public string? Login { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
