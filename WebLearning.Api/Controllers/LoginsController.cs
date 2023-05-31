using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebLearning.Application.ELearning.Services;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Login;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly ILogger<LoginsController> _logger;
        private readonly AppSetting _appSettings;
        private readonly ILoginService _loginService;
        public LoginsController(ILogger<LoginsController> logger, ILoginService loginService, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _logger = logger;
            _appSettings = optionsMonitor.CurrentValue;
            _loginService = loginService;

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var resultToken = await _loginService.Login(loginDto);
            if (string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("Tài khoản không đúng");
            }
            return Ok(resultToken);

        }

        //[HttpPost("RenewToken")]
        //public async Task<ApiResponse> RenewToken(TokenModel model)
        //{
        //    return await _loginService.RenewToken(model);
        //}

    }
}
