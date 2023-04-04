using Microsoft.AspNetCore.Mvc;

namespace WebLearning.AppUser.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult HeaderNotify()
        {
            return ViewComponent("HeaderNotify");
        }
        public IActionResult NumberNotify()
        {
            return ViewComponent("NumberNotify");

        }
    }
}
