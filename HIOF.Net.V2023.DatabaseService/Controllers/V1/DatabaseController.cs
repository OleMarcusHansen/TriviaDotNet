using HIOF.Net.V2023.DatabaseService.Data;
using HIOF.Net.V2023.DatabaseService.Model.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HIOF.Net.V2023.DatabaseService.Controllers.V1
{
    [ApiController]
    [Route("api/1.0/UserData")]
    public class DatabaseController : ControllerBase
    {

        private readonly ILogger<DatabaseController> _logger;
        private readonly UserDataDbContext _userDataDbContext;

        public DatabaseController(ILogger<DatabaseController> logger, UserDataDbContext userDataDbContext)
        {
            _logger = logger;
            _userDataDbContext = userDataDbContext;
        }

        [HttpGet("getAll")]
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

        [HttpGet("{id}")]
        public async Task<Result<Data.UserData>> GetUserData(Guid id)
        {
            var responsUserData = await _userDataDbContext.UserDatas.FindAsync(id);

            if (responsUserData == null)
            {
                return new Result<Data.UserData>(new Data.UserData { Id = id })
                {
                    
                    Errors = new List<string> { "UserData not found" }
                };
            }

            return new Result<Data.UserData>(responsUserData);
        }

        [HttpGet("average")]
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

        [HttpPost("create")]
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
        
        [HttpPut("update")]
        public async Task<Result<Data.UserData>> UpdateUserDataAdd(Guid id, int correct, int wrong)
        {
            PostUserData userDataPost = new PostUserData(id, correct, wrong);

            var userData = await _userDataDbContext.UserDatas.FindAsync(userDataPost.Id);

            if (userData == null)
            {
                return new Result<Data.UserData>(new Data.UserData { Id = userDataPost.Id })
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
    }
}