using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreMVCAPI.Models
{
    public class JobDeputy
    {
        public int ID { get; set; }

        public int DeptID { get; set; }

        public int PositionID { get; set; }

        public int StaffID { get; set; }

        [StringLength(3)]
        public string? TelExt { get; set; }

        public int FirstDeputy { get; set; }

        [StringLength(3)]
        public string? DirstDeputyExt { get; set; }

        public int? SecondDeputy { get; set; }

        [StringLength(3)]
        public string? SecondDeputyExt { get; set; }

        public string? Notes { get; set; }
    }
}
