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
    }
}
