namespace CoreMVCAPI.Models
{
    public class LoginModel
    {
        //public string Username { get; set; } = string.Empty; // 預設為空字串，避免 null 錯誤
        //public string Password { get; set; } = string.Empty;

        public string? SystemAccount { get; set; } = string.Empty;
        public string? Pwd { get; set; } = string.Empty;
    }
}
