﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreMVCAPI.Data;
using CoreMVCAPI.Models;

namespace CoreMVCAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
    [Authorize] // 這裡加上 JWT 驗證
    public class StaffController : ControllerBase
	{
		private readonly ApplicationDbContext dbo;

		public StaffController(ApplicationDbContext context)
		{
			dbo = context;
		}
		

		// 1. 獲取所有 Staff
		//[HttpGet()]
		//public IActionResult GetAll()
		//{

  //          try
  //          {
  //              var staffs = _context.Staff
		//			.Select(s => new
		//			{
		//				Id = s.Id,
  //                      PositionID = s.PositionID,
  //                      DepID = s.DepID,
  //                      ADAccount = s.ADAccount ?? "未知",
		//				Name = s.Name ?? "未知",
		//				PositionName = s.PositionName ?? "未知",
  //                      TakeOfficeDate = s.TakeOfficeDate.HasValue ? s.TakeOfficeDate.Value.ToString("yyyy-MM-dd") : null, // ✅ 正確轉換為 `string`
  //                      SystemAccount = s.SystemAccount ?? "未知",
  //                      Phone1 = s.Phone1 ?? "未知",
		//				Phone2 = s.Phone2 ?? "未知",
		//				PhoneExt = s.PhoneExt ?? "未知",
  //                      Birthday = s.Birthday.HasValue ? s.Birthday.Value.ToString("yyyy-MM-dd") : null // ✅ 正確轉換為 `string`
                        
		//			})
		//			.OrderBy(s=>s.ADAccount)
		//			.ToList();

		//		return Ok(staffs);
		//		}
  //          catch (Exception ex)
  //          {
  //              return StatusCode(500, new { message = "發生錯誤", error = ex.Message });
  //          }
  //      }


        [HttpGet()]
        public IActionResult GetAll()
        {
            try
            {
                var staffs = (
                    from s in dbo.Staff
                    join st in dbo.SiteInfo on s.SiteID equals st.ID into sts
                    from st in sts.DefaultIfEmpty()
                    join d in dbo.JobDeputy on s.Id equals d.StaffID into ds
                    from d in ds.DefaultIfEmpty()
                    join dept in dbo.DepartmentInfo on s.DepID equals dept.DepID into depts
                    from dept in depts.DefaultIfEmpty()
                    join s1 in dbo.Staff on d.FirstDeputy equals s1.Id into s1s
                    from s1 in s1s.DefaultIfEmpty()
                    join s2 in dbo.Staff on d.SecondDeputy equals s2.Id into s2s
                    from s2 in s2s.DefaultIfEmpty()
                    join Si in dbo.SiteInfo on s.SiteID equals Si.ID into Ssi
                    from Si in Ssi.DefaultIfEmpty()
                    join pg in dbo.PositionGrade on s.PositionGradeID equals pg.ID into pgs
                    from pg in pgs.DefaultIfEmpty()
                    select new
                    {
                        Id = s.Id,
                        Name = s.Name ?? "未知",
                        //TakeOfficeDate = s.TakeOfficeDate.HasValue ? s.TakeOfficeDate.Value.ToString("yyyy-MM-dd") : null,
                        SystemAccount = s.SystemAccount ?? "未知",
                        PositionName = s.PositionName ?? "未知",
                        PositionGradeName = pg.PositionGradeName ?? "未知",
                        DeptName = dept.DeptName ?? "未知",
                        Phone1 = s.Phone1 ?? "未知",
                        OfficialPhone = s.OfficialPhone ?? "未知",
                        PhoneExt = s.PhoneExt ?? "未知",
                        Site = Si.SiteName ?? "未知",
                        //FirstDeputyID = s1 != null ? s1.Id : 0,
                        //FirstDeputyName = s1?.Name ?? "未知",
                        //SecondDeputyID = s2 != null ? s2.Id : 0,
                        //SecondDeputyName = s2?.Name ?? "未知",
                        ADAccount = s.ADAccount ?? "未知",
                        IsActive = s.IsActive,
                        TakeOfficeDate = s.TakeOfficeDate != null ? s.TakeOfficeDate.Value.ToString("yyyy-MM-dd") : "未知",
                        Birthday = s.Birthday != null ? s.Birthday.Value.ToString("yyyy-MM-dd") : "未知",

                        //Birthday = s.Birthday.HasValue ? s.Birthday.Value.ToString("yyyy-MM-dd") : null
                    }
                );

            
                // 排序
                var result = staffs.OrderByDescending(s => s.IsActive)
                                   .ThenBy(s => s.ADAccount)
                                   .ThenBy(s => s.Id)
                                   .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "發生錯誤", error = ex.Message });
            }
        }




        // 2. 根據 ID 獲取單個 Staff
        [HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var staff = dbo.Staff.FirstOrDefault(s => s.Id == id);
			if (staff == null) return NotFound();
			return Ok(staff);
		}

		// 3. 新增 Staff
		[HttpPost]
		public IActionResult Create(Staff staff)
		{
			dbo.Staff.Add(staff);
			dbo.SaveChanges();
			return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
		}

		// 4. 更新 Staff
		[HttpPut("{id}")]
		public IActionResult Update(int id, Staff staff)
		{
			var existingStaff = dbo.Staff.FirstOrDefault(s => s.Id == id);
			if (existingStaff == null) return NotFound();
            		
			existingStaff.ADAccount = staff.ADAccount;
            existingStaff.Name = staff.Name;
			existingStaff.PositionName = staff.PositionName;
			existingStaff.TakeOfficeDate = staff.TakeOfficeDate;
            existingStaff.SystemAccount = staff.SystemAccount;
            existingStaff.Phone1 = staff.Phone1;
            existingStaff.Phone2 = staff.Phone2;
            existingStaff.PhoneExt = staff.PhoneExt;
            existingStaff.Birthday = staff.Birthday;

            dbo.SaveChanges();
			return NoContent();
		}

		// 5. 刪除 Staff
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var staff = dbo.Staff.FirstOrDefault(s => s.Id == id);
			if (staff == null) return NotFound();

			dbo.Staff.Remove(staff);
			dbo.SaveChanges();
			return NoContent();
		}
	}
}
