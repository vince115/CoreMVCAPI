using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreMVCAPI.Models
{
    public class User
    {
        public int ID { get; set; } // 員工 ID

        [Required(ErrorMessage = "姓名是必填項")]
        public string Name { get; set; } = string.Empty; // 員工姓名

        [Required(ErrorMessage = "職位是必填項")]
        public string Position { get; set; } = string.Empty; // 職位

        [Range(0, double.MaxValue, ErrorMessage = "薪資必須為正數")]
        [Column(TypeName = "decimal(18,2)")] // 指定 SQL Server 列類型
        public decimal Salary { get; set; } // 薪資

        //[Required(ErrorMessage = "帳號是必填項")]
        public string SystemAccount { get; set; } = string.Empty;

        [JsonIgnore] // ✅ 這行確保 API 不會回傳密碼
        public string? Pwd { get; set; }

    }
}
