using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CourseRole;

namespace WebLearning.App.Controllers
{
    [Authorize]

    public class CourseRoleController : Controller
    {
        private readonly ILogger<CourseRoleController> _logger;
        private readonly ICourseRoleService _CourseRoleService;
        private readonly ICourseService _courseService;
        private readonly IRoleService _roleService;
        public INotyfService _notyfService { get; }

        public CourseRoleController(ILogger<CourseRoleController> logger, IHttpClientFactory factory, ICourseRoleService CourseRoleService, INotyfService notyfService, ICourseService courseService, IRoleService roleService)
        {
            _logger = logger;
            _CourseRoleService = CourseRoleService;
            _notyfService = notyfService;
            _courseService = courseService;
            _roleService = roleService;
        }
        [Route("/quyen-khoa-hoc.html")]
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
            var Roke = await _CourseRoleService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return RedirectToAction("Login", "Login");
            }
            //ViewBag.Keyword = Keyword;

            return View(Roke);
        }
        [Route("/chi-tiet-quyen-khoa-hoc.html")]

        public async Task<IActionResult> Detail(Guid id, Guid roleId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var CourseRole = await _CourseRoleService.AdminGetCourseRole(id, roleId);
            return View(CourseRole);
        }
        [HttpGet]
        [Route("/tao-moi-quyen-khoa-hoc.html")]

        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var allRole = await _roleService.GetAllRoles();
            var allCourse = await _courseService.GetAllCourse();
            ViewData["Course"] = new SelectList(allCourse, "Id", "Name");
            ViewData["Role"] = new SelectList(allRole, "Id", "RoleName");
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-quyen-khoa-hoc.html")]
        public async Task<IActionResult> Create(CreateCourseRoleDto createCourseRoleDto)
        {

            if (createCourseRoleDto != null)
            {
                var roleId = await _roleService.GetRoleById(createCourseRoleDto.RoleId);

                createCourseRoleDto.Id = Guid.NewGuid();

                createCourseRoleDto.RoleName = roleId.RoleName;

                var token = HttpContext.Session.GetString("Token");

                if (token == null)
                {
                    return Redirect("/dang-nhap.html");
                }

                await _CourseRoleService.InsertCourseRole(createCourseRoleDto);


                _notyfService.Success("Tạo thành công");

                return RedirectToAction("Index", "CourseRole");

            }
            return View(createCourseRoleDto);
        }

        [HttpGet]
        [Route("/xoa-quyen-khoa-hoc/{id}/{roleId}")]

        public async Task<IActionResult> Delete(CourseRoleDto dto, Guid id, Guid roleId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var CourseRole = await _CourseRoleService.AdminGetCourseRole(id, roleId);
            dto.Id = CourseRole.Id;
            dto.CourseId = CourseRole.CourseId;
            dto.RoleId = CourseRole.RoleId;
            dto.RoleName = CourseRole.RoleName;
            dto.CourseDto = CourseRole.CourseDto;
            dto.RoleDto = CourseRole.RoleDto;
            dto.Code = CourseRole.Code;
            return View(dto);
        }
        [HttpPost]
        [Route("/xoa-quyen-khoa-hoc/{id}/{roleId}")]

        public async Task<IActionResult> Delete(Guid id, Guid roldId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _CourseRoleService.DeleteCourseRole(id, roldId);
            _notyfService.Success("Xóa thành công");
            return RedirectToAction("Index", "CourseRole");
        }
    }
}
