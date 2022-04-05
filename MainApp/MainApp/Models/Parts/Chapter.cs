using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainApp.Models
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        private const string TableName = "chapter";

        public int SubsectionId { get; set; }
        public Subsection Subsection { get; set; }
    }
}
