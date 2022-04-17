using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Section -> Subsection -> Chapter -> Subchapter -> POST

    // Model for Post
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string? Title { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage = "Отсутвует содержание поста")]
        public string? Content { get; set; }

        public int SubchapterId { get; set; }
        public Subchapter? Subchapter { get; set; }
    }
}
