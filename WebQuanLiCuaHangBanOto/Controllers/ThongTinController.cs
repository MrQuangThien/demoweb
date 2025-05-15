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
        public IActionResult DocBangThongTin()
        {

            return View(_context.Thongtins.ToList());
        }

        // ========== CREATE ==========
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Thongtin tt)
        {
            //if (ModelState.IsValid)
            //{
            try
            { _context.Thongtins.Add(tt);
            _context.SaveChanges();
                
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message;
                Console.WriteLine("Lỗi: " + innerMessage);
                // Hoặc dùng logger, hoặc ViewBag để debug trên View
            }

            _context.Thongtins.Add(tt);
            _context.SaveChanges();
            TempData["Message"] = "Tạo mới thông tin khách hàng thành công!";
            return RedirectToAction(nameof(DocBangThongTin));
            //}
            return View(DocBangThongTin);
        }

        // ========== EDIT ==========
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //if (id == null || id <= 0)
            //{
            //    return BadRequest();
            //}

            var tt = _context.Thongtins.Find(id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}
            return View(tt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Thongtin tt)
        {
            //if (ModelState.IsValid)
            //{
            _context.Thongtins.Update(tt);
            _context.SaveChanges();
            TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
            return RedirectToAction(nameof(DocBangThongTin));
            //}
            return View(DocBangThongTin);
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

            var tt = _context.Thongtins.FirstOrDefault(x => x.Idkh == id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}
            return View(tt);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tt = _context.Thongtins.Find(id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}

            _context.Thongtins.Remove(tt);
            _context.SaveChanges();
            TempData["Message"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(DocBangThongTin));
        }
    }
}
