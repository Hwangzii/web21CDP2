using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEB_21CDP2.Areas.Identity.Data;
using WEB_21CDP2.Models;

namespace WEB_21CDP2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
        }

        // Phương thức xử lý yêu cầu trang chính
        public IActionResult Index()
        {
            // Lấy UserID của người dùng hiện tại và gán vào ViewData
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            return View();
        }

        // Phương thức xử lý yêu cầu trang quyền riêng tư
        public IActionResult Privacy()
        {
            return View();
        }

        // Phương thức xử lý yêu cầu lỗi
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Hiển thị trang lỗi với thông tin chi tiết về lỗi
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
