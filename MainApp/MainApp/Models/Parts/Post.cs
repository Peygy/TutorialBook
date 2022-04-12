using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Отсутвует содержание статьи")]
        public string Content { get; set; }


        public int SubchapterId { get; set; }
        public Subchapter Subchapter { get; set; }
    }
}
