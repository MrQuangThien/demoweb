using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebQuanLiCuaHangBanOto.Models;

public class TimKiemController : Controller
{
    private readonly QLCHOTOContext _context;

    public TimKiemController(QLCHOTOContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> KetQua(string keyword)
    {
        ViewBag.TuKhoa = keyword;

        // Tìm sản phẩm
        var sanphams = await _context.Sanphams
            .Where(sp => sp.TenSp.Contains(keyword) || sp.HangXe.Contains(keyword) || sp.LoaiXe.Contains(keyword))
            .ToListAsync();

        // Tìm thông tin khách hàng
        var thongtins = await _context.Thongtins
            .Where(t => t.HoTen.Contains(keyword) || t.Sdt.Contains(keyword) || t.DiaChi.Contains(keyword))
            .ToListAsync();

        // Tìm nhân viên
        var nhanviens = await _context.Nhanviens
            .Where(nv => nv.HoTen.Contains(keyword) || nv.Sdt.Contains(keyword) || nv.Gmail.Contains(keyword))
            .ToListAsync();

        ViewBag.Sanphams = sanphams;
        ViewBag.Thongtins = thongtins;
        ViewBag.Nhanviens = nhanviens;

        return View();
    }
}
