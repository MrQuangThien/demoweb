using System.ComponentModel.DataAnnotations;

namespace WebQuanLiCuaHangBanOto.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Gmail đã đăng ký")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Gmail { get; set; }
    }
}
