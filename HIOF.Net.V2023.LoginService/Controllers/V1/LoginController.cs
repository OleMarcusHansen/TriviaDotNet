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
                return Ok(new { String = "Bruker registrert" });
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
                var token = await _userManager.GenerateUserTokenAsync(_userManager.FindByNameAsync(username).Result, "Default", "token");
                await _userManager.SetAuthenticationTokenAsync(_userManager.FindByNameAsync(username).Result, "Default", "token", token);
                return Ok(new { Token = token });
            }
            else
            {
                return BadRequest("Invalid username or password");
            }
        }
        
        [HttpGet("Verify")]
        public async Task<IActionResult> LoggedIn(String username, String token)
        {
            var valid = await _userManager.VerifyUserTokenAsync(_userManager.FindByNameAsync(username).Result, "Default", "token", token);

            if (valid)
            {
                return Ok(new { String = "logget inn" });
            }
            else
            {
                return BadRequest("Ikke logget inn");
            }
        }
    }
}
