using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly QLCHOTOContext _context;

        public HoaDonController(QLCHOTOContext context)
        {
            _context = context;
        }

        // ========== READ ==========
        public IActionResult DocBangHoaDon()
        {
            var danhSach = _context.Hoadons.Include(h => h.IdkhNavigation).Include(h => h.IdnvNavigation).ToList();
            return View(danhSach);
        }
        public IActionResult Create()
        {
            ViewBag.KhachHangs = new SelectList(_context.Thongtins, "Idkh", "HoTen");
            ViewBag.NhanViens = new SelectList(_context.Nhanviens, "Idnv", "HoTen");
            ViewBag.SanPhams = _context.Sanphams
                .Select(sp => new SelectListItem
                {
                    Value = sp.Idsp.ToString(),
                    Text = $"{sp.TenSp} ({sp.GiaBan} VND)"
                }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hoadon hd, int Idsp, int SoLuong)
        {
            var sp = _context.Sanphams.FirstOrDefault(s => s.Idsp == Idsp);
            if (sp == null)
            {
                ModelState.AddModelError("Idsp", "Sản phẩm không tồn tại.");
            }

            if (ModelState.IsValid)
            {
                hd.ThanhTien = sp.GiaBan * SoLuong;
                hd.SoLuong = SoLuong;
                hd.NgayMua = DateTime.Now;

                _context.Hoadons.Add(hd);
                _context.SaveChanges();

                _context.Chitietdonhangs.Add(new Chitietdonhang
                {
                    Idhd = hd.Idhd,
                    Idsp = Idsp,
                    SoLuong = SoLuong,
                    DonGia = sp.GiaBan,
                    ThanhTien = sp.GiaBan * SoLuong
                });
                _context.SaveChanges();

                return RedirectToAction(nameof(DocBangHoaDon));
            }

            // Re-populate selects if ModelState fails
            ViewBag.KhachHangs = new SelectList(_context.Thongtins, "Idkh", "HoTen", hd.Idkh);
            ViewBag.NhanViens = new SelectList(_context.Nhanviens, "Idnv", "HoTen", hd.Idnv);
            ViewBag.SanPhams = _context.Sanphams
                .Select(sp => new SelectListItem
                {
                    Value = sp.Idsp.ToString(),
                    Text = $"{sp.TenSp} ({sp.GiaBan} VND)"
                }).ToList();

            return View(hd);
        }


        // ========== EDIT ==========
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var hd = _context.Hoadons.Find(id);
            if (hd == null) return NotFound();

            ViewBag.KhachHangs = new SelectList(_context.Thongtins, "Idkh", "HoTen", hd.Idkh);
            ViewBag.NhanViens = new SelectList(_context.Nhanviens, "Idnv", "HoTen", hd.Idnv);
            return View(hd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Hoadon hd)
        {
            if (ModelState.IsValid)
            {
                _context.Hoadons.Update(hd);
                _context.SaveChanges();
                TempData["Message"] = "Cập nhật hóa đơn thành công!";
                return RedirectToAction(nameof(DocBangHoaDon));
            }
            ViewBag.KhachHangs = new SelectList(_context.Thongtins, "Idkh", "HoTen", hd.Idkh);
            ViewBag.NhanViens = new SelectList(_context.Nhanviens, "Idnv", "HoTen", hd.Idnv);
            return View(hd);
        }

        // ========== DELETE ==========
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var hd = _context.Hoadons.Include(h => h.IdkhNavigation).Include(h => h.IdnvNavigation).FirstOrDefault(x => x.Idhd == id);
            if (hd == null) return NotFound();
            return View(hd);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hd = _context.Hoadons.Find(id);
            if (hd == null) return NotFound();

            _context.Hoadons.Remove(hd);
            _context.SaveChanges();
            TempData["Message"] = "Xóa hóa đơn thành công!";
            return RedirectToAction(nameof(DocBangHoaDon));
        }
    }
}