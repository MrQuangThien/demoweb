using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly QLCHOTOContext _context;

        public NhanVienController(QLCHOTOContext context)
        {
            _context = context;
        }

        // ========== READ ==========
        public IActionResult DocBangNhanVien()
        {
            var list = _context.Nhanviens.ToList();
            return View(list);
        }

        // ========== CREATE ==========

        [HttpGet]
        public IActionResult Create()
        {
            return View(); // hiển thị form trống
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Nhanvien nv)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Nhanviens.Add(nv);
                    _context.SaveChanges();

                    TempData["Message"] = "Thêm nhân viên thành công!";
                    return RedirectToAction(nameof(DocBangNhanVien)); // ✅ đúng kiểu
                }
                catch (DbUpdateException ex)
                {
                    var innerMessage = ex.InnerException?.Message;
                    ModelState.AddModelError("", "Lỗi khi thêm nhân viên: " + innerMessage);
                }
            }

            return View(nv); // Trả lại view kèm dữ liệu nếu lỗi
        }

        // ========== EDIT ==========

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var nv = _context.Nhanviens.Find(id);
            if (nv == null)
                return NotFound();

            return View(nv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Nhanvien nv)
        {
            if (ModelState.IsValid)
            {
                _context.Nhanviens.Update(nv);
                _context.SaveChanges();

                TempData["Message"] = "Cập nhật thông tin nhân viên thành công!";
                return RedirectToAction(nameof(DocBangNhanVien));
            }

            return View(nv); // ✅ sửa lại: trả đúng model, không phải method
        }

        // ========== DELETE ==========

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var nv = _context.Nhanviens.FirstOrDefault(x => x.Idnv == id);
            if (nv == null)
                return NotFound();

            return View(nv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var nv = _context.Nhanviens.Find(id);
            if (nv == null)
                return NotFound();

            _context.Nhanviens.Remove(nv);
            _context.SaveChanges();

            TempData["Message"] = "Xóa nhân viên thành công!";
            return RedirectToAction(nameof(DocBangNhanVien));
        }
    }
}
