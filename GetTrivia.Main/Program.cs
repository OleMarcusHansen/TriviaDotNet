using GetTrivia.ConsoleService.Model;
using System.Net.Http.Json;

namespace GetTrivia.ConsoleService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] catergoryList = { "Arts & Literature", "Film & TV", "Food & Drink", "General Knowledge", 
            "Geography", "History", "Music", "Science", "Society & Culture", "Sport & Leisure"};

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

            int correct = 0;
            int wrong = 0;

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
                    correct++;
                }
                else
                {
                    Console.WriteLine("Wrong, the answer was: " + jsonQn.CorrectAnswer);
                    wrong++;
                }
                Console.WriteLine("\n");
            }

            //url = $"https://localhost:7160/api/1.0/UserData/00000000-0000-0000-0000-000000000001";
            url = $"https://localhost:7160/api/1.0/UserData/update?id=00000000-0000-0000-0000-000000000001&correct={correct}&wrong={wrong}";
            var test = client.PutAsync(url, null).Result;

            Console.WriteLine(test);

            Console.WriteLine("Thank you for playing, press Enter to close");
            Console.ReadLine();


            Console.WriteLine("test push");
        }
    }
}