using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreMVCAPI.Data;
using CoreMVCAPI.Models;
using System.Linq;

namespace CoreMVCAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 這裡加上 JWT 驗證
    public class SalaryGradeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SalaryGradeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. 獲取所有 SalaryGrade
        [HttpGet()]
        public IActionResult GetAll()
        {
            //var salaryGrades = _context.SalaryGrade.ToList();

            var salaryGrades = _context.SalaryGrade
                              .OrderBy(s => s.SalaryGradeLevel) // 依 SalaryGradeLevel 排序
                              .ToList();

            return Ok(salaryGrades);
        }

        // 2. 根據 ID 獲取單個 SalaryGrade
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var salaryGrade = _context.SalaryGrade.FirstOrDefault(s => s.ID == id);
            if (salaryGrade == null) return NotFound();
            return Ok(salaryGrade);
        }

    }
}
