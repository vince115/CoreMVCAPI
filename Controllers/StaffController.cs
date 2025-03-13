using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreMVCAPI.Data;
using CoreMVCAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
			//dbo = context;
            dbo = context ?? throw new ArgumentNullException(nameof(context));
        }


        [HttpGet()]
        public IActionResult GetAll()
        {
            try
            {
                var staffs = (
                    from s in dbo.Staff
                    join st in dbo.SiteInfo on s.SiteID equals st.ID into sts
                    from st in sts.DefaultIfEmpty()
                    join d in dbo.JobDeputy on s.ID equals d.StaffID into ds
                    from d in ds.DefaultIfEmpty()
                    join dept in dbo.DepartmentInfo on s.DepID equals dept.DepID into depts
                    from dept in depts.DefaultIfEmpty()
                    join s1 in dbo.Staff on d.FirstDeputy equals s1.ID into s1s
                    from s1 in s1s.DefaultIfEmpty()
                    join s2 in dbo.Staff on d.SecondDeputy equals s2.ID into s2s
                    from s2 in s2s.DefaultIfEmpty()
                    join Si in dbo.SiteInfo on s.SiteID equals Si.ID into Ssi
                    from Si in Ssi.DefaultIfEmpty()
                    join pg in dbo.PositionGrade on s.PositionGradeID equals pg.ID into pgs
                    from pg in pgs.DefaultIfEmpty()
                    select new
                    {
                        Id = s.ID,
                        SiteID = s.SiteID, 
                        PositionID = s.PositionID, 
                        DepID = s.DepID,  
                        Name = s.Name ?? "無名",
                        NickName = s.NickName ?? "",
                        EName = s.EName ?? "",
                        Marriage = s.Marriage,
                        IdentityID = s.IdentityID ?? "",
                        BloodType = s.BloodType ?? "",
                        Addr = s.Addr ?? "",
                        Tel = s.Tel ?? "",
                        MailingAddress = s.MailingAddress ?? "",
                        BankAccount = s.BankAccount ?? "",
                        SystemAccount = s.SystemAccount ?? "",
                        PositionName = s.PositionName ?? "",
                        PositionGradeName = pg.PositionGradeName ?? "",
                        DeptName = dept.DeptName ?? "",
                        Phone1 = s.Phone1 ?? "",
                        OfficialPhone = s.OfficialPhone ?? "",
                        PhoneExt = s.PhoneExt ?? "",
                        EMail1 = s.EMail1 ?? "",
                        EMail2 = s.EMail2 ?? "",
                        PositionGradeID = s.PositionGradeID ?? 0,
                        PositionLevel = s.PositionLevel ?? 0,
                        SalaryGradeID = s.SalaryGradeID ?? 0,
                        SalaryGradeLevel = s.SalaryGradeLevel ?? 0,

                        AdAccount = s.AdAccount ?? "",
                        IsActive = s.IsActive,
                        TakeOfficeDate = s.TakeOfficeDate != null ? s.TakeOfficeDate.Value.ToString("yyyy-MM-dd") : "",
                        Birthday = s.Birthday != null ? s.Birthday.Value.ToString("yyyy-MM-dd") : "",

                    
                    }
                );

            
                // 排序
                var result = staffs.OrderByDescending(s => s.IsActive)
                                   .ThenBy(s => s.AdAccount)
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
			var staff = dbo.Staff.FirstOrDefault(s => s.ID == id);
			if (staff == null) return NotFound();
			return Ok(staff);
		}

		// 3. 新增 Staff
        [HttpPost]
        public IActionResult Create([FromBody] Staff staff)
        {
            try
            {
                if (staff == null)
                {
                    return BadRequest(new { message = "請提供有效的 Staff 資料" });
                }

                // 檢查 `staff` 物件的必填欄位
                if (string.IsNullOrWhiteSpace(staff.Name) || staff.SiteID == 0 || staff.DepID == 0)
                {
                    return BadRequest(new { message = "缺少必要欄位: Name, SiteID, DepID" });
                }

                // 將 Staff 物件新增到資料庫
                dbo.Staff.Add(staff);
                dbo.SaveChanges();

                // 返回 201 Created，並回傳新增的 Staff 資料
                return CreatedAtAction(nameof(GetById), new { id = staff.ID }, staff);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new {
                    message = "新增 Staff 失敗", 
                    error = dbEx.InnerException?.Message ?? dbEx.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "未知錯誤", error = ex.Message });
            }
        }

        // 4. 更新 Staff
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Staff staff)
        {
            Console.WriteLine($"📥 收到更新請求 ID: {id}");
            Console.WriteLine($"📥 收到更新資料: {JsonSerializer.Serialize(staff)}");

            if (staff == null)
            {
                Console.WriteLine("❌ `staff` 物件為空，回傳 400 Bad Request");
                return BadRequest("請求體中的 `staff` 物件為空");
            }

            var existingStaff = dbo.Staff.FirstOrDefault(s => s.ID == id);
            if (existingStaff == null)
            {
                return NotFound(new { message = $"找不到 ID 為 {id} 的 Staff" });
            }

            try
            {
                // ✅ 更新所有欄位
                existingStaff.SiteID = staff.SiteID;
                existingStaff.PositionID = staff.PositionID;
                existingStaff.DepID = staff.DepID;
                existingStaff.Name = staff.Name;
                existingStaff.NickName = staff.NickName;
                existingStaff.EName = staff.EName;
                existingStaff.IdentityID = staff.IdentityID;
                existingStaff.BloodType = staff.BloodType;
                existingStaff.PositionName = staff.PositionName;
                existingStaff.PositionGradeID = staff.PositionGradeID;
                existingStaff.PositionLevel = staff.PositionLevel;
                existingStaff.SalaryGradeID = staff.SalaryGradeID;
                existingStaff.SalaryGradeLevel = staff.SalaryGradeLevel;
                existingStaff.Marriage = staff.Marriage;
                existingStaff.EmergencyName = staff.EmergencyName;
                existingStaff.EmergencyPhone = staff.EmergencyPhone;
                existingStaff.SystemAccount = staff.SystemAccount;
                existingStaff.AdAccount = staff.AdAccount;
                existingStaff.Addr = staff.Addr;
                existingStaff.MailingAddress = staff.MailingAddress;
                existingStaff.BankAccount = staff.BankAccount;
                existingStaff.Tel = staff.Tel;
                existingStaff.Phone1 = staff.Phone1;
                existingStaff.Phone2 = staff.Phone2;
                existingStaff.OfficialPhone = staff.OfficialPhone;
                existingStaff.PhoneExt = staff.PhoneExt;
                existingStaff.EMail1 = staff.EMail1;
                existingStaff.EMail2 = staff.EMail2;
                existingStaff.IsActive = staff.IsActive;
                existingStaff.TakeOfficeDate = staff.TakeOfficeDate;

                // ✅ 避免 NULL 值覆蓋原本資料
                if (staff.Birthday.HasValue)
                {
                    existingStaff.Birthday = staff.Birthday;
                }

                if (staff.LeaveOfficeDate.HasValue)
                {
                    existingStaff.LeaveOfficeDate = staff.LeaveOfficeDate;
                }

                existingStaff.IsProjectBonus = staff.IsProjectBonus;
                existingStaff.IsPerformanceBonus = staff.IsPerformanceBonus;

                dbo.SaveChanges();
                return Ok(new { message = "更新成功", updatedStaff = existingStaff });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new
                {
                    message = "更新失敗",
                    error = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "未知錯誤", error = ex.Message });
            }
            
		}

		// 5. 刪除 Staff
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var staff = dbo.Staff.FirstOrDefault(s => s.ID == id);
			if (staff == null) return NotFound();

			dbo.Staff.Remove(staff);
			dbo.SaveChanges();
			return NoContent();
		}
	}
}
