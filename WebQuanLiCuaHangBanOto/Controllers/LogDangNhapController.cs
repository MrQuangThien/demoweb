    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WebQuanLiCuaHangBanOto.Models;

    namespace WebQuanLiCuaHangBanOto.Controllers
    {
        public class LogDangNhapController : Controller
        {
            private readonly QLCHOTOContext _context;

            public LogDangNhapController(QLCHOTOContext context)
            {
                _context = context;
            }
            public IActionResult DocBangLogDangNhap()
            {
                _context.LogDangnhaps.ToList();
                return View(_context.LogDangnhaps.ToList());
            }
            public IActionResult Create(LogDangnhap dn)
            {
           
                try
                {
                    _context.LogDangnhaps.Add(dn);
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    var innerMessage = ex.InnerException?.Message;
                    Console.WriteLine("Lỗi: " + innerMessage);
                    // Hoặc dùng logger, hoặc ViewBag để debug trên View
                }
            
                return View(dn);
            }
            [HttpGet]
            public IActionResult Edit(int id)
            {
                //if (id == null || id <= 0)
                //{
                //    return BadRequest();
                //}

                var tt = _context.LogDangnhaps.Find(id);
                //if (tt == null)
                //{
                //    return NotFound();
                //}
                return View(tt);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(LogDangnhap dn)
            {
                //if (ModelState.IsValid)
                //{
                _context.LogDangnhaps.Update(dn);
                _context.SaveChanges();
                TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
                return RedirectToAction(nameof(DocBangLogDangNhap));
                //}
                return View(DocBangLogDangNhap);
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

                var dn = _context.LogDangnhaps.FirstOrDefault(x => x.Idlog == id);
                //if (tt == null)
                //{
                //    return NotFound();
                //}
                return View(dn);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                var dn = _context.LogDangnhaps.Find(id);
                //if (dn == null)
                //{
                //    return NotFound();
                //}

                _context.LogDangnhaps.Remove(dn);
                _context.SaveChanges();
                TempData["Message"] = "Xóa khách hàng thành công!";
                return RedirectToAction(nameof(DocBangLogDangNhap));
            }
        }
    }
