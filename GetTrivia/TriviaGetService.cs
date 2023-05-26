using GetTrivia.Controllers.V1;
using GetTrivia.GetTriviaService.Model.V1;
using Grpc.Core;
using System.Text.Json;

namespace HIOF.Net.V2023.GetTriviaService
{
    public class TriviaGetService : TriviaService.TriviaServiceBase
    {
        private readonly ILogger<TriviaGetService> _logger;

        public TriviaGetService(ILogger<TriviaGetService> logger)
        {
            _logger = logger;
        }

        public async override Task<GetTriviaResponse> GetTrivia(GetTriviaRequest request, ServerCallContext context)
        {
            using var client = new HttpClient();

            //Er kategori og difficulty er valgfri, kan vi bruke ternery
            _logger.LogInformation("Getting questions from API");
            var jsonFileUrl = $"https://the-trivia-api.com/api/questions?categories={request.Category}&limit={request.NumberOfQuestions}&difficulty={request.Difficulty}";
            var questions = await client.GetFromJsonAsync<Quest[]>(jsonFileUrl);

            if (request.Category == null)
            {
                _logger.LogError("Invalid category name given");
            }

            if (request.NumberOfQuestions >= 0)
            {
                _logger.LogError("Invalid Number of questions");
            }

            if (request.Difficulty == null)
            {
                _logger.LogError("Invalid difficulity given");

            }

            var response = new GetTriviaResponse();
            response.JsonData = JsonSerializer.Serialize(questions);

            return response;
        }

        public async override Task<GetCategoriesResponse> GetCategories(NoRequest request, ServerCallContext context)
        {
            using var client = new HttpClient();

            //Er kategori og difficulty er valgfri, kan vi bruke ternery
            _logger.LogInformation("Getting categories from API");
            var jsonFileUrl = $"https://the-trivia-api.com/api/categories";
            var questions = await client.GetFromJsonAsync<Dictionary<string, List<string>>>(jsonFileUrl);

            var response = new GetCategoriesResponse();
            for (int i = 0; i < questions.Count; i++)
            {
                response.Categories.Add(questions.Values.ElementAt(i).ElementAt(0));
            }

            return response;
        }
    }
}
