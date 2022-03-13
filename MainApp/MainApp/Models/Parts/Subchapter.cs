namespace MainApp.Models
{
    public class Subchapter
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }
    }
}
