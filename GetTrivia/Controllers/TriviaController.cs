using GetTrivia.Intregations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GetTrivia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TriviaController : ControllerBase
    {

        private readonly ILogger<TriviaController> _logger;

        public TriviaController(ILogger<TriviaController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Henter API-kategory
        /// </summary>
        /// <param name="category"></param>
        /// <param name="numbersofQuestions"></param>
        /// <param name="difficulty"></param>
        /// <returns></returns>

        [HttpGet("TriviaCa")]

        public async Task<string> TriviaCa(string category = "history", int numbersofQuestions = 1, string difficulty = "easy")
        {
            using var client = new HttpClient();

            //Er kategori og difficulty er valgfri, kan vi bruke ternery
            var jsonFileUrl = $"https://the-trivia-api.com/api/questions?categories={category}&limit={numbersofQuestions}&difficulty={difficulty}";
            var questions = await client.GetFromJsonAsync<Quest[]>(jsonFileUrl);

            return JsonSerializer.Serialize(questions);


            // Tried to implement apiClient Intregation, does not work :/ - Radin Morik
            /*
            public async Task <string> exportJSON(string category = "history", int numbersofQuestions = 1, string difficulty = "easy") {
                var apiClient = new TriviaAPIClient();

                var json = await apiClient.TriviaCa(category = "history", numbersofQuestions = 1,  difficulty = "easy");

                return json;
            } */



        }
    }
    class Quest
    {
        //public string category { get; set; }
        //public string id { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }
        public string Question { get; set; }
        //public string[] tags { get; set; }
        //public string Type { get; set; }
        //public string Difficulty { get; set; }
        //public string[] Regions { get; set; }
        //public bool IsNiche { get; set; }
    }
}