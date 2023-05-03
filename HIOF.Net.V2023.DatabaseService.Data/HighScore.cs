namespace HIOF.Net.V2023.DatabaseService.Data
{
    public class HighScore
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public int Correct { get; set; }
        public int Wrong { get; set; }

        public UserData User { get; set; }
    }
}
