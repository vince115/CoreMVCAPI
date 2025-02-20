using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreMVCAPI.Models
{
    public class PositionGrade
    {
        public int ID { get; set; }

        public int PositionID { get; set; }

        [Required]
        [StringLength(255)]
        public string? PositionGradeName { get; set; }

        public int PositionLevel { get; set; }

        public int DisplayOrder { get; set; }

        public int? StopDays { get; set; }

        public int? ExpStopDays { get; set; }

        public int? QuarCategory { get; set; }

        public double? BasicSalary { get; set; }
    }
}
