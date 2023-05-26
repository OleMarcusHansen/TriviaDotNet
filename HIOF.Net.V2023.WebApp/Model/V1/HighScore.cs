namespace HIOF.Net.V2023.WebApp.Model.V1
{
    public class HighScore
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public int Correct { get; set; }
        public int Wrong { get; set; }
        public int Score { get; set; }
    }
}
