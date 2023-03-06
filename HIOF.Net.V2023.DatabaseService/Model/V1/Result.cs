namespace HIOF.Net.V2023.DatabaseService.Model.V1
{
    public class Result<T>
    {
        public List<string> Errors { get; set; } = new List<string>();

        public bool HasErrors => Errors.Any();

        public T Value { get; set; }
    }
}
