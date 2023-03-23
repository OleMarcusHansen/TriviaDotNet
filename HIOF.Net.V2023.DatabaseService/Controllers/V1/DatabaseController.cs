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
        public Result<IEnumerable<UserData>> Get()
        {
            var dummyData = new[]
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

            return new Result<IEnumerable<UserData>>(dummyData);
        }

        [HttpPost]
        public Result<UserData> CreateUserData(PostUserData userDataPost)
        {
            var result = new Result<UserData>(new UserData
            {
                Correct = userDataPost.Correct,
                Wrong = userDataPost.Wrong
            });

            return result;
        }
    }
}