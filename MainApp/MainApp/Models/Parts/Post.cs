using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Section -> Subsection -> Chapter -> Subchapter -> POST

    // Model for Post
    public class Post : GeneralPart
    {
        [Required(ErrorMessage = "Отсутвует содержание поста")]
        public string? Content { get; set; }
    }
}
