using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Contract.Dtos.Role;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssestsController : ApiBase
    {
        private readonly ILogger<AssestsController> _logger;
        private readonly IAssetService _assetService;
        public AssestsController(ILogger<AssestsController> logger, IAssetService assetService)
        {
            _logger = logger;
            _assetService = assetService;
        }
        // GET: api/<RoleController>
        /// <summary>
        /// Danh sách tất cả các tài sản thiết bị
        /// </summary>
        [HttpGet]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<AssetsDto>> GetAssests()
        {

            return await _assetService.GetAsset();

        }
        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<AssetsDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _assetService.GetPaging(getListPagingRequest);

        }

        // GET api/<RoleController>/5
        /// <summary>
        /// Lấy chi tiết tài sản theo Id
        /// </summary>
        [HttpGet("{id}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetRoleById(string id)
        {
            if (await _assetService.GetAssetById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _assetService.GetAssetById(id));

        }
        /// <summary>
        /// Lấy chi tiết tài sản theo mã code
        /// </summary>
        [HttpGet("catcode/{code}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetCatByCode(string code)
        {
            if (await _assetService.GetCode(code) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _assetService.GetCode(code));

        }
        // POST api/<RoleController>
        /// <summary>
        /// Tạo tài sản mới
        /// </summary>
        [HttpPost]

        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetsDto createAssetsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _assetService.InsertAsset(createAssetsDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<RoleController>/5
        /// <summary>
        /// Cập nhật tài sản
        /// </summary>
        [HttpPut("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAccount(string id, [FromBody] UpdateAssetsDto updateAssetsDto)
        {
            try
            {
                if (updateAssetsDto == null)
                    return BadRequest();
                if (await _assetService.GetAssetById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _assetService.UpdateAsset(updateAssetsDto, id);

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
        /// Xóa tài sản
        /// </summary>
        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                if (await _assetService.GetAssetById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _assetService.DeleteAsset(id);
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
