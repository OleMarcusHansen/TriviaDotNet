using HIOF.Net.V2023.DatabaseService.Data;
using HIOF.Net.V2023.DatabaseService.Model.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HIOF.Net.V2023.DatabaseService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0")]
    public class DatabaseController : ControllerBase
    {

        private readonly ILogger<DatabaseController> _logger;
        private readonly UserDataDbContext _userDataDbContext;
        private readonly HighScoreDbContext _highScoreDbContext;

        public DatabaseController(ILogger<DatabaseController> logger, UserDataDbContext userDataDbContext, HighScoreDbContext highScoreDbContext)
        {
            _logger = logger;
            _userDataDbContext = userDataDbContext;
            _highScoreDbContext = highScoreDbContext;
        }

        [HttpGet("UserData/getAll")]
        public async Task<Result<IEnumerable<Data.UserData>>> GetAllUserData()
        {
            var responsUserData = await _userDataDbContext.UserDatas.Select(userData => new Data.UserData
            {
                Id = userData.Id,
                Correct = userData.Correct,
                Wrong = userData.Wrong,
            }).ToListAsync();

            return new Result<IEnumerable<Data.UserData>>(responsUserData);
        }

        [HttpGet("UserData/get/{id}")]
        public async Task<Result<Data.UserData>> GetUserData(Guid id)
        {
            var responsUserData = await _userDataDbContext.UserDatas.FindAsync(id);

            if (responsUserData == null)
            {
                _logger.LogWarning("Userdata not found");
                return new Result<Data.UserData>(new Data.UserData())
                {
                    
                    Errors = new List<string> { "UserData not found" }
                };
            }

            return new Result<Data.UserData>(responsUserData);
        }

        [HttpGet("UserData/average")]
        public async Task<Result<Data.UserData>> GetAverageUserData()
        {
            var userDatas = await _userDataDbContext.UserDatas.Select(userData => new Data.UserData
            {
                Id = userData.Id,
                Correct = userData.Correct,
                Wrong = userData.Wrong,
            }).ToListAsync();

            var averageUserData = new Data.UserData();

            foreach (var userData in userDatas)
            {
                averageUserData.Correct += userData.Correct;
                averageUserData.Wrong += userData.Wrong;
            }

            averageUserData.Correct /= userDatas.Count;
            averageUserData.Wrong /= userDatas.Count;

            return new Result<Data.UserData>(averageUserData);
        }

        [HttpPost("UserData/create")]
        public async Task<Result<Data.UserData>> CreateUserData(PostUserData userDataPost)
        {

            var userData = new Data.UserData
            {
                Id = userDataPost.Id,
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
        
        [HttpPut("UserData/update")]
        public async Task<Result<Data.UserData>> UpdateUserDataAdd(Guid id, int correct, int wrong)
        {
            PostUserData userDataPost = new PostUserData(id, correct, wrong);

            var userData = await _userDataDbContext.UserDatas.FindAsync(userDataPost.Id);

            if (userData == null)
            {
                _logger.LogWarning("UserData not found");
                return new Result<Data.UserData>(new Data.UserData())
                {
                    Errors = new List<string> { "UserData not found" }
                    
                };
            }

            userData.Correct += userDataPost.Correct;
            userData.Wrong += userDataPost.Wrong;

            _userDataDbContext.UserDatas.Update(userData);
            await _userDataDbContext.SaveChangesAsync();

            var result = new Result<Data.UserData>(new Data.UserData
            {
                Id = userData.Id,
                Correct = userData.Correct,
                Wrong = userData.Wrong
            });

            return result;
        }

        [HttpPost("HighScore/create")]
        public async Task<Result<Data.HighScore>> CreateHighScore(PostHighScore highScorePost)
        {
            var userData = await _userDataDbContext.UserDatas.FindAsync(highScorePost.Id);

            var highScore = new Data.HighScore
            {
                Id = highScorePost.Id,
                Category = highScorePost.Category,
                Correct = highScorePost.Correct,
                Wrong = highScorePost.Wrong,
                User = userData
            };

            _highScoreDbContext.HighScores.Add(highScore);
            await _highScoreDbContext.SaveChangesAsync();

            var result = new Result<Data.HighScore>(new Data.HighScore
            {
                Id = highScore.Id,
                Category = highScore.Category,
                Correct = highScorePost.Correct,
                Wrong = highScorePost.Wrong,
                User = userData
            });

            return result;
        }
    }
}