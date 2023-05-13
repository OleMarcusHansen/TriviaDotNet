namespace GetTrivia.ConsoleService.Model
{
    public class HighScore
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public int Correct { get; set; }
        public int Wrong { get; set; }
    }
}
