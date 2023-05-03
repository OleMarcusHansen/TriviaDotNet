namespace HIOF.Net.V2023.DatabaseService.Model.V1
{
    public class PostHighScore
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public int Correct { get; set; }
        public int Wrong { get; set; }

        public Data.UserData User { get; set; }

        public PostHighScore(Guid id, string category, int correct, int wrong, Data.UserData user)
        {
            Id = id;
            Category = category;
            Correct = correct;
            Wrong = wrong;
            User = user;
        }
    }
}
