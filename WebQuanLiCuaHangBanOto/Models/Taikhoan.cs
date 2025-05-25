using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLiCuaHangBanOto.Models;
namespace WebQuanLiCuaHangBanOto.Models;
public partial class Taikhoan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Idtk { get; set; }

    [Required]
    public int Idkh { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
    public string TenTk { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    public string MatKhau { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập Gmail")]
    public string Gmail { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn vai trò")]
    public string Role { get; set; }
    public string? ResetToken { get; set; }
    public DateTime? TokenExpiry { get; set; }


    [ForeignKey("Idkh")]
    public virtual Thongtin? IdkhNavigation { get; set; } // <- sửa lại nullable
    public virtual ICollection<LogDangnhap> LogDangnhaps { get; set; } = new List<LogDangnhap>();
}
