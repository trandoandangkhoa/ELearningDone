using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebLearning.AppUser.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
    }
}
