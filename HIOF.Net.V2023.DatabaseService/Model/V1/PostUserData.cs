namespace HIOF.Net.V2023.DatabaseService.Model.V1
{
    public class PostUserData
    {
        public Guid Id { get; set; }
        public int Correct { get; set; }
        public int Wrong { get; set; }

        public PostUserData(Guid id, int correct, int wrong)
        {
            Id = id;
            Correct = correct;
            Wrong = wrong;
        }
    }
}
