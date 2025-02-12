using HIOF.Net.V2023.DatabaseService.Data;
using HIOF.Net.V2023.DatabaseService.Model.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HIOF.Net.V2023.DatabaseService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/statistics")]
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

        [HttpGet("userdata/getall")]
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

        [HttpGet("userdata/get/{id}")]
        public async Task<Result<Data.UserData>> GetUserData(Guid id)
        {
            _logger.LogInformation("Getting userdata");
            var responseUserData = await _userDataDbContext.UserData.FindAsync(id);

            if (responseUserData == null)
            {
                _logger.LogError("An error accured while getting userdata(Userdata not found)");
                return new Result<Data.UserData>(new Data.UserData { Id=id})
                {
                    Errors = new List<string> { "UserData not found" }
                };
            }

            return new Result<Data.UserData>(responseUserData);
        }

        [HttpGet("userdata/average")]
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

        [HttpPost("userdata/create")]
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
        
        [HttpPut("userdata/update")]
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
            _logger.LogInformation("Saving changes to database");

            var result = new Result<Data.UserData>(new Data.UserData
            {
                Id = userData.Id,
                Correct = userData.Correct,
                Wrong = userData.Wrong

            });


            return result;
        }

        [HttpPost("highscore/create")]
        public async Task<Result<Data.HighScore>> CreateHighScore(Guid id, string category, int correct, int wrong, int score)
        {
            var userData = await _userDataDbContext.UserData.FindAsync(id);
            if (userData == null)
            {
                _logger.LogWarning("Userdata attached to highscore not found");
                return new Result<HighScore>(new HighScore { Id=id})
                {
                    Errors = new List<string> { "Userdata attached to highscore not found" }
                };
            }

            var highScore = new Data.HighScore
            {
                Id = id,
                Category = category,
                Correct = correct,
                Wrong = wrong,
                Score = score
            };

            _highScoreDbContext.HighScores.Add(highScore);
            await _highScoreDbContext.SaveChangesAsync();

            var result = new Result<Data.HighScore>(new Data.HighScore
            {
                Id = highScore.Id,
                Category = highScore.Category,
                Correct = highScore.Correct,
                Wrong = highScore.Wrong,
                Score = highScore.Score,
                User = userData
            });

            var url = $"https://localhost:7043/api/1.0/notification/notify?user={id}&message=Congrats on your first ever high score in the {category} category!";
            using var client = new HttpClient();
            await client.GetAsync(url);

            return result;
        }

        [HttpGet("highscore/get/{id}/{category}")]
        public async Task<Result<Data.HighScore>> GetHighScore(Guid id, string category)
        {
            var userData = await _userDataDbContext.UserData.FindAsync(id);
            if (userData == null)
            {
                _logger.LogWarning("Userdata attached to highscore not found");
                return new Result<HighScore>(new HighScore { Id=id})
                {
                    Errors = new List<string> { "Userdata attached to highscore not found" }
                };
            }

            var responseHighScore = await _highScoreDbContext.HighScores.FindAsync(id, category);
            if (responseHighScore == null)
            {
                _logger.LogWarning("Highscore not found");
                return new Result<HighScore>(new HighScore { Id=id, Category=category})
                {
                    Errors = new List<string> { "Highscore not found" }
                };
            }

            responseHighScore.User = userData;

            return new Result<HighScore>(responseHighScore);
        }

        [HttpGet("highscore/compareexisting")]
        public async Task<Result<Data.HighScore>> CompareExistingHighScore(Guid id, string category, int correct, int wrong)
        {
            var userData = await _userDataDbContext.UserData.FindAsync(id);
            if (userData == null)
            {
                _logger.LogWarning("Userdata attached to highscore not found");
                return new Result<HighScore>(new HighScore { Id = id })
                {
                    Errors = new List<string> { "Userdata attached to highscore not found" }
                };
            }

            var highScore = new Data.HighScore
            {
                Id = id,
                Category = category,
                Correct = correct,
                Wrong = wrong
            };

            var existingHighScore = await _highScoreDbContext.HighScores.FindAsync(id, category);
            if (existingHighScore == null)
            {
                _logger.LogWarning("Highscore not found");
                return new Result<HighScore>(new HighScore { Id = id, Category = category })
                {
                    Errors = new List<string> { "Highscore not found" }
                };
            }

            if ((float)highScore.Correct / highScore.Wrong <= (float)existingHighScore.Correct / existingHighScore.Wrong)
            {
                highScore.Correct = existingHighScore.Correct;
                highScore.Wrong = existingHighScore.Wrong;
            }

            highScore.User = userData;

            return new Result<HighScore>(highScore);
        }

        [HttpPut("highscore/update")]
        public async Task<Result<Data.HighScore>> UpdateHighScore(Guid id, string category, int correct, int wrong, int score)
        {
            _logger.LogInformation("Called UpdateHighScore");
            
            var userData = await _userDataDbContext.UserData.FindAsync(id);
            if (userData == null)
            {
                _logger.LogWarning("Userdata attached to highscore not found");
                return new Result<HighScore>(new HighScore { Id=id})
                {
                    Errors = new List<string> { "Userdata attached to highscore not found" }
                };
            }

            var highScore = await _highScoreDbContext.HighScores.FindAsync(id, category);
            if (highScore == null)
            {
                _logger.LogWarning("Highscore not found");
                return new Result<HighScore>(new HighScore { Id=id, Category=category})
                {
                    Errors = new List<string> { "Highscore not found" }
                };
            }

            highScore.Correct = correct;
            highScore.Wrong = wrong;
            highScore.Score = score;

            _highScoreDbContext.HighScores.Update(highScore);
            await _highScoreDbContext.SaveChangesAsync();

            var result = new Result<HighScore>(new HighScore
            {
                Id = highScore.Id,
                Category = highScore.Category,
                Correct = highScore.Correct,
                Wrong = highScore.Wrong,
                Score = highScore.Score,
                User = userData
            });

            var url = $"https://localhost:7043/api/1.0/notification/notify?user={id}&message=Congrats on your new high score in the {category} category!";
            using var client = new HttpClient();
            await client.GetStringAsync(url);

            return result;
        }
    }
}