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
            _logger.LogInformation("Called GetAllUserData");
            _logger.LogInformation("Getting all user data");
            var responseUserData = await _userDataDbContext.UserData.Select(userData => new Data.UserData
            {
                Id = userData.Id,
                Correct = userData.Correct,
                Wrong = userData.Wrong,
            }).ToListAsync();

            return new Result<IEnumerable<Data.UserData>>(responseUserData);
        }

        [HttpGet("UserData/get/{id}")]
        public async Task<Result<Data.UserData>> GetUserData(Guid id)
        {
            var responseUserData = await _userDataDbContext.UserData.FindAsync(id);

            if (responseUserData == null)
            {
                _logger.LogWarning("Userdata not found");
                return new Result<Data.UserData>(new Data.UserData())
                {
                    
                    Errors = new List<string> { "UserData not found" }
                };
            }

            return new Result<Data.UserData>(responseUserData);
        }

        [HttpGet("UserData/average")]
        public async Task<Result<Data.UserData>> GetAverageUserData()
        {
            _logger.LogInformation("Called GetAvarageUserData");
            var userDatas = await _userDataDbContext.UserData.Select(userData => new Data.UserData
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

            _logger.LogInformation("Returning avarage user data");
            return new Result<Data.UserData>(averageUserData);
        }

        [HttpPost("UserData/create")]
        public async Task<Result<Data.UserData>> CreateUserData(PostUserData userDataPost)
        {
            _logger.LogInformation("Called CreateUserData");
            var userData = new Data.UserData
            {
                Id = userDataPost.Id,
                Correct = userDataPost.Correct,
                Wrong = userDataPost.Wrong,
            };

            _userDataDbContext.UserData.Add(userData);
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
            _logger.LogInformation("Called UpdateUserDataAdd");

            var userData = await _userDataDbContext.UserData.FindAsync(id);

            if (userData == null)
            {
                _logger.LogError("UserData not found");
                return new Result<Data.UserData>(new Data.UserData())
                {
                    Errors = new List<string> { "UserData not found" }
                    
                };
            }

            userData.Correct += correct;
            userData.Wrong += wrong;

            _userDataDbContext.UserData.Update(userData);
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
            var userData = await _userDataDbContext.UserData.FindAsync(highScorePost.Id);
            if (userData == null)
            {
                _logger.LogWarning("Userdata attached to highscore not found");
                return new Result<HighScore>(new HighScore())
                {
                    Errors = new List<string> { "Userdata attached to highscore not found" }
                };
            }

            var highScore = new Data.HighScore
            {
                Id = highScorePost.Id,
                Category = highScorePost.Category,
                Correct = highScorePost.Correct,
                Wrong = highScorePost.Wrong
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

        [HttpGet("HighScore/get/{id}/{category}")]
        public async Task<Result<Data.HighScore>> GetHighScore(Guid id, string category)
        {
            var userData = await _userDataDbContext.UserData.FindAsync(id);
            if (userData == null)
            {
                _logger.LogWarning("Userdata attached to highscore not found");
                return new Result<HighScore>(new HighScore())
                {
                    Errors = new List<string> { "Userdata attached to highscore not found" }
                };
            }

            var responseHighScore = await _highScoreDbContext.HighScores.FindAsync(id, category);
            if (responseHighScore == null)
            {
                _logger.LogWarning("Highscore not found");
                return new Result<HighScore>(new HighScore())
                {
                    Errors = new List<string> { "Highscore not found" }
                };
            }

            responseHighScore.User = userData;

            return new Result<HighScore>(responseHighScore);
        }

        [HttpPut("HighScore/update")]
        public async Task<Result<Data.HighScore>> UpdateHighScore(PostHighScore highScorePost)
        {
            _logger.LogInformation("Called UpdateHighScore");
            
            var userData = await _userDataDbContext.UserData.FindAsync(highScorePost.Id);
            if (userData == null)
            {
                _logger.LogWarning("Userdata attached to highscore not found");
                return new Result<HighScore>(new HighScore())
                {
                    Errors = new List<string> { "Userdata attached to highscore not found" }
                };
            }

            var highScore = await _highScoreDbContext.HighScores.FindAsync(highScorePost.Id, highScorePost.Category);
            if (highScore == null)
            {
                _logger.LogWarning("Highscore not found");
                return new Result<HighScore>(new HighScore())
                {
                    Errors = new List<string> { "Highscore not found" }
                };
            }

            highScore.Correct = highScorePost.Correct;
            highScore.Wrong = highScorePost.Wrong;

            _highScoreDbContext.HighScores.Update(highScore);
            await _highScoreDbContext.SaveChangesAsync();

            var result = new Result<HighScore>(new HighScore
            {
                Id = highScore.Id,
                Category = highScore.Category,
                Correct = highScore.Correct,
                Wrong = highScore.Wrong,
                User = userData
            });

            return result;
        }
    }
}