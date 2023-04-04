using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Role;

namespace WebLearning.App.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleService _roleService;
        public INotyfService _notyfService { get; }

        public RoleController(ILogger<RoleController> logger, IHttpClientFactory factory, IRoleService roleService, INotyfService notyfService)
        {
            _logger = logger;
            _roleService = roleService;
            _notyfService = notyfService;
        }
        [Route("/quyen.html")]
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
            var Roke = await _roleService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return RedirectToAction("Login", "Login");
            }
            //ViewBag.Keyword = keyword;

            return View(Roke);
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var role = await _roleService.GetRoleById(id);
            return View(role);
        }
        [HttpGet]
        [Route("/tao-moi-quyen.html")]
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-quyen.html")]
        public async Task<IActionResult> Create(CreateRoleDto createRoleDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (createRoleDto != null)
            {
                await _roleService.InsertRole(createRoleDto);
                _notyfService.Success("Tạo thành công");

                return RedirectToAction("Index", "Role");

            }
            return View(createRoleDto);
        }
        [HttpGet]
        [Route("/cap-nhat-quyen/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _roleService.GetRoleById(id);
            var updateResult = new UpdateRoleDto()
            {
                RoleName = result.RoleName,
                Description = result.Description,
            };
            return View(updateResult);
        }
        [HttpPost]
        [Route("/cap-nhat-quyen/{id}")]
        public async Task<IActionResult> Edit(UpdateRoleDto updateRoleDto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (updateRoleDto != null)
            {
                await _roleService.UpdateRole(updateRoleDto, id);
                _notyfService.Success("Cập nhật thành công");

                return RedirectToAction("Index", "Role");
            }
            return View(updateRoleDto);
        }
        [HttpGet]
        [Route("/xoa-quyen/{id}")]

        public async Task<IActionResult> Delete(RoleDto dto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var role = await _roleService.GetRoleById(id);
            dto.Id = role.Id;
            dto.RoleName = role.RoleName;
            dto.Description = role.Description;
            dto.Code = role.Code;

            return View(dto);
        }
        [HttpPost]
        [Route("/xoa-quyen/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _roleService.DeleteRole(id);
            _notyfService.Success("Xóa thành công");
            return RedirectToAction("Index", "Role");
        }
    }
}
