﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoreMVCAPI.Models
{
	public class Staff
	{
        internal readonly string? name;
        internal string? identityID;
        internal int siteID;
        internal int depID;

        public int ID { get; set; }

        //[StringLength(100)]
        //public string Name { get; set; }  // ✅ 允許為空

        //[StringLength(50)]
        //public string? IdentityID { get; set; }  // ✅ 設為可空 (nullable)

        //public int SiteID { get; set; }
        //public int DepID { get; set; }
        //public string PositionName { get; set; }
        //public int PositionID { get; set; }
        //public int? PositionGradeID { get; set; }
        //public int? PositionLevel { get; set; }
        //public int? SalaryGradeID { get; set; }
        //public int? SalaryGradeLevel { get; set; }
        //public string DeptName { get; set; }
        //public string? EName { get; set; }   // ✅ 設為可空
        //public string? NickName { get; set; }   // ✅ 設為可空
        //public string? AdAccount { get; set; }
        //public string? SystemAccount { get; set; }
        //public string? Addr { get; set; }
        //public string? MailingAddress { get; set; }
        //public string? BankAccount { get; set; }
        //public string? Tel { get; set; }
        //public string? Phone1 { get; set; }
        //public string? Phone2 { get; set; }
        //public string? PhoneExt { get; set; }
        //public string? OfficialPhone { get; set; }
        //public string? EMail1 { get; set; }
        //public string? EMail2 { get; set; }
        //public string? BloodType { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? Birthday { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? TakeOfficeDate { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? LeaveOfficeDate { get; set; }

        //public bool IsActive { get; set; } = false;
        //public bool Marriage { get; set; } = false;
        //public bool IsProjectBonus { get; set; } = false;
        //public bool IsPerformanceBonus { get; set; } = false;


        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(18)]
        public string? IdentityID { get; set; }


        [StringLength(10)]
        public string? NickName { get; set; }

        public DateTime? Birthday { get; set; }

        public int? CityID { get; set; }

        public int? AreaID { get; set; }

        [StringLength(200)]
        public string? Addr { get; set; }

        [StringLength(50)]
        public string? Tel { get; set; }

        [StringLength(50)]
        public string? Phone1 { get; set; }

        [StringLength(50)]
        public string? Phone2 { get; set; }

        [StringLength(100)]
        public string? EMail1 { get; set; }

        [StringLength(100)]
        public string? EMail2 { get; set; }

        public DateTime? TakeOfficeDate { get; set; }

        public DateTime? LeaveOfficeDate { get; set; }

        [StringLength(10)]
        public string? SystemAccount { get; set; }

        [JsonIgnore]
        [StringLength(50)]
        public string? Pwd { get; set; }

        [StringLength(255)]
        public string? Remark { get; set; }

        public int? DepID { get; set; }

        public int? PositionID { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? Creator { get; set; }

        public DateTime? LstUpdDate { get; set; }

        public int? Modifier { get; set; }

        public bool? IsActive { get; set; }

        public int? Manager { get; set; }

        public int? SiteID { get; set; }

        public DateTime? LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        //public byte[] Attachment { get; set; }

        [StringLength(5)]
        public string? PhoneExt { get; set; }

        [StringLength(50)]
        public string? OfficialPhone { get; set; }

        [StringLength(50)]
        public string? EName { get; set; }

        public string? SealImagePath { get; set; }

        public bool? Marriage { get; set; }

        public bool? Gender { get; set; }

        [StringLength(5)]
        public string? BloodType { get; set; }



        [StringLength(20)]
        public string? EmergencyName { get; set; }

        [StringLength(20)]
        public string? EmergencyPhone { get; set; }

        [StringLength(20)]
        public string? HighestLVedu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GraduationDate { get; set; }

        [StringLength(50)]
        public string? BankAccount { get; set; }

        [StringLength(10)]
        public string? AdAccount { get; set; }

        [StringLength(50)]
        public string? PastExp { get; set; }

        public bool? QuarManualFill { get; set; }

        public bool? WorkEffectivenessCalc { get; set; }

        public string? SealImage { get; set; }

        public string? PushID { get; set; }

        public bool? IsProjectBonus { get; set; }

        public int? PositionLevel { get; set; }

        public int? SalaryGradeLevel { get; set; }

        [StringLength(50)]
        public string? PositionName { get; set; }

        public int? PositionGradeID { get; set; }

        public int? SalaryGradeID { get; set; }

        public int? TeamID { get; set; }

        public int? RealSeniority { get; set; }

        public string? OfficeMail { get; set; }

        public bool? IsPerformanceBonus { get; set; }

        public bool? IsFullCoverRate { get; set; }

        [StringLength(200)]
        public string? MailingAddress { get; set; }


    }
}
