using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreMVCAPI.Data;
using CoreMVCAPI.Models;

namespace CoreMVCAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 這裡加上 JWT 驗證
    public class SiteInfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SiteInfoController(ApplicationDbContext context)
        {
            _context = context;
        }


        // 1. 獲取所有 SiteInfo
        [HttpGet()]
        public IActionResult GetAll()
        {
            var siteInfos = _context.SiteInfo.ToList();
            return Ok(siteInfos);
        }

        // 2. 根據 ID 獲取單個 SiteInfo
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var siteInfo = _context.SiteInfo.FirstOrDefault(s => s.ID == id);
            if (siteInfo == null) return NotFound();
            return Ok(siteInfo);
        }

        // 3. 新增 SiteInfo
        [HttpPost]
        public IActionResult Create(SiteInfo siteInfo)
        {
            _context.SiteInfo.Add(siteInfo);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = siteInfo.ID }, siteInfo);
        }

        // 4. 更新 SiteInfo
        [HttpPut("{id}")]
        public IActionResult Update(int id, SiteInfo siteInfo)
        {
            var existingSiteInfo = _context.SiteInfo.FirstOrDefault(s => s.ID == id);
            if (existingSiteInfo == null) return NotFound();

            existingSiteInfo.SiteName = siteInfo.SiteName;

            _context.SaveChanges();
            return NoContent();
        }

        // 5. 刪除 SiteInfo
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var siteInfo = _context.SiteInfo.FirstOrDefault(s => s.ID == id);
            if (siteInfo == null) return NotFound();

            _context.SiteInfo.Remove(siteInfo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
