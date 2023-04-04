using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;

namespace WebLearning.AppUser.Controllers
{
    [Authorize]
    public class UserCourseController : Controller
    {
        private readonly ILogger<UserCourseController> _logger;

        private readonly ICourseService _courseService;

        private readonly ICourseRoleService _courseRoleService;

        public UserCourseController(ILogger<UserCourseController> logger, ICourseService courseService, ICourseRoleService courseRoleService)
        {
            _logger = logger;

            _courseService = courseService;


            _courseRoleService = courseRoleService;
        }
        [Route("/khoa-hoc.html")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,

            };
            var Roke = await _courseService.GetPaging(request);
            if (Roke == null)
            {
                return Redirect("/dang-nhap.html");
            }

            return View(Roke);
        }
        [Route("/chi-tiet-khoa-hoc/{Alias}/{id}", Name = "UserCourseDetail")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var role = await _courseRoleService.GetCourseById(id, User.Identity.Name);

            if (role == null || role.CourseDto == null)
            {
                return Redirect("/khong-tim-thay-trang.html");
            }

            return View(role);
        }

    }

}

