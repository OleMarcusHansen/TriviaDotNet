using GetTrivia.ConsoleService.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using Grpc.Core;
using Grpc.Net.Client;
using System.Text.Json.Serialization;

namespace GetTrivia.ConsoleService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string[] catergoryList = { "Arts & Literature", "Film & TV", "Food & Drink", "General Knowledge", "Geography", "History", "Music", "Science", "Society & Culture", "Sport & Leisure"};

            Console.WriteLine("Hello, World!");

            var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7043/notificationHub?user=00000000-0000-0000-0000-000000000001")
                .Build();

            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                await hubConnection.StartAsync();
            };

            // Define a method for receiving hub messages
            hubConnection.On<string>("Notify", (message) =>
            {
                Console.WriteLine($"Received notification: {message}");
            });

            hubConnection.StartAsync();


            Console.WriteLine("Tast inn kategory: ");
            string? pickCat = Console.ReadLine();

            var url = $"https://localhost:7043/notify?user=00000000-0000-0000-0000-000000000001&message=Du valgte {pickCat} ja :J";
            using var client = new HttpClient();
            client.GetStringAsync(url);

            Console.WriteLine("Tast inn antall spørsmål: ");
            string? numbers = Console.ReadLine();

            Console.WriteLine("Tast inn vanskeligsgraden: ");
            string? difficulty = Console.ReadLine();

            // GRPC Connection
            using var channel = GrpcChannel.ForAddress("https://localhost:7107");
            var clientGrpc = new TriviaService.TriviaServiceClient(channel);
            var reply = clientGrpc.GetTrivia(new GetTriviaRequest { Category = pickCat, Difficulty = difficulty, NumberOfQuestions = int.Parse(numbers) });
            //Console.WriteLine(reply.JsonData);


            
            //https://localhost:7107/api/1.0/GetTrivia/TriviaCa?category=history&numbersofQuestions=1&difficulty=easy

            //var url = $"https://localhost:7107/api/1.0/GetTrivia/TriviaCa?category={pickCat}&numbersofQuestions={numbers}&difficulty={difficulty}";
            
            using var clientHttp = new HttpClient();
            url = $"https://localhost:7107/api/1.0/GetTrivia/TriviaCa?category={pickCat}&numbersofQuestions={numbers}&difficulty={difficulty}";

            Quest[] jsonQnA = Array.Empty<Quest>();

            try
            {
                jsonQnA = JsonConvert.DeserializeObject<Quest[]>(reply.JsonData);
                //jsonQnA = clientHttp.GetFromJsonAsync<Quest[]>(url).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            int correct = 0;
            int wrong = 0;

            foreach (Quest jsonQn in jsonQnA)
            {
                //Console.WriteLine("Press enter to get next question \n");
                Console.WriteLine(jsonQn.Question);
                Console.WriteLine("Alternatives: ");
                string[] allAnswers = new string[jsonQn.IncorrectAnswers.Count + 1];
                Array.Copy(jsonQn.IncorrectAnswers.ToArray(), allAnswers, jsonQn.IncorrectAnswers.Count);
                allAnswers[^1] = jsonQn.CorrectAnswer;
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
            var test = clientHttp.PutAsync(url, null).Result;

            Console.WriteLine(test.Content.ReadAsStringAsync().Result);

            //url = $"https://localhost:7160/api/1.0/HighScore/get/00000000-0000-0000-0000-000000000001/history";
            url = $"https://localhost:7160/api/1.0/HighScore/compareExisting?id=00000000-0000-0000-0000-000000000001&category={pickCat}&correct={correct}&wrong={wrong}";
            //var jsonHS = clientHttp.GetFromJsonAsync<HighScore>(url).Result;
            string jsonHS = clientHttp.GetStringAsync(url).Result;
            JObject json = JObject.Parse(jsonHS);
            string value = json["value"].ToString();
            HighScore? highScore = JsonConvert.DeserializeObject<HighScore>(value);

            if (highScore.Correct == 0 && highScore.Wrong == 0)
            {
                url = $"https://localhost:7160/api/1.0/HighScore/create?id=00000000-0000-0000-0000-000000000001&category={pickCat}&correct={correct}&wrong={wrong}";
                var newHighScore = clientHttp.PostAsync(url, null).Result;
            }
            else if (highScore.Correct == correct && highScore.Wrong == wrong)
            {
                url = $"https://localhost:7160/api/1.0/HighScore/update?id=00000000-0000-0000-0000-000000000001&category={pickCat}&correct={correct}&wrong={wrong}";
                var newHighScore = clientHttp.PutAsync(url, null).Result;
            }

            Console.WriteLine("Thank you for playing, press Enter to close");
            Console.ReadLine();


            Console.WriteLine("test push");
        }
    }
}