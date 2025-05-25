using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class ThongTinController : Controller
    {
        private readonly QLCHOTOContext _context;

        public ThongTinController(QLCHOTOContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ========== HIỂN THỊ DANH SÁCH KHÁCH HÀNG ==========
        public async Task<IActionResult> DocBangThongTin(string searchString)
        {
            var query = _context.Thongtins.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(t =>
                    t.HoTen.Contains(searchString) ||
                    t.Sdt.Contains(searchString) ||
                    t.DiaChi.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
            var result = await query.ToListAsync(); // ✅ đúng chỗ cần await
            return View(result);
        }


        // ========== CREATE ==========
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Bắt buộc phải có để hiển thị form tạo mới
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Thongtin tt)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine("Model error: " + error);
                }
                return View(tt);
            }

            try
            {
                _context.Thongtins.Add(tt);
                _context.SaveChanges();
                TempData["Message"] = "Thêm khách hàng thành công!";
                return RedirectToAction(nameof(DocBangThongTin));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("DB Exception: " + ex.InnerException?.Message);
                ModelState.AddModelError("", "Lỗi lưu dữ liệu: " + ex.InnerException?.Message);
                return View(tt);
            }
        }

        // ========== EDIT ==========
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tt = _context.Thongtins.Find(id);
            if (tt == null)
                return NotFound();

            return View(tt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Thongtin tt)
        {
            if (!ModelState.IsValid)
                return View(tt);

            _context.Thongtins.Update(tt);
            _context.SaveChanges();
            TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
            return RedirectToAction(nameof(DocBangThongTin));
        }

        // ========== DELETE ==========
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var tt = _context.Thongtins.FirstOrDefault(x => x.Idkh == id);
            if (tt == null)
                return NotFound();

            return View(tt);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tt = _context.Thongtins.Find(id);
            if (tt == null)
                return NotFound();

            _context.Thongtins.Remove(tt);
            _context.SaveChanges();
            TempData["Message"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(DocBangThongTin));
        }
    }
}
