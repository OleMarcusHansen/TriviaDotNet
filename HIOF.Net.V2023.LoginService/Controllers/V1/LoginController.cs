using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HIOF.Net.V2023.LoginService.Controllers.V1
{
    [Route("api/1.0")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string email, string username, string password)
        {
            var userExists = await _userManager.FindByEmailAsync(email);
            if (userExists != null)
            {
                return BadRequest("User already exists");
            }
            var newUser = new IdentityUser { UserName = username, Email = email };

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                String token = GenerateToken(username);

                return Ok(new { Token = token });
            }

            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(String username, String password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                String token = GenerateToken(username);
                return Ok(new { Token = token });
            }
            else
            {
                return BadRequest("Invalid username or password");
            }
        }

        private String GenerateToken(string username)
        {
            return "token";
        }
    }
}
