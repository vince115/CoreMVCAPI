using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreMVCAPI.Models
{
    public class SiteInfo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string? SiteName { get; set; }

        [StringLength(255)]
        public string? CompanyName { get; set; }

        [StringLength(255)]
        public string? SiteAddr { get; set; }

        [StringLength(20)]
        public string? TaxID { get; set; }

        [StringLength(255)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Fax { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        public int? Manager { get; set; }

        public string? SealImage { get; set; }

        public int? Accountant { get; set; }

        [StringLength(50)]
        public string? ProjectColor { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? IsActive { get; set; }

        public int? AnnualType { get; set; }

        public int? CalendarID { get; set; }

        public int? HRStaff { get; set; }

        public int? BusinessAuditor { get; set; }

        [StringLength(50)]
        public string? SiteCurrency { get; set; }

        public bool? IsOmniscient { get; set; }

        public bool? IsHeadQuarters { get; set; }

        public decimal? ServiceFeePercentage { get; set; }

        public int? Accountant2 { get; set; }

        public int? Accountant3 { get; set; }
    }
}