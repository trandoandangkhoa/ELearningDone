﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Avatar;
using WebLearning.Contract.Dtos.Login;

namespace WebLearning.AppUser.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginService _loginService;
        private readonly IAccountService _accountService;

        private readonly INotyfService _notyf;

        public LoginController(ILogger<LoginController> logger, ILoginService loginService, IConfiguration configuration, IAccountService accountService
                                , INotyfService notyfService)
        {
            _logger = logger;
            _loginService = loginService;
            _configuration = configuration;
            _accountService = accountService;
            _notyf = notyfService;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("/dang-nhap.html")]
        public IActionResult Login(LoginDto loginDto, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(loginDto);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAccount(LoginDto loginDto, string returnUrl = null)
        {
            var provider = new EphemeralDataProtectionProvider();

            if (!ModelState.IsValid) return View(ModelState);

            var token = await _loginService.Authenicate(loginDto);

            if (token == "Tài khoản không đúng")
            {
                TempData["Error"] = "Tài khoản không đúng! ";

                return Redirect("/dang-nhap.html");
            }

            var account = await _accountService.GetFullName(loginDto.UserName);

            var imagePath = await _accountService.GetAvatrImagePath(loginDto.UserName);

            string _url = null;
            if (imagePath != "NotFound")
            {
                _url = $"{_configuration["BaseAddress"]}/avatarImage/{imagePath}";

            }
            else
            {
                _url = $"{_configuration["BaseAddress"]}/avatarImage/user.png";

            }

            var claim = new List<Claim>
                {
                new Claim(ClaimTypes.Email,loginDto.UserName),
                new Claim(ClaimTypes.GivenName,loginDto.UserName),
                new Claim(ClaimTypes.Name,loginDto.UserName),
                new Claim("Avatar", _url),
                new Claim("Role", account.AuthorizeRole.ToString()),

                new Claim("FullName",account.accountDetailDto.FullName),
            };
            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

            var principle = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties();

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, props).Wait();

            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("AccountId", account.Id.ToString());
            _notyf.Success("Đăng nhập thành công!");

            return RedirectToLocal(returnUrl);
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

        }
        [HttpPost]
        [Route("/dang-xuat.html")]
        [AllowAnonymous]
        public async Task<IActionResult> LogOutAccount(LoginDto loginDto)
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");

            _notyf.Success("Đăng xuất thành công!");

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Route("/upload-anh-dai-dien.html")]
        public IActionResult UploadAvatar()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpGet]
        [Route("/thay-doi-mat-khau.html")]
        public IActionResult ChangePassword()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpPost]
        [Route("/thay-doi-mat-khau.html")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var id = HttpContext.Session.GetString("AccountId");
            var res = await _accountService.ChangePassword(Guid.Parse(id), changePassword);

            if (res != "Cập nhật thành công!")
            {
                _notyf.Error(res);
                return Redirect("/thay-doi-mat-khau.html");

            }
            _notyf.Success("Cập nhật thành công");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/dang-nhap.html");
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 700000000)]
        [RequestSizeLimit(700000000)]
        [Consumes("multipart/form-data")]
        [Route("/upload-anh-dai-dien.html")]
        public async Task<IActionResult> UploadAvatar(Guid accountId, [FromForm] CreateAvatarDto createAvatarDto, IFormFile fThumb)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var account = await _accountService.GetAccountByEmail(User.Identity.Name);

            if (account == null)
            {
                return RedirectToAction("Login", "Login");
            }
            accountId = account.AccountDto.Id;

            createAvatarDto.Name = (await _accountService.GetAccountById(accountId)).accountDetailDto.FullName;

            createAvatarDto.AccountId = accountId;

            createAvatarDto.Image = fThumb;

            var result = await _accountService.AddImage(accountId, createAvatarDto);

            if (result == true)
            {
                _notyf.Success("Thêm thành công");

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Redirect("/dang-nhap.html");
            }
            else
            {
                _notyf.Error("Thêm không thành công");

            }

            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("/cap-nhat-anh-dai-dien.html")]
        public IActionResult UpdateAvatar()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 700000000)]
        [RequestSizeLimit(700000000)]
        [Consumes("multipart/form-data")]
        [HttpGet]
        [Route("/cap-nhat-anh-dai-dien.html")]
        public async Task<IActionResult> UpdateAvatar(Guid accountId, [FromForm] UpdateAvatarDto updateAvatarDto, IFormFile fThumb)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }


            var account = await _accountService.GetAccountByEmail(User.Identity.Name);

            if (account == null)
            {
                return RedirectToAction("Login", "Login");
            }
            accountId = account.AccountDto.Id;

            updateAvatarDto.Name = account.AccountDto.accountDetailDto.FullName;


            updateAvatarDto.Image = fThumb;

            var result = await _accountService.UpdateImage(accountId, updateAvatarDto);


            if (result == true)
            {
                _notyf.Success("Cập nhật thành công");

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Redirect("/dang-nhap.html");


            }
            else
            {
                _notyf.Error("Cập nhật không thành công");

            }

            return RedirectToAction("Dashboard");
        }
    }
}
