using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Avatar;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ApiBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;

        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }
        // GET: api/<AccountController>
        /// <summary>
        /// Danh sách tài khoản
        /// </summary>

        [HttpGet]
        public async Task<IEnumerable<AccountDto>> GetUsers()
        {

            return await _accountService.GetAccount();

        }
        /// <summary>
        /// Phân trang tài khoản
        /// </summary>
        [HttpGet("/accountpaging")]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<AccountDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _accountService.GetPaging(getListPagingRequest);

        }
        // GET api/<AccountController>/5
        /// <summary>
        /// Lấy thông tin tài khoản bằng Id
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetAccountById(Guid id)
        {
            if (await _accountService.GetUserById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _accountService.GetUserById(id));

        }

        /// <summary>
        /// Lấy họ và tên tài khoản bằng Email
        /// </summary>

        [HttpGet("GetFullName/{accountName}")]
        public async Task<IActionResult> GetFullName(string accountName)
        {
            if (await _accountService.GetNameUser(accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _accountService.GetNameUser(accountName));

        }

        /// <summary>
        /// Lấy thông tin chi tiết tài khoản bằng Email
        /// </summary>

        [HttpGet("AccountDetail/{accountName}")]

        public async Task<IActionResult> GetAccountByEmail(string accountName)
        {
            if (await _accountService.GetUserByKeyWord(accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _accountService.GetUserByKeyWord(accountName));

        }

        /// <summary>
        /// Lấy đường dẫn ảnh đại diện bằng Email
        /// </summary>

        [HttpGet("AvatarImagePath")]
        public async Task<IActionResult> GetAvatarImagePath(string accountName)
        {
            if (await _accountService.AvatarImagePath(accountName) == null)
            {
                return NotFound("NotFound");
            }
            return Ok(await _accountService.AvatarImagePath(accountName));

        }
        // POST api/<AccountController>

        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>

        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateAccountDto createAccountDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                var checkExist = await _accountService.GetAccount();

                foreach (var item in checkExist)
                {
                    if (item.Email.Equals(createAccountDto.Email))
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Tài khoản đã tồn tại");
                    }
                }
                await _accountService.InsertUserInfo(createAccountDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // PUT api/<AccountController>/5
        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>

        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountDto updateAccountDto)
        {
            try
            {
                if (updateAccountDto == null)
                    return BadRequest();
                if (await _accountService.GetUserById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _accountService.UpdateUser(updateAccountDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<AccountController>/5
        /// <summary>
        /// Xóa tài khoản
        /// </summary>

        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                if (await _accountService.GetUserById(id) == null)
                {
                    return NotFound();
                }
                await _accountService.DeleteUser(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        /// <summary>
        /// Tạo ảnh đại diện tài khoản
        /// </summary>

        [HttpPost("{accountId}/Avatar")]
        [RequestFormLimits(MultipartBodyLengthLimit = 700000000)]
        [RequestSizeLimit(700000000)]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.Guest)]

        public async Task<IActionResult> CreateAvatar(Guid accountId, [FromForm] CreateAvatarDto createAvatarDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _accountService.AddImage(accountId, createAvatarDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }

        }

        /// <summary>
        /// Cập nhật ảnh đại diện tài khoản
        /// </summary>

        [HttpPut("{accountId}/Avatar")]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 700000000)]
        [RequestSizeLimit(700000000)]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.Guest)]

        public async Task<IActionResult> UpdateImage(Guid accountId, [FromForm] UpdateAvatarDto updateAvatarDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _accountService.UpdateImage(accountId, updateAvatarDto);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        [HttpPut("changepassword/{accountId}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.Guest, AuthorizeRole.ManagerRole, AuthorizeRole.StaffRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> ChangePassword(Guid accountId, ChangePassword changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var a = await _accountService.ChangePassword(accountId, changePassword);
                return StatusCode(StatusCodes.Status200OK, a);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
