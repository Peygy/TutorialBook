namespace MainApp.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int SubsectionId { get; set; }
        public Subsection Subsection { get; set; }
    }
}
