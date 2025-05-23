using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using WebQuanLiCuaHangBanOto.Models;

public class ChitietdonhangController : Controller
{
    private readonly QLCHOTOContext _context;

    public ChitietdonhangController(QLCHOTOContext context)
    {
        _context = context;
    }

    // ========== READ ==========
    public IActionResult DocBangChiTietDonHang()
    {
        var list = _context.Chitietdonhangs
            .ToList();
        return View(list);
    }

    // ========== CREATE ==========
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Hoadons = new SelectList(_context.Hoadons.ToList(), "Idhd", "Idhd");
        ViewBag.Sanphams = new SelectList(_context.Sanphams.ToList(), "Idsp", "TenSp");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Chitietdonhang ctd)
    {
        // Kiểm tra tồn tại khóa ngoại
        if (!_context.Hoadons.Any(h => h.Idhd == ctd.Idhd))
            ModelState.AddModelError("Idhd", "Hóa đơn không tồn tại.");

        if (!_context.Sanphams.Any(s => s.Idsp == ctd.Idsp))
            ModelState.AddModelError("Idsp", "Sản phẩm không tồn tại.");

        // Tính lại thành tiền (nếu cần)
        if (ctd.DonGia.HasValue && ctd.SoLuong.HasValue)
        {
            ctd.ThanhTien = ctd.DonGia.Value * ctd.SoLuong.Value;
        }

        if (ModelState.IsValid)
        {
            _context.Chitietdonhangs.Add(ctd);
            _context.SaveChanges();
            TempData["Message"] = "Thêm chi tiết đơn hàng thành công!";
            return RedirectToAction(nameof(DocBangChiTietDonHang));
        }

        // Gán lại ViewBag nếu model không hợp lệ
        ViewBag.Hoadons = new SelectList(_context.Hoadons.ToList(), "Idhd", "Idhd", ctd.Idhd);
        ViewBag.Sanphams = new SelectList(_context.Sanphams.ToList(), "Idsp", "TenSp", ctd.Idsp);
        return View(ctd);
    }

    // ========== EDIT ==========
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var ctd = _context.Chitietdonhangs.Find(id);
        if (ctd == null) return NotFound();

        ViewBag.Hoadons = new SelectList(_context.Hoadons, "Idhd", "Idhd", ctd.Idhd);
        ViewBag.Sanphams = new SelectList(_context.Sanphams, "Idsp", "TenSp", ctd.Idsp);

        return View(ctd);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Chitietdonhang ctd)
    {
        if (ctd.DonGia.HasValue && ctd.SoLuong.HasValue)
        {
            ctd.ThanhTien = ctd.DonGia.Value * ctd.SoLuong.Value;
        }

        if (ModelState.IsValid)
        {
            _context.Chitietdonhangs.Update(ctd);
            _context.SaveChanges();
            TempData["Message"] = "Cập nhật chi tiết đơn hàng thành công!";
            return RedirectToAction(nameof(DocBangChiTietDonHang));
        }

        ViewBag.Hoadons = new SelectList(_context.Hoadons, "Idhd", "Idhd", ctd.Idhd);
        ViewBag.Sanphams = new SelectList(_context.Sanphams, "Idsp", "TenSp", ctd.Idsp);
        return View(ctd);
    }

    // ========== DELETE ==========
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id <= 0) return BadRequest();

        var ctd = _context.Chitietdonhangs.Find(id);
        if (ctd == null) return NotFound();

        return View(ctd);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var ctd = _context.Chitietdonhangs.Find(id);
        if (ctd == null) return NotFound();

        _context.Chitietdonhangs.Remove(ctd);
        _context.SaveChanges();
        TempData["Message"] = "Xóa chi tiết đơn hàng thành công!";
        return RedirectToAction(nameof(DocBangChiTietDonHang));
    }
}
