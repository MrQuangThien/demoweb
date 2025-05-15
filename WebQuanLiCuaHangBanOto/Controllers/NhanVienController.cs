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
        public IActionResult DocBangNhanVien()
        {


            return View(_context.Nhanviens.ToList());
        }

        public IActionResult Create(Nhanvien nv)
        {
            try
            {
                _context.Nhanviens.Add(nv);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message;
                Console.WriteLine("Lỗi: " + innerMessage);
                // Hoặc dùng logger, hoặc ViewBag để debug trên View
            }


            return View(DocBangNhanVien);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //if (id == null || id <= 0)
            //{
            //    return BadRequest();
            //}

            var tt = _context.Nhanviens.Find(id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}
            return View(tt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Nhanvien nv)
        {
            if (ModelState.IsValid)
            {
                _context.Nhanviens.Update(nv);
                _context.SaveChanges();
                TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
                return RedirectToAction(nameof(DocBangNhanVien));
            }
            return View(DocBangNhanVien);
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

            var nv = _context.Nhanviens.FirstOrDefault(x => x.Idnv == id);
            //if (nv == null)
            //{
            //    return NotFound();
            //}
            return View(nv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var nv = _context.Nhanviens.Find(id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}

            _context.Nhanviens.Remove(nv);
            _context.SaveChanges();
            TempData["Message"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(DocBangNhanVien));
        }
    }
}
