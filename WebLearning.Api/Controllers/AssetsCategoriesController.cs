using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Assets.Category;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsCategoriesController : ApiBase
    {
        private readonly ILogger<AssetsCategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        public AssetsCategoriesController(ILogger<AssetsCategoriesController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }
        // GET: api/<AssetCategoryController>
        /// <summary>
        /// Danh sách tất cả các loại thiết bị
        /// </summary>
        [HttpGet]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IEnumerable<AssetsCategoryDto>> GetAssetsCategories()
        {

            return await _categoryService.GetAssetsCategory();

        }
        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<AssetsCategoryDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _categoryService.GetPaging(getListPagingRequest);

        }

        // GET api/<AssetCategoryController>/5
        /// <summary>
        /// Lấy chi tiết loại theo Id
        /// </summary>
        [HttpGet("{id}")]

        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> GetAssetCategoryById(Guid id)
        {
            if (await _categoryService.GetAssetsCategoryById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _categoryService.GetAssetsCategoryById(id));

        }
        /// <summary>
        /// Lấy chi tiết loại theo mã code
        /// </summary>
        [HttpGet("catcode/{code}")]

        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> GetCatByCode(string code)
        {
            if (await _categoryService.GetCode(code) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _categoryService.GetCode(code));

        }
        // POST api/<AssetCategoryController>
        /// <summary>
        /// Tạo loại mới
        /// </summary>
        [HttpPost]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetsCategoryDto createAssetCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _categoryService.InsertAssetsCategory(createAssetCategoryDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<AssetCategoryController>/5
        /// <summary>
        /// Cập nhật loại
        /// </summary>
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAssetCategory(Guid id, [FromBody] UpdateAssetsCategoryDto updateAssetsCategoryDto)
        {
            try
            {
                if (updateAssetsCategoryDto == null)
                    return BadRequest();
                if (await _categoryService.GetAssetsCategoryById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _categoryService.UpdateAssetsCategory(updateAssetsCategoryDto, id);

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
        /// Xóa loại
        /// </summary>
        // DELETE api/<AssetCategoryController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> DeleteAssetCategory(Guid id)
        {
            try
            {
                if (await _categoryService.GetAssetsCategoryById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _categoryService.DeleteAssetsCategory(id);
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
