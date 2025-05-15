using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class DanhGiaController : Controller
    {
        private readonly QLCHOTOContext _context;

        public DanhGiaController(QLCHOTOContext context)
        {
            _context = context;
        }
        public IActionResult DocBangDanhGia()
        {
            _context.Danhgia.ToList();
            return View(_context.Danhgia.ToList());
        }
        public IActionResult Create(Danhgia dg)
        {
            try
            {
                _context.Danhgia.Add(dg);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message;
                Console.WriteLine("Lỗi: " + innerMessage);
                // Hoặc dùng logger, hoặc ViewBag để debug trên View
            }


            return View(dg);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            /*     if (id == null || id <= 0)
                 {
                     return BadRequest();
                 }*/

            var dg = _context.Danhgia.Find(id);
            /*     if (tt == null)
                 {
                     return NotFound();
                 }*/
            return View(dg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Danhgia dg)
        {
            //if (ModelState.IsValid)
            //{
            _context.Danhgia.Update(dg);
            _context.SaveChanges();
            TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
            return RedirectToAction(nameof(DocBangDanhGia));
            //}
            return View(DocBangDanhGia);
        }

        /// detels. 

        // ========== DELETE ==========
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            /*          if (id == null || id <= 0)
                      {
                          return BadRequest();
                      }*/

            var dg = _context.Danhgia.FirstOrDefault(x => x.Iddg == id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}
            return View(dg);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var dg = _context.Danhgia.Find(id);
            /*  if (tt == null)
              {
                  return NotFound();
              }*/

            _context.Danhgia.Remove(dg);
            _context.SaveChanges();
            TempData["Message"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(DocBangDanhGia));
        }
    }
}
