using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.Login;

namespace WebLearning.App.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;

        private readonly ILoginService _loginService;
        public LoginController(ILogger<LoginController> logger, ILoginService loginService, IConfiguration configuration)
        {
            _logger = logger;
            _loginService = loginService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/dang-nhap.html", Name = "Login")]
        public IActionResult Login(LoginDto loginDto)
        {
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
        public async Task<IActionResult> LoginAccount(LoginDto loginDto)
        {

            if (!ModelState.IsValid) return View(ModelState);


            var token = await _loginService.Authenicate(loginDto);
            if (token == "Tài khoản không đúng")
            {
                return Redirect("/dang-nhap.html");
            }
            var claim = new List<Claim>
            {
            new Claim(ClaimTypes.Email,loginDto.UserName),
            new Claim(ClaimTypes.GivenName,loginDto.UserName),
            new Claim(ClaimTypes.Name,loginDto.UserName)
            };
            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var principle = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, props).Wait();
            HttpContext.Session.SetString("Token", token);

            return RedirectToAction("Index", "Home");



        }
        [HttpPost]
        [AllowAnonymous]
        [Route("/dang-xuat.html")]
        public async Task<IActionResult> LogOutAccount(LoginDto loginDto)
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "Login");
        }

    }
}
