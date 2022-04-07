using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class Subchapter
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        private const string TableName = "subchapter";

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
    }
}
