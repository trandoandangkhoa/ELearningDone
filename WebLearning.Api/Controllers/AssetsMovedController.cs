using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Assets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsMovedController : ApiBase
    {
        private readonly ILogger<AssetsMovedController> _logger;
        private readonly IAssetMovedService _moveService;
        public AssetsMovedController(ILogger<AssetsMovedController> logger, IAssetMovedService moveService)
        {
            _logger = logger;
            _moveService = moveService;
        }
        // GET: api/<RoleController>
        /// <summary>
        /// Danh sách tất cả các điều chuyển thiết bị
        /// </summary>
        [HttpGet]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IEnumerable<AssetsMovedDto>> GetAssetsMoved()
        {

            return await _moveService.GetAssetsMoved();

        }

        [HttpGet("paging")]
        public async Task<PagedViewModel<AssetsMovedDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _moveService.GetPaging(getListPagingRequest);

        }

        // GET api/<RoleController>/5
        /// <summary>
        /// Lấy chi tiết điều chuyển theo Id
        /// </summary>
        [HttpGet("{id}")]

        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            if (await _moveService.GetAssetsMovedById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _moveService.GetAssetsMovedById(id));

        }
        // POST api/<RoleController>
        /// <summary>
        /// Tạo điều chuyển mới
        /// </summary>
        [HttpPost]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetsMovedDto createAssetsMovedDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _moveService.InsertAssetsMoved(createAssetsMovedDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<RoleController>/5
        /// <summary>
        /// Cập nhật điều chuyển
        /// </summary>
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAssetsMovedDto updateAssetsMovedDto)
        {
            try
            {
                if (updateAssetsMovedDto == null)
                    return BadRequest();
                if (await _moveService.GetAssetsMovedById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _moveService.UpdateAssetsMoved(updateAssetsMovedDto, id);

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
        /// Xóa điều chuyển
        /// </summary>
        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                if (await _moveService.GetAssetsMovedById(id) == null)
                {
                    return NotFound($"Asset with Id = {id} not found");
                }
                await _moveService.DeleteAssetsMoved(id);
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
