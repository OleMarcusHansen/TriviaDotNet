using HIOF.Net.V2023.DatabaseService.Model.V1;
using Microsoft.AspNetCore.Mvc;

namespace HIOF.Net.V2023.DatabaseService.Controllers.V1
{
    [ApiController]
    [Route("V1/UserData")]
    public class DatabaseController : ControllerBase
    {

        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(ILogger<DatabaseController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IEnumerable<UserData> Get()
        {
            return new[]
            {
                new UserData
                {
                    Id = Guid.NewGuid(),
                    Correct = 0,
                    Wrong = 0,
                },
                new UserData
                {
                    Id = Guid.NewGuid(),
                    Correct = 0,
                    Wrong = 0,
                }
            };
        }

        [HttpPost]
        public UserData CreateUserData(PostUserData userDataPost)
        {
            var result = new Result<UserData>();

            result.Value

            return new UserData
            {
                Correct = userDataPost.Correct,
                Wrong = userDataPost.Wrong
            };
        }
    }
}