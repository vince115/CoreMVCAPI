using CoreMVCAPI.Data;
using CoreMVCAPI.Models; // 引入 LoginModel 所在的命名空間
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CoreMVCAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
     
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // 宣告一個私有欄位

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; // 取得並儲存，用於後續呼叫
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            //if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            if (model == null || 
                string.IsNullOrEmpty(model.SystemAccount) || 
                string.IsNullOrEmpty(model.Pwd))
                {
                return BadRequest("請提供有效的帳號與密碼");
            }

            // 假設這裡是驗證邏輯，實際上應該連接資料庫驗證
            var staff = _context.Staffs.SingleOrDefault(s => s.SystemAccount == model.SystemAccount);

            if (staff == null)
            {
                return Unauthorized("帳號或密碼錯誤");
            }

            // 使用官方 MD5 進行雜湊 (取代 WA_getMd5Hash)
            var hashedInputPwd = ComputeMd5Hash(model.Pwd);


            // 如果有加密，請先解密 staff.Pwd
            if (staff.Pwd != hashedInputPwd)
            {
                return Unauthorized("帳號或密碼錯誤");
            }
            else
            {
                // 通過驗證
                var token = GenerateJwtToken(staff.SystemAccount);
                return Ok(new { token });
            }


            //if (staff.Pwd != model.Pwd)
            //{
            //    return Unauthorized("帳號或密碼錯誤");
            //}

            //var token = GenerateJwtToken(staff.SystemAccount);
            //return Ok(new { token });

            //if (model.Username == "admin" && model.Password == "123456")
            //{
            //    var token = GenerateJwtToken(model.Username);
            //    return Ok(new { token });
            //}
            //return Unauthorized("帳號或密碼錯誤");
        }

        /// <summary>
        /// 獲取當前使用者資訊
        /// </summary>
        [HttpGet("me")]
        [Authorize] // Requires JWT Token
        public IActionResult Me()
        {
            var user = HttpContext.User;

            if (user.Identity is not { IsAuthenticated: true })
            {
                return Unauthorized(new { message = "未授權" });
            }

            var username = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "使用者名稱無效" });
            }

            // Fetch the user details from the database
            var staff = _context.Staffs
                                .Where(s => s.SystemAccount == username)
                                .Select(s => new
                                {
                                    Id = s.Id,
                                    SystemAccount = s.SystemAccount,
                                    Name = s.Name,  // Assuming `Email` exists in `Staffs`
                                    Position = s.Position  // Mapping role to position
                                })
                                .FirstOrDefault();

            if (staff == null)
            {
                return NotFound(new { message = "使用者資料不存在" });
            }

            return Ok(new
            {
                Id = staff.Id,
                SystemAccount = staff.SystemAccount,
                Name = staff.Name,
                Position = staff.Position
            });
        }

        //刷新 JWT Token (/api/Auth/refresh)
        [HttpPost("refresh")]
        [Authorize]
        public IActionResult RefreshToken()
        {
            var user = HttpContext.User;
            var username = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized("無法驗證使用者");
            }

            var newToken = GenerateJwtToken(username);
            return Ok(new { token = newToken });
        }


        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var user = HttpContext.User;

            // 檢查請求是否包含身份驗證資訊
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "未授權，請提供有效的 Token" });
            }

            // 這裡可以選擇將 Token 加入黑名單，防止重複使用
            // _context.InvalidTokens.Add(new InvalidToken { Token = currentToken, Expiry = tokenExpiry });
            // _context.SaveChanges();

            return Ok(new { message = "登出成功" });
        }



        /// 使用官方 MD5 計算雜湊值
        private static string ComputeMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 將雜湊結果轉換為十六進制字串 (與傳統 MD5 輸出相同)
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
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

}
