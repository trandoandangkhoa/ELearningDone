using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Assets.Status;
using WebLearning.Contract.Dtos.Role;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsStatuesController : ApiBase
    {
        private readonly ILogger<AssetsStatuesController> _logger;
        private readonly IStatusService _statusService;
        public AssetsStatuesController(ILogger<AssetsStatuesController> logger, IStatusService statusService)
        {
            _logger = logger;
            _statusService = statusService;
        }
        // GET: api/<RoleController>
        /// <summary>
        /// Danh sách tất cả các trạng thái thiết bị
        /// </summary>
        [HttpGet]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<AssetsStatusDto>> GetAssetsStatues()
        {

            return await _statusService.GetAssetsStatus();

        }
        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<AssetsStatusDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _statusService.GetPaging(getListPagingRequest);

        }

        // GET api/<RoleController>/5
        /// <summary>
        /// Lấy chi tiết trạng thái theo Id
        /// </summary>
        [HttpGet("{id}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            if (await _statusService.GetAssetsStatusById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _statusService.GetAssetsStatusById(id));

        }
        // POST api/<RoleController>
        /// <summary>
        /// Tạo trạng thái mới
        /// </summary>
        [HttpPost]

        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetsStatusDto createAssetsStatusDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _statusService.InsertAssetsStatus(createAssetsStatusDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<RoleController>/5
        /// <summary>
        /// Cập nhật trạng thái
        /// </summary>
        [HttpPut("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAssetsStatusDto updateAssetsStatusDto)
        {
            try
            {
                if (updateAssetsStatusDto == null)
                    return BadRequest();
                if (await _statusService.GetAssetsStatusById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _statusService.UpdateAssetsStatus(updateAssetsStatusDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
        /// <summary>
        /// Xóa trạng thái
        /// </summary>
        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                if (await _statusService.GetAssetsStatusById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _statusService.DeleteAssetsStatus(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
