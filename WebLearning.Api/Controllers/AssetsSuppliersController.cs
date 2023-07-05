using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Supplier;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsSuppliersController : ApiBase
    {
        private readonly ILogger<AssetsSuppliersController> _logger;
        private readonly ISupplierService _supplierService;
        public AssetsSuppliersController(ILogger<AssetsSuppliersController> logger, ISupplierService supplierService)
        {
            _logger = logger;
            _supplierService = supplierService;
        }
        // GET: api/<AssetSupplierController>
        /// <summary>
        /// Danh sách tất cả các nhà cung cấp thiết bị
        /// </summary>
        [HttpGet]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IEnumerable<AssetsSupplierDto>> GetAssetsSupplier()
        {

            return await _supplierService.GetAssetsSupplier();

        }
        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<PagedViewModel<AssetsSupplierDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _supplierService.GetPaging(getListPagingRequest);

        }

        // GET api/<AssetSupplierController>/5
        /// <summary>
        /// Lấy chi tiết nhà cung cấp theo Id
        /// </summary>
        [HttpGet("{id}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]
        public async Task<IActionResult> GetAssetSupplierById(string id)
        {
            if (await _supplierService.GetAssetsSupplierById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _supplierService.GetAssetsSupplierById(id));

        }
        /// <summary>
        /// Lấy chi tiết nhà cung cấp theo mã code
        /// </summary>
        [HttpGet("catcode/{code}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]
        public async Task<IActionResult> GetCatByCode(string code)
        {
            if (await _supplierService.GetCode(code) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _supplierService.GetCode(code));

        }
        // POST api/<AssetSupplierController>
        /// <summary>
        /// Tạo nhà cung cấp mới
        /// </summary>
        [HttpPost]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetsSupplierDto createAssetSupplierDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _supplierService.InsertAssetsSupplier(createAssetSupplierDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<AssetSupplierController>/5
        /// <summary>
        /// Cập nhật nhà cung cấp
        /// </summary>
        [HttpPut("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> UpdateAssetSupplier(string id, [FromBody] UpdateAssetsSupplierDto updateAssetsSupplierDto)
        {
            try
            {
                if (updateAssetsSupplierDto == null)
                    return BadRequest();
                if (await _supplierService.GetAssetsSupplierById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _supplierService.UpdateAssetsSupplier(updateAssetsSupplierDto, id);

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
        /// Xóa nhà cung cấp
        /// </summary>
        // DELETE api/<AssetSupplierController>/5
        [HttpDelete("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> DeleteAssetSupplier(string id)
        {
            try
            {
                if (await _supplierService.GetAssetsSupplierById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _supplierService.DeleteAssetsSupplier(id);
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
