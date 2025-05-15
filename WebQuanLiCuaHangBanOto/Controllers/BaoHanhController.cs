using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class BaoHanhController : Controller
    {
        private readonly QLCHOTOContext _context;
        public BaoHanhController(QLCHOTOContext context)
        {
            _context = context;
        }



        public IActionResult DocBangBaoHanh()
        {
            return View(_context.Baohanhs.ToList());
        }
        [HttpGet]
        public IActionResult Create(int id)
        {
            var bh = _context.Baohanhs.Find(id);
            return View(bh);
        }
        [HttpPost]
        public IActionResult Create(Baohanh bh)
        {
            _context.Baohanhs.Add(bh);
            _context.SaveChanges();
            return View(DocBangBaoHanh);

        }






        [HttpGet]
        public IActionResult Edit(int id)
        {
            //if (id == null || id <= 0)
            //{
            //    return BadRequest();
            //}

            var bh = _context.Baohanhs.Find(id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}
            return View(bh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Baohanh bh)
        {
            //if (ModelState.IsValid)
            //{
            _context.Baohanhs.Update(bh);
            _context.SaveChanges();
            TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
            return RedirectToAction(nameof(DocBangBaoHanh));
            //}
            return View(DocBangBaoHanh);
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

            var bh = _context.Baohanhs.FirstOrDefault(x => x.Idbh == id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}
            return View(bh);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var bh = _context.Baohanhs.Find(id);
            //if (tt == null)
            //{
            //    return NotFound();
            //}

            _context.Baohanhs.Remove(bh);
            _context.SaveChanges();
            TempData["Message"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(DocBangBaoHanh));
        }
    }
}
