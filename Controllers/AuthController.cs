using CoreMVCAPI.Models; // 引入 LoginModel 所在的命名空間
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreMVCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("請提供有效的帳號與密碼");
            }

            // 假設這裡是驗證邏輯，實際上應該連接資料庫驗證
            if (model.Username == "admin" && model.Password == "123456")
            {
                var token = GenerateJwtToken(model.Username);
                return Ok(new { token });
            }

            return Unauthorized("帳號或密碼錯誤");
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            //var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
            string secretKeyString = jwtSettings.GetValue<string>("SecretKey") ?? string.Empty;

            if (string.IsNullOrEmpty(secretKeyString))
            {
                throw new Exception("JWT SecretKey 未正確設定，請檢查 appsettings.json");
            }

            var secretKey = Encoding.UTF8.GetBytes(secretKeyString);
            var key = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin") // 這裡可以加入角色
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
