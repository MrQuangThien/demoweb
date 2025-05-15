using Microsoft.AspNetCore.Mvc;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class ThongBaoConTtroller : Controller
    {
        private readonly QLCHOTOContext _context;

        public ThongBaoConTtroller(QLCHOTOContext context)
        {
            _context = context;
        }
        public ActionResult DocBangThongBao()
        {
            return View(_context.ThongBaos.ToList());
        }

        public ActionResult Create(ThongBao tb)
        {


            _context.ThongBaos.Add(tb);
            _context.SaveChanges();

            return View(tb);
        }

    }

}
