using System.ComponentModel.DataAnnotations;

namespace CoreMVCAPI.Models
{
	public class Staff
	{
		public int Id { get; set; } // 員工 ID

		[Required(ErrorMessage = "姓名是必填項")]
		public string Name { get; set; } = string.Empty; // 員工姓名

		[Required(ErrorMessage = "職位是必填項")]
		public string Position { get; set; } = string.Empty; // 職位

		[Range(0, double.MaxValue, ErrorMessage = "薪資必須為正數")]
		public decimal Salary { get; set; } // 薪資
	}
}
