using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLiCuaHangBanOto.Models;

public partial class Sanpham
{
    public Sanpham()
    {
        Baohanhs = new HashSet<Baohanh>();
        Chitietdonhangs = new HashSet<Chitietdonhang>();
        Danhgia = new HashSet<Danhgia>();
        ThongkeSlbans = new HashSet<ThongkeSlban>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Idsp { get; set; }

    public string TenSp { get; set; }
    public string HangXe { get; set; }
    public string LoaiXe { get; set; }
    public decimal GiaBan { get; set; }
    public DateTime? NgaySanXuat { get; set; }
    public int SoLuong { get; set; }

    public byte[]? HinhAnh { get; set; }

    [NotMapped]
    public IFormFile? HinhAnhUpload { get; set; }

    [NotMapped]

    public string? HinhAnhBase64
    => HinhAnh != null ? $"data:image/png;base64,{Convert.ToBase64String(HinhAnh)}" : null;

    public virtual ICollection<Baohanh> Baohanhs { get; set; }
    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; }
    public virtual ICollection<Danhgia> Danhgia { get; set; }
    public virtual ICollection<ThongkeSlban> ThongkeSlbans { get; set; }
}
