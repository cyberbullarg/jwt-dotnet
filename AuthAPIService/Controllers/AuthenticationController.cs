using AuthAPIService.Config;
using AuthAPIService.Entities;
using AuthAPIService.Model.Request;
using AuthAPIService.Model.Response;
using AuthAPIService.Services;
using AuthAPIService.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly JwtConfig _jwtConfig;
        private readonly UserService _service;

        public AuthenticationController(IOptionsMonitor<JwtConfig> options, UserService service)
        {
            _jwtConfig = options.CurrentValue;
            _service = service;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(SignUpRequest req)
        {
            if (!ModelState.IsValid)
            {
                // ...
            }

            if (await _service.FindByEmailAsync(req.Email))
            {
                // ...
            }

            User user = new()
            {
                Name = req.Name,
                Email = req.Email,
                Password = req.Password, // TODO: Encrypt password
                CreatedAt = DateTime.Now,
            };

            if (!await _service.CreateAsync(user))
            {
                // ...
            }

            string token = GenerateJwtToken(user);

            return Ok(new Result<AuthResponse>()
            {
                Failure = false,
                Data = new AuthResponse()
                {
                    Id = user.Id,
                    Name = req.Name,
                    Email = user.Email,
                    CreatedAt = DateTime.Now,
                    Token = token
                }
            });
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn(SignInRequest req)
        {
            if (!ModelState.IsValid)
            {
                // ...
            }

            User? user = await _service.FindByEmailAndPasswordAsync(req.Email, req.Password);

            if (user is null)
            {
                return BadRequest("Incorrect credentials, please try again");
            }

            // TODO: Decrypt and validate password

            string token = GenerateJwtToken(user);

            return Ok(new Result<AuthResponse>()
            {
                Failure = false,
                Data = new AuthResponse()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Token = token
                }
            });
        }

        private string GenerateJwtToken(User user)
        {
            JwtSecurityTokenHandler jwtTokenHandler = new();

            byte[] key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            SecurityToken token = jwtTokenHandler.CreateToken(descriptor);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
