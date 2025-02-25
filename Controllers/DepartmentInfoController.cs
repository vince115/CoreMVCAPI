using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreMVCAPI.Data;
using CoreMVCAPI.Models;

namespace CoreMVCAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 這裡加上 JWT 驗證
    public class DepartmentInfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentInfoController(ApplicationDbContext context)
        {
            _context = context;
        }


        // 1. 獲取所有 DepartmentInfo
        [HttpGet()]
        public IActionResult GetAll()
        {
            var depInfos = _context.DepartmentInfo.ToList();
            return Ok(depInfos);
        }

        // 2. 根據 ID 獲取單個 DepartmentInfo
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var depInfo = _context.DepartmentInfo.FirstOrDefault(s => s.ID == id);
            if (depInfo == null) return NotFound();
            return Ok(depInfo);
        }

        // 3. 新增 DepartmentInfo
        [HttpPost]
        public IActionResult Create(DepartmentInfo depInfo)
        {
            _context.DepartmentInfo.Add(depInfo);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = depInfo.ID }, depInfo);
        }

        // 4. 更新 DepartmentInfo
        [HttpPut("{id}")]
        public IActionResult Update(int id, DepartmentInfo depInfo)
        {
            var existingDepInfo = _context.DepartmentInfo.FirstOrDefault(s => s.ID == id);
            if (existingDepInfo == null) return NotFound();

            existingDepInfo.DeptName = depInfo.DeptName;

            _context.SaveChanges();
            return NoContent();
        }

        // 5. 刪除 DepartmentInfo
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var depInfo = _context.DepartmentInfo.FirstOrDefault(s => s.ID == id);
            if (depInfo == null) return NotFound();

            _context.DepartmentInfo.Remove(depInfo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
