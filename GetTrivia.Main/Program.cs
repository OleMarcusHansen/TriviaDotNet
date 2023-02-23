using GetTrivia.Main.Model;
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

            //https://localhost:7107/api/1.0/GetTrivia/TriviaCa?category=history&numbersofQuestions=1&difficulty=easy

            var url = $"https://localhost:7107/api/1.0/GetTrivia/TriviaCa?category={pickCat}&numbersofQuestions={numbers}&difficulty={difficulty}";
            
            using var client = new HttpClient();

            var jsonQnA = client.GetFromJsonAsync<Quest[]>(url).Result;
            

            // Limited to 100 questions
            foreach (var jsonQn in jsonQnA)
            {
                //Console.WriteLine("Press enter to get next question \n");
                Console.WriteLine(jsonQn.Question);
                Console.WriteLine("Alternatives: ");
                string[] allAnswers = new string[jsonQn.IncorrectAnswers.Length + 1];
                Array.Copy(jsonQn.IncorrectAnswers, allAnswers, jsonQn.IncorrectAnswers.Length);
                allAnswers[allAnswers.Length - 1] = jsonQn.CorrectAnswer;
                Array.Sort(allAnswers);
                for (int i = 0; i < allAnswers.Length; i++)
                {
                    Console.WriteLine((i+1) + ". " + allAnswers[i]);
                }
                Console.WriteLine("");
                Console.WriteLine("Skriv ditt svar:");
                var answerInput = Console.ReadLine();
                Console.WriteLine("");
                if (allAnswers[int.Parse(answerInput)-1] == jsonQn.CorrectAnswer)
                {
                    Console.WriteLine("Correct!");
                }
                else
                {
                    Console.WriteLine("Wrong, the answer was: " + jsonQn.CorrectAnswer);
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine("Thank you for playing, press Enter to close");
            Console.ReadLine();

        }
    }
}