using System.ComponentModel.DataAnnotations;

namespace MainApp.Tests.Models.Parts
{
    public class Post : GeneralPart
    {
        [Required]
        public string? Content { get; set; }
    }
}
