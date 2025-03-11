using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreMVCAPI.Models
{
    public class SalaryGrade
    {
        public int ID { get; set; }

        public int SalaryGradeLevel{ get; set; }

        public int SalaryBonusBase { get; set; }

        public string? SalaryGradeName { get; set; }
    }
}
