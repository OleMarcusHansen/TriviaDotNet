using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;
using Newtonsoft;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace GetTrivia.Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Console.WriteLine("Tast inn kategory: ");
            string? pickCat = Console.ReadLine();

            Console.WriteLine("Tast inn antall spørsmål: ");
            string? numbers = Console.ReadLine();

            Console.WriteLine("Tast inn vanskeligsgraden: ");
            string? difficulty = Console.ReadLine();

            //https://localhost:7107/Trivia/TriviaCa?category=history&numbersofQuestions=1&difficulty=easy

            var url = $"https://localhost:7107/Trivia/TriviaCa?category={pickCat}&numbersofQuestions={numbers}&difficulty={difficulty}";

            using var client = new HttpClient();

            var jsonQnA = client.GetFromJsonAsync<Quest[]>(url).Result;

            Console.WriteLine(jsonQnA[0].Question);
            Console.WriteLine(jsonQnA[0].CorrectAnswer);
            Console.ReadLine();
            //JObject jobject = JObject.Parse(jsonQnA);
            //var q = JsonConvert.DeserializeObject<quiz>(jsonQnA);

            //Console.WriteLine(q);
            //Console.ReadLine();
        }
    }
    class Quest
    {
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }
        public string Question { get; set; }
    }
    /*struct categori
    {
        public string text;
    }
    struct question
    {
        public string text;
        public answer correct;
        public List<answer> wrong;
        public categori categori;
    }
    struct quiz
    {
        public List<question> questions;
        public List<categori> categori;
        public int number;
    }*/
}