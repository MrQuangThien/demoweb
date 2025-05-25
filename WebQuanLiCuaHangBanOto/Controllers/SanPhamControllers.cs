using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebQuanLiCuaHangBanOto.Models;

public class SanPhamController : Controller
{
    private readonly QLCHOTOContext _context;
    private readonly IWebHostEnvironment _env;

    public SanPhamController(QLCHOTOContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // ========== READ ==========
    public async Task<IActionResult> Docbangsanpham()
    {
        var list = await _context.Sanphams.ToListAsync();
        return View(list); // KHÔNG cần gán HinhAnhBase64
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
            if (sanpham.HinhAnhUpload?.Length > 0)
            {
                using var ms = new MemoryStream();
                await sanpham.HinhAnhUpload.CopyToAsync(ms);
                sanpham.HinhAnh = ms.ToArray();
            }

            _context.Sanphams.Add(sanpham);
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

        return View(sp);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Sanpham model)
    {
        if (id != model.Idsp) return NotFound();

        var sanPham = await _context.Sanphams.FindAsync(id);
        if (sanPham == null) return NotFound();

        if (model.HinhAnhUpload?.Length > 0)
        {
            using var ms = new MemoryStream();
            await model.HinhAnhUpload.CopyToAsync(ms);
            sanPham.HinhAnh = ms.ToArray();
        }

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
        if (id == null || id <= 0) return BadRequest();

        var sp = _context.Sanphams.FirstOrDefault(x => x.Idsp == id);
        if (sp == null) return NotFound();

        return View(sp);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var sanpham = await _context.Sanphams.FindAsync(id);
        if (sanpham == null) return NotFound();

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
