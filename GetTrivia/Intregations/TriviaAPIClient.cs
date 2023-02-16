namespace GetTrivia.Intregations
{
    public class TriviaAPIClient
    {
        public async Task<string> TriviaCa(string category = "history", int numbersofQuestions = 1, string difficulty = "easy")
        {
            using var client = new HttpClient();

            //Er kategori og difficulty er valgfri, kan vi bruke ternery
            var jsonFileUrl = $"https://localhost:7107/Trivia/TriviaCa?category={category}&numbersofQuestions={numbersofQuestions}&difficulty={difficulty}";
            var jsonFile = await client.GetStringAsync(jsonFileUrl);

            return jsonFile;
        }

    }
}
