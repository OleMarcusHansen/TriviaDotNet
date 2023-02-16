using GetTrivia.Intregations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

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
            var jsonFile = await client.GetStringAsync(jsonFileUrl);

            return jsonFile;


            // Tried to implement apiClient Intregation, does not work :/ - Radin Morik
            /*
            public async Task <string> exportJSON(string category = "history", int numbersofQuestions = 1, string difficulty = "easy") {
                var apiClient = new TriviaAPIClient();

                var json = await apiClient.TriviaCa(category = "history", numbersofQuestions = 1,  difficulty = "easy");

                return json;
            } */



        }
    }
}