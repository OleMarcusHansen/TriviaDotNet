using GetTrivia.ConsoleService.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            Quest[] jsonQnA = new Quest[0];

            try
            {
                jsonQnA = client.GetFromJsonAsync<Quest[]>(url).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            int correct = 0;
            int wrong = 0;

            foreach (var jsonQn in jsonQnA)
            {
                //Console.WriteLine("Press enter to get next question \n");
                Console.WriteLine(jsonQn.Question);
                Console.WriteLine("Alternatives: ");
                string[] allAnswers = new string[jsonQn.IncorrectAnswers.Count + 1];
                Array.Copy(jsonQn.IncorrectAnswers.ToArray(), allAnswers, jsonQn.IncorrectAnswers.Count);
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

            Console.WriteLine(test.Content.ReadAsStringAsync().Result);

            //url = $"https://localhost:7160/api/1.0/HighScore/get/00000000-0000-0000-0000-000000000001/history";
            url = $"https://localhost:7160/api/1.0/HighScore/get/00000000-0000-0000-0000-000000000001/{pickCat}";
            //var jsonHS = client.GetFromJsonAsync<HighScore>(url).Result;
            var jsonHS = client.GetStringAsync(url).Result;
            var json = JObject.Parse(jsonHS);
            var value = json["value"].ToString();
            var earera = JsonConvert.DeserializeObject<HighScore>(value);

            if (jsonHS != null)
            {
                Console.WriteLine(earera.Id);
                Console.WriteLine(earera.Category);
                Console.WriteLine(earera.Correct);
                Console.WriteLine(earera.Wrong);
            }
            else
            {
                Console.WriteLine("null");
            }

            Console.WriteLine("Thank you for playing, press Enter to close");
            Console.ReadLine();


            Console.WriteLine("test push");
        }
    }
}