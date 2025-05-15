using Microsoft.AspNetCore.Mvc;
using WebQuanLiCuaHangBanOto.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System;

public class SanPhamController : Controller
{
    private readonly QLCHOTOContext _context;
    private readonly IWebHostEnvironment _env;

    public SanPhamController(QLCHOTOContext db, IWebHostEnvironment env)
    {
        _context = db;
        _env = env;
    }

    // ========== READ (List Products) ==========
    public IActionResult Docbangsanpham()
    {
        var ds = _context.Sanphams.ToList();

        foreach (var item in ds)
        {
            if (item.HinhAnh != null && item.HinhAnh.Length > 0)
            {
                item.HinhAnhBase64 = "data:image/png;base64," + Convert.ToBase64String(item.HinhAnh);
            }
        }

        return View(ds);
    }

    // ========== CREATE ==========
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Sanpham sanpham)
    {
        if (ModelState.IsValid)
        {
            if (sanpham.HinhAnhUpload != null && sanpham.HinhAnhUpload.Length > 0)
            {
                using var ms = new MemoryStream();
                await sanpham.HinhAnhUpload.CopyToAsync(ms);
                sanpham.HinhAnh = ms.ToArray();
                sanpham.HinhAnhBase64 = "data:image/png;base64," + Convert.ToBase64String(sanpham.HinhAnh);
            }

            _context.Add(sanpham);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thêm sản phẩm thành công!";
            return RedirectToAction(nameof(Docbangsanpham));
        }

        return View(sanpham);
    }

    // ========== EDIT ==========
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var sp = await _context.Sanphams.FindAsync(id);
        if (sp == null) return NotFound();

        if (sp.HinhAnh != null && sp.HinhAnh.Length > 0)
        {
            sp.HinhAnhBase64 = "data:image/png;base64," + Convert.ToBase64String(sp.HinhAnh);
        }

        return View(sp);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Sanpham model, IFormFile? file)
    {
        if (id != model.Idsp)
            return NotFound();

        var sanPham = await _context.Sanphams.FindAsync(id);
        if (sanPham == null)
            return NotFound();

        if (file != null && file.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            sanPham.HinhAnh = memoryStream.ToArray();
            sanPham.HinhAnhBase64 = "data:image/png;base64," + Convert.ToBase64String(sanPham.HinhAnh);
        }

        // Cập nhật các trường còn lại
        sanPham.TenSp = model.TenSp;
        sanPham.NgaySanXuat = model.NgaySanXuat;
        sanPham.LoaiXe = model.LoaiXe;
        sanPham.HangXe = model.HangXe;
        sanPham.GiaBan = model.GiaBan;
        sanPham.SoLuong = model.SoLuong;

        if (ModelState.IsValid)
        {
            _context.Update(sanPham);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction(nameof(Docbangsanpham));
        }

        return View(sanPham);
    }

    // ========== DELETE ==========
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id <= 0)
            return BadRequest();

        var sp = _context.Sanphams.FirstOrDefault(x => x.Idsp == id);
        if (sp == null)
            return NotFound();

        return View(sp);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var sanpham = await _context.Sanphams.FindAsync(id);
        if (sanpham == null)
            return NotFound();

        // Xóa dữ liệu liên quan trước khi xóa sản phẩm
        _context.Chitietdonhangs.RemoveRange(_context.Chitietdonhangs.Where(c => c.Idsp == id));
        _context.Danhgia.RemoveRange(_context.Danhgia.Where(d => d.Idsp == id));
        _context.Baohanhs.RemoveRange(_context.Baohanhs.Where(b => b.Idsp == id));
        _context.ThongkeSlbans.RemoveRange(_context.ThongkeSlbans.Where(t => t.Idsp == id));

        _context.Sanphams.Remove(sanpham);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Xóa sản phẩm thành công!";
        return RedirectToAction(nameof(Docbangsanpham));
    }
}
