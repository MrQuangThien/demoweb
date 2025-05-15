using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLiCuaHangBanOto.Models;
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
    
    public int Idsp { get; set; }
    public string TenSp { get; set; }
    public string HangXe { get; set; }
    public string LoaiXe { get; set; }
    public decimal GiaBan { get; set; }
    public DateTime? NgaySanXuat { get; set; }
    public int SoLuong { get; set; }

    // Thêm thuộc tính ảnh dạng byte[]
    public byte[]? HinhAnh { get; set; }

    // Thuộc tính hỗ trợ hiển thị ảnh base64 (không map vào DB)
    [NotMapped]
    public string? HinhAnhBase64 { get; set; }

    // Thuộc tính dùng để upload ảnh (không map vào DB)
    [NotMapped]
    public IFormFile? HinhAnhUpload { get; set; }

    // Navigation properties
    public virtual ICollection<Baohanh> Baohanhs { get; set; }
    public virtual ICollection<Chitietdonhang> Chitietdonhangs { get; set; }
    public virtual ICollection<Danhgia> Danhgia { get; set; }
    public virtual ICollection<ThongkeSlban> ThongkeSlbans { get; set; }
}
