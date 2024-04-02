using Microsoft.AspNetCore.Mvc;

namespace WEB_21CDP2.Controllers
{
    public class Group : Controller
    {
        public IActionResult Trangchu()
        {
            return View();
        }

        public IActionResult Giaodich() {
            return View();
        }
        public IActionResult PageTest1() 
        {
            return View();
        }
    }
}
