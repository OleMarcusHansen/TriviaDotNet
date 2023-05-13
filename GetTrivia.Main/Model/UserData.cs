namespace GetTrivia.ConsoleService.Model
{
    public class UserData
    {
        public Guid Id { get; set; }
        public int Correct { get; set; }
        public int Wrong { get; set; }
    }
}
