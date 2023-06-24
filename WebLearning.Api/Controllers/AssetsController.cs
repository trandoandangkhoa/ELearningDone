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
    public class AssestsController : ApiBase
    {
        private readonly ILogger<AssestsController> _logger;
        private readonly IAssetService _assetService;
        public AssestsController(ILogger<AssestsController> logger, IAssetService assetService)
        {
            _logger = logger;
            _assetService = assetService;
        }
        // GET: api/<AssetController>
        /// <summary>
        /// Danh sách tất cả các tài sản thiết bị
        /// </summary>
        [HttpGet]

        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IEnumerable<AssetsDto>> GetAssests()
        {

            return await _assetService.GetAsset();

        }

        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<PagedViewModel<AssetsDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _assetService.GetPaging(getListPagingRequest);

        }
        [HttpGet("export")]

        public async Task<IEnumerable<AssetsDto>> Export([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _assetService.Export(getListPagingRequest);

        }
        [HttpGet("paging/historyFilter")]

        public async Task<PagedViewModel<AssetsDto>> Filter([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _assetService.GetPaging(getListPagingRequest);

        }
        // GET api/<AssetController>/5
        /// <summary>
        /// Lấy chi tiết tài sản theo Id
        /// </summary>
        [HttpGet("{id}")]

        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]
        public async Task<IActionResult> GetAssetById(string id)
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

        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]
        public async Task<IActionResult> GetCatByCode(string code)
        {
            if (await _assetService.GetCode(code) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _assetService.GetCode(code));

        }
        // POST api/<AssetController>
        /// <summary>
        /// Tạo tài sản mới
        /// </summary>
        [HttpPost]

        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

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

        // PUT api/<AssetController>/5
        /// <summary>
        /// Cập nhật tài sản
        /// </summary>
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> UpdateAsset(string id, [FromBody] UpdateAssetsDto updateAssetsDto)
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
        /// Cập nhật tài sản
        /// </summary>
        [HttpPut("multiassets")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> UpdateMultiAsset([FromBody] UpdateMultiAssetsDto updateMultiAssetsDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _assetService.UpdateMultiAsset(updateMultiAssetsDto);

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
        // DELETE api/<AssetController>/5
        [HttpDelete("{id}")]
        //[SecurityAsset(AuthorizeAsset.AdminAsset)]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> DeleteAsset(string id)
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
