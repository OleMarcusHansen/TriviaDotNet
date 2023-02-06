using static System.Net.WebRequestMethods;

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

            var jsonQnA = client.GetStringAsync(url).Result;

            Console.WriteLine(jsonQnA);
            Console.ReadLine();

        }
    }
    struct answer
    {
        public string text;
    }
    struct categori
    {
        public string text;
    }
    struct question
    {
        public string text;
        public answer correct;
        public list<answer> wrong;
        public categori categori;
    }
    struct quiz
    {
        public list<question> questions;
        public list<categori> categori;
        public int number;
    }

}