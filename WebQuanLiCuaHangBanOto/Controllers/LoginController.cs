using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using WebQuanLiCuaHangBanOto.Models;
using System.Linq;

public class LoginController : Controller
{
    private readonly QLCHOTOContext _context;

    public LoginController(QLCHOTOContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            foreach (var error in errors)
            {
                Console.WriteLine("Model error: " + error);
            }
            return View(model);
        }

        try
        {
            var thongtin = new Thongtin
            {
                HoTen = model.HoTen,
                NgaySinh = model.NgaySinh,
                GioiTinh = model.GioiTinh,
                Sdt = model.SDT,
                DiaChi = model.DiaChi,
                Gmail = model.Gmail
            };

            _context.Thongtins.Add(thongtin);
            _context.SaveChanges();
            Console.WriteLine("ThongTin IDKH = " + thongtin.Idkh);


            var taikhoan = new Taikhoan
            {
                Idkh = thongtin.Idkh,
                TenTk = model.TenTk,
                MatKhau = model.MatKhau,
                Gmail = model.Gmail,
                Role = model.Role ?? "user"
            };

            _context.Taikhoans.Add(taikhoan);
            _context.SaveChanges();
            Console.WriteLine("ThongTin IDKH = " + thongtin.Idkh);

            var log = new LogDangnhap
            {
                Idtk = taikhoan.Idtk,
                ThoiGian = DateTime.Now,
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                ThanhCong = true
            };

            _context.LogDangnhaps.Add(log);
            _context.SaveChanges();
            Console.WriteLine("ThongTin IDKH = " + thongtin.Idkh);

            TempData["Message"] = "Đăng ký thành công!";
            return RedirectToAction("Index", "Login");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("DB Exception: " + ex.InnerException?.Message);
            ModelState.AddModelError("", "Lỗi lưu dữ liệu: " + ex.InnerException?.Message);
            return View(model);
        }
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _context.Taikhoans
            .FirstOrDefaultAsync(u => u.TenTk == model.Username && u.MatKhau == model.Password);

        var log = new LogDangnhap
        {
            ThoiGian = DateTime.Now,
            Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
            ThanhCong = user != null
        };

        if (user != null)
        {
            log.Idtk = user.Idtk;
            _context.LogDangnhaps.Add(log);
            await _context.SaveChangesAsync();


            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.TenTk),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (user.Role == "admin")
                return RedirectToAction("viewadmin", "Home");
            else
                return RedirectToAction("viewindex", "Home");
        }
        else
        {
            var tk = await _context.Taikhoans.FirstOrDefaultAsync(t => t.TenTk == model.Username);
            if (tk != null)
            {
                log.Idtk = tk.Idtk;
                _context.LogDangnhaps.Add(log);
                await _context.SaveChangesAsync();
            }

            ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
