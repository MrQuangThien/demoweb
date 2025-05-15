using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class ThongkeSlbanController : Controller
    {
        private readonly QLCHOTOContext _context;

        public ThongkeSlbanController(QLCHOTOContext context)
        {
            _context = context;
        }

        // ========== HIỂN THỊ DANH SÁCH ==========
        public async Task<IActionResult> DocBangThongkeSLban()
        {
            var danhSach = await _context.ThongkeSlbans
                                         .Include(t => t.IdspNavigation)
                                         .ToListAsync();
            return View(danhSach);
        }
        /////create 
        ///
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ThongkeSlban tkb)
        {

            try
            {
                   _context.ThongkeSlbans.Add(tkb);
            _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message;
                Console.WriteLine("Lỗi: " + innerMessage);
                // Hoặc dùng logger, hoặc ViewBag để debug trên View
            }

            
            TempData["Message"] = "Tạo mới thông tin khách hàng thành công!";
            return RedirectToAction(nameof(DocBangThongkeSLban));

        }

        // ========== EDIT ==========
        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return BadRequest();
        //    }

        //    var tkb = _context.ThongkeSlbans.Find(id);
        //    if (tkb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(tkb);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ThongkeSlban tkb)
        {
            if (ModelState.IsValid)
            {
                _context.ThongkeSlbans.Update(tkb);
                _context.SaveChanges();
                TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
                return RedirectToAction(nameof(DocBangThongkeSLban));
            }
            return View(DocBangThongkeSLban);
        }

        /// detels. 

        // ========== DELETE ==========
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id == null || id == 0)
            //{
            //    return BadRequest();
            //}

            var tkb = _context.ThongkeSlbans.FirstOrDefault(x => x.Idsp == id);
            //if (tkb == null)
            //{
            //    return NotFound();
            //}
            return View(tkb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tkb = _context.ThongkeSlbans.Find(id);
            //if (tkb == null)
            //{
            //    return NotFound();
            //}

            _context.ThongkeSlbans.Remove(tkb);
            _context.SaveChanges();
            TempData["Message"] = "Xóa khách hàng thành công!";
            return RedirectToAction(nameof(DocBangThongkeSLban));
        }


    }

}
