using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Account;

namespace WebLearning.App.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        public INotyfService _notyfService { get; }

        public AccountController(ILogger<AccountController> logger, IHttpClientFactory factory, IAccountService accountService, INotyfService notyfService, IRoleService roleService)
        {
            _logger = logger;
            _accountService = accountService;
            _notyfService = notyfService;
            _roleService = roleService;
        }
        [Route("/tai-khoan.html")]

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
            var Roke = await _accountService.GetPaging(request);
            if (Roke == null)
            {
                _notyfService.Warning("Phiên đăng nhập đã hết hạn!");
                return RedirectToAction("Login", "Login");
            }
            //ViewBag.Keyword = keyword;

            return View(Roke);
        }
        [Route("/chi-tiet-tai-khoan/{id}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var role = await _accountService.GetAccountById(id);
            return View(role);
        }
        [HttpGet]
        [Route("/tao-moi-tai-khoan.html")]
        public async Task<IActionResult> Create()
        {
            //var token = HttpContext.Session.GetString("Token");

            //if (token == null)
            //{
            //    return Redirect("/dang-nhap.html");
            //}
            var allRole = await _roleService.GetAllRoles();
            ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName");
            return View();
        }
        [HttpPost]
        [Route("/tao-moi-tai-khoan.html")]
        public async Task<IActionResult> Create(CreateAccountDto createAccountDto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (createAccountDto != null)
            {
                var allRole = await _roleService.GetAllRoles();
                ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName", createAccountDto.RoleId);

                var checkExist = await _accountService.GetAccount();

                foreach (var item in checkExist)
                {
                    if (item.Email.Equals(createAccountDto.Email))
                    {
                        _notyfService.Error("Đã tài khoản đã tồn tại!");

                        return RedirectToAction("Index", "Account");
                    }
                }
                await _accountService.InsertAccount(createAccountDto);

                _notyfService.Success("Tạo thành công");

                return RedirectToAction("Index", "Account");

            }
            return View(createAccountDto);
        }
        [HttpGet]
        [Route("/cap-nhat-tai-khoan/{id}")]

        public async Task<IActionResult> Edit(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _accountService.GetAccountById(id);
            var allRole = await _roleService.GetAllRoles();
            ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName");
            var updateResult = new UpdateAccountDto()
            {
                Email = result.Email,
                Password = result.Password,
                PasswordHased = result.PasswordHased,
                Active = result.Active,
                RoleId = result.RoleId,
                DateCreated = result.DateCreated,
                LastLogin = result.LastLogin,
                FullName = result.accountDetailDto.FullName,
                Phone = result.accountDetailDto.Phone,
                Address = result.accountDetailDto.Address,

            };
            return View(updateResult);
        }
        [HttpPost]
        [Route("/cap-nhat-tai-khoan/{id}")]
        public async Task<IActionResult> Edit(UpdateAccountDto updateAccountDto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (updateAccountDto != null)
            {
                var allRole = await _roleService.GetAllRoles();
                ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName");
                await _accountService.UpdateAccount(updateAccountDto, id);
                _notyfService.Success("Cập nhật thành công");

                return RedirectToAction("Index", "Account");
            }
            return View(updateAccountDto);
        }
        [HttpGet]
        [Route("/xoa-tai-khoan/{id}")]

        public async Task<IActionResult> Delete(AccountDto dto, Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var role = await _accountService.GetAccountById(id);
            var allRole = await _roleService.GetAllRoles();
            ViewData["DanhMuc"] = new SelectList(allRole, "Id", "RoleName");
            dto.Id = role.Id;
            dto.Email = role.Email;
            dto.Password = role.Password;
            dto.PasswordHased = role.PasswordHased;
            dto.DateCreated = role.DateCreated;
            dto.LastLogin = role.LastLogin;
            dto.RoleId = role.RoleId;
            dto.Active = role.Active;
            dto.accountDetailDto = role.accountDetailDto;
            dto.roleDto = role.roleDto;
            dto.Code = role.Code;
            return View(dto);
        }
        [HttpPost]
        [Route("/xoa-tai-khoan/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            await _accountService.DeleteAccount(id);
            _notyfService.Success("Xóa thành công");
            return RedirectToAction("Index", "Account");
        }
    }
}
