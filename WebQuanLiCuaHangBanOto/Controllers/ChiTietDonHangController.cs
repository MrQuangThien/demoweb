using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebQuanLiCuaHangBanOto.Models;

public class ChitietdonhangController : Controller
{
    private readonly QLCHOTOContext _context;

    public ChitietdonhangController(QLCHOTOContext context)
    {
        _context = context;
    }

    public IActionResult DocBangChiTietDonHang()
    {
        _context.Chitietdonhangs.ToList();
        return View(_context.Chitietdonhangs.ToList());
    }
    public IActionResult Create(Chitietdonhang ctd)
    {
        _context.Chitietdonhangs.Add(ctd);
        _context.SaveChanges();
        return View(ctd);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        //if (id == null || id <= 0)
        //{
        //    return BadRequest();
        //}

        var ctd = _context.Chitietdonhangs.Find(id);
        //if (tt == null)
        //{
        //    return NotFound();
        //}
        return View(ctd);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Chitietdonhang ctd)
    {
        //if (ModelState.IsValid)
        //{
        _context.Chitietdonhangs.Update(ctd);
        _context.SaveChanges();
        TempData["Message"] = "Cập nhật thông tin khách hàng thành công!";
        return RedirectToAction(nameof(DocBangChiTietDonHang));
        //}
        return View(DocBangChiTietDonHang);
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

        var ctd = _context.Chitietdonhangs.FirstOrDefault(x => x.Idctdh == id);
        //if (tt == null)
        //{
        //    return NotFound();
        //}
        return View(ctd);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var ctd = _context.Chitietdonhangs.Find(id);
        //if (tt == null)
        //{
        //    return NotFound();
        //}

        _context.Chitietdonhangs.Remove(ctd);
        _context.SaveChanges();
        TempData["Message"] = "Xóa khách hàng thành công!";
        return RedirectToAction(nameof(DocBangChiTietDonHang));
    }
}
