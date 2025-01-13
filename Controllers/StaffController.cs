using Microsoft.AspNetCore.Mvc;
using CoreMVCAPI.Data;
using CoreMVCAPI.Models;
using System.Linq;

namespace CoreMVCAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StaffController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public StaffController(ApplicationDbContext context)
		{
			_context = context;
		}

		// 1. 獲取所有 Staff
		[HttpGet]
		public IActionResult GetAll()
		{
			var staffs = _context.Staffs.ToList();
			return Ok(staffs);
		}

		// 2. 根據 ID 獲取單個 Staff
		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var staff = _context.Staffs.FirstOrDefault(s => s.Id == id);
			if (staff == null) return NotFound();
			return Ok(staff);
		}

		// 3. 新增 Staff
		[HttpPost]
		public IActionResult Create(Staff staff)
		{
			_context.Staffs.Add(staff);
			_context.SaveChanges();
			return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
		}

		// 4. 更新 Staff
		[HttpPut("{id}")]
		public IActionResult Update(int id, Staff staff)
		{
			var existingStaff = _context.Staffs.FirstOrDefault(s => s.Id == id);
			if (existingStaff == null) return NotFound();

			existingStaff.Name = staff.Name;
			existingStaff.Position = staff.Position;
			existingStaff.Salary = staff.Salary;

			_context.SaveChanges();
			return NoContent();
		}

		// 5. 刪除 Staff
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var staff = _context.Staffs.FirstOrDefault(s => s.Id == id);
			if (staff == null) return NotFound();

			_context.Staffs.Remove(staff);
			_context.SaveChanges();
			return NoContent();
		}
	}
}
