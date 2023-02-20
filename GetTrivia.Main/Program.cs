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

            var jsonQnA = client.GetFromJsonAsync<quest[]>(url).Result;


            // Limited to 100 questions
            for (int i = 0; i < jsonQnA.Length; i++)
            {
            Console.WriteLine("Press enter to get next question");
            var answerInput = Console.ReadLine();
            Console.WriteLine(jsonQnA[i].category);
            Console.WriteLine(jsonQnA[i].question);
            Console.WriteLine(jsonQnA[i].correctAnswer);
            Console.ReadLine();
         
             //       if (answerInput == " " ){
                /*
                else if (i == jsonQnA.Length)
                {
                    Console.WriteLine("Thank you for playing, closing now");
                }
                */
            }

            //JObject jobject = JObject.Parse(jsonQnA);
            //var q = JsonConvert.DeserializeObject<quiz>(jsonQnA);

            //Console.WriteLine(q)
            //Console.ReadLine();
        }
    }
    class quiz
    {
        public quest[] QuestionList { get; set;}
    }
    class quest
    {
        public string category { get; set; }
        //public string id { get; set; }
        public string correctAnswer { get; set; }
        public string[] incorrectAnswers { get; set; }
        public string question { get; set; }
        public string[] tags { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string[] regions { get; set; }
        public bool isNiche { get; set; }
    }
    struct categori
    {
        public string text;
    }
    /*struct question
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