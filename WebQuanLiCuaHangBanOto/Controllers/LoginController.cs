using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using WebQuanLiCuaHangBanOto.Models;

public class LoginController : Controller
{
    private readonly QLCHOTOContext _context;
    private readonly IConfiguration _config;

    public LoginController(QLCHOTOContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

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

            var taikhoan = new Taikhoan
            {
                Idkh = thongtin.Idkh,
                TenTk = model.TenTk,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau),
                Gmail = model.Gmail,
                Role = model.Role ?? "user"
            };

            _context.Taikhoans.Add(taikhoan);
            _context.SaveChanges();

            _context.LogDangnhaps.Add(new LogDangnhap
            {
                Idtk = taikhoan.Idtk,
                ThoiGian = DateTime.Now,
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                ThanhCong = true
            });

            _context.SaveChanges();

            TempData["Message"] = "Đăng ký thành công!";
            return RedirectToAction("Index");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Lỗi lưu dữ liệu: " + ex.InnerException?.Message);
            return View(model);
        }
    }

    public IActionResult Index()
    {
        if (TempData["Message"] != null)
        {
            ViewBag.Message = TempData["Message"];
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _context.Taikhoans.FirstOrDefaultAsync(u => u.TenTk == model.Username);

        var log = new LogDangnhap
        {
            ThoiGian = DateTime.Now,
            Ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
            ThanhCong = false
        };

        if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.MatKhau))
        {
            log.ThanhCong = true;
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

            return user.Role == "admin"
                ? RedirectToAction("viewadmin", "Home")
                : RedirectToAction("index", "Home");
        }

        if (user != null)
        {
            log.Idtk = user.Idtk;
            _context.LogDangnhaps.Add(log);
            await _context.SaveChangesAsync();
        }

        ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }

    public IActionResult AccessDenied() => View();

    [HttpGet]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ForgotPassword(ForgotPasswordModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var account = _context.Taikhoans.FirstOrDefault(t => t.Gmail == model.Gmail);
        if (account == null)
        {
            ModelState.AddModelError("", "Không tìm thấy tài khoản.");
            return View();
        }

        if (account.TokenExpiry != null && account.TokenExpiry > DateTime.Now.AddMinutes(-5))
        {
            ViewBag.Message = "Email đã được gửi gần đây. Vui lòng kiểm tra hộp thư hoặc thử lại sau ít phút.";
            return View();
        }

        string token = Guid.NewGuid().ToString();
        account.ResetToken = token;
        account.TokenExpiry = DateTime.Now.AddMinutes(30);
        _context.SaveChanges();

        string link = Url.Action("ResetPassword", "Login", new { token = token }, Request.Scheme);
        SendResetPasswordEmail(account.Gmail, link);

        ViewBag.Message = "Đã gửi link đặt lại mật khẩu qua email.";
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        var tk = _context.Taikhoans.FirstOrDefault(t => t.ResetToken == token && t.TokenExpiry > DateTime.Now);
        if (tk == null) return NotFound("Link không hợp lệ hoặc đã hết hạn");

        return View(new ResetPasswordModel { Token = token });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var tk = _context.Taikhoans.FirstOrDefault(t => t.ResetToken == model.Token && t.TokenExpiry > DateTime.Now);
        if (tk == null) return NotFound("Token không hợp lệ hoặc đã hết hạn");

        tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        tk.ResetToken = null;
        tk.TokenExpiry = null;

        _context.SaveChanges();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, tk.TenTk),
            new Claim(ClaimTypes.Role, tk.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        TempData["Message"] = "Mật khẩu đã được đặt lại và bạn đã được đăng nhập.";
        return RedirectToAction("Index");
    }

    private void SendResetPasswordEmail(string toEmail, string link)
    {
        var fromEmail = _config["EmailSettings:FromEmail"];
        var appPassword = _config["EmailSettings:AppPassword"];

        var from = new MailAddress(fromEmail, "Admin Hệ thống");
        var to = new MailAddress(toEmail);
        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(fromEmail, appPassword)
        };

        using (var message = new MailMessage(from, to)
        {
            Subject = "Yêu cầu đặt lại mật khẩu",
            Body = $"Xin chào,\n\nBạn đã yêu cầu đặt lại mật khẩu. Click vào link bên dưới để đặt lại:\n{link}\n\nNếu không phải bạn, hãy bỏ qua email này.\n\nTrân trọng.",
            IsBodyHtml = false
        })
        {
            smtp.Send(message);
        }
    }

    [HttpGet]
    [Route("Login/GeneratePasswordHash")]
    public IActionResult GeneratePasswordHash(string password = "")
    {
        if (string.IsNullOrWhiteSpace(password))
            return Content("❌ Vui lòng truyền mật khẩu bằng cách thêm ?password=giatri vào URL");

        try
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            return Content($"✅ Mật khẩu gốc: {password}\n\n🔐 Mã hóa:\n{hash}");
        }
        catch (Exception ex)
        {
            return Content("❌ Lỗi: " + ex.Message);
        }
    }


}
