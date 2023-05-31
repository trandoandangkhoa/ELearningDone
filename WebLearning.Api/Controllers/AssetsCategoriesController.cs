using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Role;

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
        // GET: api/<RoleController>
        /// <summary>
        /// Danh sách tất cả các loại thiết bị
        /// </summary>
        [HttpGet]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<AssetsCategoryDto>> GetAssetsCategories()
        {

            return await _categoryService.GetAssetsCategory();

        }
        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<AssetsCategoryDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _categoryService.GetPaging(getListPagingRequest);

        }

        // GET api/<RoleController>/5
        /// <summary>
        /// Lấy chi tiết loại theo Id
        /// </summary>
        [HttpGet("{id}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetRoleById(Guid id)
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

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetCatByCode(string code)
        {
            if (await _categoryService.GetCode(code) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _categoryService.GetCode(code));

        }
        // POST api/<RoleController>
        /// <summary>
        /// Tạo loại mới
        /// </summary>
        [HttpPost]

        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetCategoryDto createAssetCategoryDto)
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

        // PUT api/<RoleController>/5
        /// <summary>
        /// Cập nhật loại
        /// </summary>
        [HttpPut("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAssetsCategoryDto updateAssetsCategoryDto)
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
        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> DeleteRole(Guid id)
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
