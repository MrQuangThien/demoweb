using Microsoft.AspNetCore.Mvc;
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
        public IActionResult DocBangTaiKhoan( )
        {
            
            return View(_context.Taikhoans.ToList());
        }
        public IActionResult Create(Taikhoan tk)
        {
            try
            {
                _context.Taikhoans.Add(tk);
                _context.SaveChanges();
                // Sau khi thêm xong, chuyển hướng về action đọc bảng tài khoản (không return View lỗi nữa)
                return RedirectToAction(nameof(DocBangTaiKhoan));
            }
            catch (DbUpdateException ex)
            {
                // Ghi lỗi chi tiết ra Console hoặc Log
                Console.WriteLine(ex.InnerException?.Message);
                // Hoặc bạn có thể thêm ModelState error để hiện lỗi lên View
                ModelState.AddModelError("", "Không thể thêm tài khoản mới: " + ex.InnerException?.Message);

                return View(tk); // Trả lại view cùng dữ liệu tk vừa nhập để người dùng sửa
            }
        }


        // ========== EDIT ==========
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //if (id == null || id <= 0)
            //{
            //    return BadRequest();
            //}

            var tk = _context.Taikhoans.Find(id);
            //if (tk == null)
            //{
            //    return NotFound();
            //}
            return View(tk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Taikhoan tk)
        {
            if (ModelState.IsValid)
            {
                _context.Taikhoans.Update(tk);
                _context.SaveChanges();
                TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
                return RedirectToAction(nameof(DocBangTaiKhoan));
            }
            return View(DocBangTaiKhoan);
        }

        /// detels. 

        // ========== DELETE ==========
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id == null || id <= 0)
            //{
            //    return BadRequest();
            //}

            var tk = _context.Taikhoans.FirstOrDefault(x => x.Idkh == id);
            //if (tk == null)
            //{
            //    return NotFound();
            //}
            return View(tk);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tk = _context.Taikhoans.Find(id);
            //if (tk == null)
            //{
            //    return NotFound();
            //}

            _context.Taikhoans.Remove(tk);
            _context.SaveChanges();
            TempData["Message"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(DocBangTaiKhoan));
        }

    }
}
