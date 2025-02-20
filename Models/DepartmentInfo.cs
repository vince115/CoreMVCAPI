using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreMVCAPI.Models
{
    public class DepartmentInfo
    {
        public int ID { get; set; }

        public int SiteID { get; set; }

        [Required]
        [StringLength(50)]
        public string? DeptName { get; set; }

        public int Manager { get; set; }

        public int? DepID { get; set; }

        public string? SealImage { get; set; }

        public bool? IsProxy { get; set; }

        public int? QuarCategory { get; set; }

        public int? MangerAddTo { get; set; }

        public int? DeputyManager1 { get; set; }

        public int? DeputyManager2 { get; set; }

        public int? DeputyManager3 { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsOmniscient { get; set; }

        public int? DeptType { get; set; }
    }
}
