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
        public async Task<Result<UserData>> CreateUserData(PostUserData userDataPost)
        {
            var dbContext = new Data.UserDataDbContext();

            var userData = new Data.UserData
            {
                Id = Guid.NewGuid(),
                Correct = userDataPost.Correct,
                Wrong = userDataPost.Wrong,
            };

            dbContext.UserDatas.Add(userData);
            await dbContext.SaveChangesAsync();

            var result = new Result<UserData>(new UserData
            {
                Id = userData.Id,
                Correct = userDataPost.Correct,
                Wrong = userDataPost.Wrong
            });

            return result;
        }
    }
}