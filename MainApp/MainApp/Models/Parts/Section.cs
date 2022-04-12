using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        private const string TableName = "section";
    }
}
