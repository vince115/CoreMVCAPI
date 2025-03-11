using Microsoft.AspNetCore.Authorization;
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
                        Name = s.Name ?? "未知",
                        NickName = s.NickName ?? "",
                        EName = s.EName ?? "未知",
                        Marriage = s.Marriage,
                        IdentityID = s.IdentityID ?? "未知",
                        BloodType = s.BloodType ?? "",
                        Addr = s.Addr ?? "",
                        Tel = s.Tel ?? "",
                        MailingAddress = s.MailingAddress ?? "",
                        BankAccount = s.BankAccount ?? "",
                        SystemAccount = s.SystemAccount ?? "未知",
                        PositionName = s.PositionName ?? "未知",
                        PositionGradeName = pg.PositionGradeName ?? "未知",
                        DeptName = dept.DeptName ?? "未知",
                        Phone1 = s.Phone1 ?? "未知",
                        OfficialPhone = s.OfficialPhone ?? "未知",
                        PhoneExt = s.PhoneExt ?? "未知",
                        EMail1 = s.EMail1 ?? "",
                        EMail2 = s.EMail2 ?? "",
                        PositionGradeID = s.PositionGradeID ?? 0,
                        PositionLevel = s.PositionLevel ?? 0,
                        SalaryGradeID = s.SalaryGradeID ?? 0,
                        SalaryGradeLevel = s.SalaryGradeLevel ?? 0,

                        AdAccount = s.AdAccount ?? "未知",
                        IsActive = s.IsActive,
                        TakeOfficeDate = s.TakeOfficeDate != null ? s.TakeOfficeDate.Value.ToString("yyyy-MM-dd") : "未知",
                        Birthday = s.Birthday != null ? s.Birthday.Value.ToString("yyyy-MM-dd") : "未知",

                    
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
        //public IActionResult Create(Staff staff)
        //{
        //	dbo.Staff.Add(staff);
        //	dbo.SaveChanges();
        //	return CreatedAtAction(nameof(GetById), new { id = staff.ID }, staff);
        //}
        [HttpPost]
      public IActionResult Create([FromBody] Staff newStaff)
{
    try
    {
        //Console.WriteLine($"📥 新增員工請求: {JsonConvert.SerializeObject(newStaff)}");

        if (newStaff == null)
        {
            return BadRequest("❌ 提交的員工資料為空!");
        }

        // **檢查必填欄位**
        if (string.IsNullOrEmpty(newStaff.Name) || newStaff.SiteID == 0 || newStaff.DepID == 0)
        {
            return BadRequest("❌ 必填欄位缺失!");
        }

        // **檢查 IdentityID 是否唯一**
        var existingStaff = dbo.Staff.FirstOrDefault(s => s.IdentityID == newStaff.IdentityID);
        if (existingStaff != null)
        {
            return Conflict("❌ 員工已存在!");
        }

        // **嘗試將員工資料寫入數據庫**
        dbo.Staff.Add(newStaff);
        dbo.SaveChanges();

        Console.WriteLine($"✅ 員工 {newStaff.Name} 新增成功");

        return CreatedAtAction(nameof(GetById), new { id = newStaff.ID }, newStaff);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ 伺服器錯誤: {ex.Message}");
        return StatusCode(500, $"❌ 內部伺服器錯誤: {ex.Message}");
    }
}



        // 4. 更新 Staff
        [HttpPut("{id}")]
		public IActionResult Update(int id, Staff staff)
		{
			var existingStaff = dbo.Staff.FirstOrDefault(s => s.ID == id);
			if (existingStaff == null) return NotFound();
            		
			existingStaff.AdAccount = staff.AdAccount;
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
			var staff = dbo.Staff.FirstOrDefault(s => s.ID == id);
			if (staff == null) return NotFound();

			dbo.Staff.Remove(staff);
			dbo.SaveChanges();
			return NoContent();
		}
	}
}
