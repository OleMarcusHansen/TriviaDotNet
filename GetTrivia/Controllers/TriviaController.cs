using Microsoft.AspNetCore.Mvc;

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
        public async Task<string> TriviaCa(string category="history", int numbersofQuestions=1, string difficulty="easy")
        {
            using var client = new HttpClient();

            //Er kategori og difficulty er valgfri, kan vi bruke ternery
            var jsonFileUrl = $"https://the-trivia-api.com/api/questions?categories={category}&limit={numbersofQuestions}&difficulty={difficulty}";
            var jsonFile = await client.GetStringAsync(jsonFileUrl);

            

            return jsonFile;
        }
    }
    /*class question
    {
        public string category { get; set; }
        public string id { get; set; }
        public string correctAnswer { get; set; }
        public string[] incorrectAnswers { get; set; }
        public string question { get; set; }
        public string[] tags { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string[] regions { get; set; }
        public bool isNiche { get; set; }
    }*/
}