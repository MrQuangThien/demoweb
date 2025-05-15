using Microsoft.AspNetCore.Mvc;
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
        public IActionResult DocBangHoaDon()
        {
            var danhSach = _context.Hoadons.ToList();
            return View(danhSach);
        }


        public IActionResult Create(Hoadon hd)
        {
            try
            {
                _context.Hoadons.Add(hd);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message;
                Console.WriteLine("Lỗi: " + innerMessage);
                // Hoặc dùng logger, hoặc ViewBag để debug trên View
            }

        
            return View(hd);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //if (id == null || id <= 0)
            //{
            //    return BadRequest();
            //}

            var hd= _context.Hoadons.Find(id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}
            return View(hd);
        }   

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Hoadon hd)
        {
            //if (ModelState.IsValid)
            //{
            _context.Hoadons.Update(hd);
            _context.SaveChanges();
            TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
            return RedirectToAction(nameof(DocBangHoaDon));
            //}
            return View(DocBangHoaDon);
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

            var hd = _context.Hoadons.FirstOrDefault(x => x.Idhd == id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}
            return View(hd);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hd = _context.Hoadons.Find(id);
            //if (hd == null)
            //{
            //    return NotFound();
            //}

            _context.Hoadons.Remove(hd);
            _context.SaveChanges();
            TempData["Message"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(DocBangHoaDon));
        }
    }
}
