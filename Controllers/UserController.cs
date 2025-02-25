using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreMVCAPI.Data;
using CoreMVCAPI.Models;

namespace CoreMVCAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 這裡加上 JWT 驗證
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }


        // 1. 獲取所有 User
        [HttpGet()]
        public IActionResult GetAll()
        {
            var users = _context.User.ToList();
            return Ok(users);
        }

        // 2. 根據 ID 獲取單個 User
        [HttpGet("{ID}")]
        public IActionResult GetById(int ID)
        {
            var user = _context.User.FirstOrDefault(s => s.ID == ID);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // 3. 新增 User
        [HttpPost]
        public IActionResult Create(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { ID = user.ID }, user);
        }

        // 4. 更新 User
        [HttpPut("{ID}")]
        public IActionResult Update(int ID, User user)
        {
            var existingUser = _context.User.FirstOrDefault(s => s.ID== ID);
            if (existingUser == null) return NotFound();

            existingUser.Name = user.Name;
            existingUser.Position = user.Position;
            existingUser.Salary = user.Salary;

            _context.SaveChanges();
            return NoContent();
        }

        // 5. 刪除 User
        [HttpDelete("{ID}")]
        public IActionResult Delete(int ID)
        {
            var user = _context.User.FirstOrDefault(s => s.ID == ID);
            if (user == null) return NotFound();

            _context.User.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
