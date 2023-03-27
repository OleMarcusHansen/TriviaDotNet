using HIOF.Net.V2023.DatabaseService.Data;
using HIOF.Net.V2023.DatabaseService.Model.V1;
using Microsoft.AspNetCore.Mvc;

namespace HIOF.Net.V2023.DatabaseService.Controllers.V1
{
    [ApiController]
    [Route("V1/UserData")]
    public class DatabaseController : ControllerBase
    {

        private readonly ILogger<DatabaseController> _logger;
        private readonly UserDataDbContext _userDataDbContext;

        public DatabaseController(ILogger<DatabaseController> logger, UserDataDbContext userDataDbContext)
        {
            _logger = logger;
            _userDataDbContext = userDataDbContext;
        }

        [HttpGet("")]
        public Result<IEnumerable<Data.UserData>> Get()
        {
            var dummyData = new[]
            {
                new Data.UserData
                {
                    Id = Guid.NewGuid(),
                    Correct = 0,
                    Wrong = 0,
                },
                new Data.UserData
                {
                    Id = Guid.NewGuid(),
                    Correct = 0,
                    Wrong = 0,
                }
            };

            return new Result<IEnumerable<Data.UserData>>(dummyData);
        }

        [HttpPost]
        public async Task<Result<Data.UserData>> CreateUserData(PostUserData userDataPost)
        {

            var userData = new Data.UserData
            {
                Id = Guid.NewGuid(),
                Correct = userDataPost.Correct,
                Wrong = userDataPost.Wrong,
            };

            _userDataDbContext.UserDatas.Add(userData);
            await _userDataDbContext.SaveChangesAsync();

            var result = new Result<Data.UserData>(new Data.UserData
            {
                Id = userData.Id,
                Correct = userDataPost.Correct,
                Wrong = userDataPost.Wrong
            });

            return result;
        }
    }
}