using System.ComponentModel.DataAnnotations;

namespace WebQuanLiCuaHangBanOto.Models
{
    public class RegisterViewModel
    {
        // THÔNG TIN CÁ NHÂN
        [Required]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Required]
        [EmailAddress]
        public string Gmail { get; set; }

        // TÀI KHOẢN ĐĂNG NHẬP
        [Required]
        [Display(Name = "Tên tài khoản")]
        public string TenTk { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        public string Role { get; set; } = "user";
    }
}
