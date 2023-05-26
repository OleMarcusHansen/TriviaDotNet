namespace HIOF.Net.V2023.WebApp.Model.V1
{
    public class Quest
    {
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }
        public string Question { get; set; }
    }
}
