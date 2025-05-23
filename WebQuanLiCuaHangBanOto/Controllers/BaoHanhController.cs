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

        // ========== READ ==========
        public IActionResult DocBangBaoHanh()
        {
            var list = _context.Baohanhs.Include(b => b.IdspNavigation).ToList();
            return View(list);
        }

        // ========== CREATE ==========
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Baohanh bh)
        {
            if (ModelState.IsValid)
            {
                _context.Baohanhs.Add(bh);
                _context.SaveChanges();
                TempData["Message"] = "Thêm bảo hành thành công!";
                return RedirectToAction(nameof(DocBangBaoHanh));
            }

            return View(bh);
        }

        // ========== EDIT ==========
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bh = _context.Baohanhs.Find(id);
            if (bh == null)
                return NotFound();

            return View(bh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Baohanh bh)
        {
            if (ModelState.IsValid)
            {
                _context.Baohanhs.Update(bh);
                _context.SaveChanges();
                TempData["Message"] = "Cập nhật bảo hành thành công!";
                return RedirectToAction(nameof(DocBangBaoHanh));
            }

            return View(bh);
        }

        // ========== DELETE ==========
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var bh = _context.Baohanhs.FirstOrDefault(x => x.Idbh == id);
            if (bh == null)
                return NotFound();

            return View(bh);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var bh = _context.Baohanhs.Find(id);
            if (bh == null)
                return NotFound();

            _context.Baohanhs.Remove(bh);
            _context.SaveChanges();
            TempData["Message"] = "Xóa bảo hành thành công!";
            return RedirectToAction(nameof(DocBangBaoHanh));
        }
    }
}
