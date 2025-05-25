using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly QLCHOTOContext _context;

        public TaiKhoanController(QLCHOTOContext context)
        {
            _context = context;
        }

        // READ
        public IActionResult DocBangTaiKhoan()
        {
            var danhSach = _context.Taikhoans.Include(t => t.IdkhNavigation).ToList();
            return View(danhSach);
        }

        // CREATE
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.KhachHangList = new SelectList(_context.Thongtins, "Idkh", "HoTen");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Taikhoan tk)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // ✅ Mã hóa mật khẩu trước khi lưu
                    tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(tk.MatKhau);

                    _context.Taikhoans.Add(tk);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(DocBangTaiKhoan));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu vào CSDL: " + ex.InnerException?.Message);
                }
            }

            ViewBag.KhachHangList = new SelectList(_context.Thongtins, "Idkh", "HoTen", tk.Idkh);
            return View(tk);
        }

        // GET: TaiKhoan/Edit/5
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var tk = _context.Taikhoans.Find(id);
            if (tk == null) return NotFound();

            // Xóa giá trị mật khẩu trước khi truyền vào View
            tk.MatKhau = string.Empty;

            ViewBag.KhachHangList = new SelectList(_context.Thongtins, "Idkh", "HoTen", tk.Idkh);
            return View(tk);
        }


        // POST: TaiKhoan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Taikhoan tk)
        {
            if (id != tk.Idtk) return NotFound();

            var tkDb = _context.Taikhoans.FirstOrDefault(t => t.Idtk == id);
            if (tkDb == null) return NotFound();

            if (ModelState.IsValid)
            {
                tkDb.Idkh = tk.Idkh;
                tkDb.TenTk = tk.TenTk;
                tkDb.Gmail = tk.Gmail;
                tkDb.Role = tk.Role;

                // ✅ Mã hóa mật khẩu mới nếu có nhập
                if (!string.IsNullOrWhiteSpace(tk.MatKhau))
                {
                    tkDb.MatKhau = BCrypt.Net.BCrypt.HashPassword(tk.MatKhau);
                }

                _context.Update(tkDb);
                _context.SaveChanges();

                return RedirectToAction(nameof(DocBangTaiKhoan));
            }

            ViewBag.KhachHangList = new SelectList(_context.Thongtins, "Idkh", "HoTen", tk.Idkh);
            return View(tk);
        }


        // GET: TaiKhoan/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var tk = _context.Taikhoans.Include(t => t.IdkhNavigation).FirstOrDefault(t => t.Idtk == id);
            if (tk == null) return NotFound();

            return View(tk);
        }

        // POST: TaiKhoan/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tk = _context.Taikhoans.Find(id);
            if (tk == null) return NotFound();

            _context.Taikhoans.Remove(tk);
            _context.SaveChanges();
            return RedirectToAction(nameof(DocBangTaiKhoan));
        }

    }

}
