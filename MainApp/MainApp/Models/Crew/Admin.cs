namespace MainApp.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int StatusCode { get; set; } = 01;
    }
}
