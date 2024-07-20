using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginRequest request)
        {
            // Simulate asynchronous operation
            await Task.Yield();

            if (request.Username == "test" && request.Password == "password")
            {
                var token = await GenerateJwtTokenAsync(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private static async Task<string> GenerateJwtTokenAsync(string username)
        {
            // Simulate asynchronous operation
            await Task.Yield();

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your_issuer",
                audience: "your_audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}