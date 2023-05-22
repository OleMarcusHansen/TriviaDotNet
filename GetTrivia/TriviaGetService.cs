using Grpc.Core;

namespace HIOF.Net.V2023.GetTriviaService
{
    public class TriviaGetService : TriviaService.TriviaServiceBase
    {
        public override Task<GetTriviaResponse> GetTrivia(GetTriviaRequest request, ServerCallContext context)
        {
            var response = new GetTriviaResponse();
            response.JsonData = "test grpc";

            // Return the response to the caller
            return Task.FromResult(response);
        }
    }
}
