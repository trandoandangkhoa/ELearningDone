using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Assets.SubCategory;
using WebLearning.Contract.Dtos.Role;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsSubCategoriesController : ApiBase
    {
        private readonly ILogger<AssetsSubCategoriesController> _logger;
        private readonly ISubCategoryService _subCategoryService;
        public AssetsSubCategoriesController(ILogger<AssetsSubCategoriesController> logger, ISubCategoryService subCategoryService)
        {
            _logger = logger;
            _subCategoryService = subCategoryService;
        }
        // GET: api/<RoleController>
        /// <summary>
        /// Danh sách tất cả các model  thiết bị
        /// </summary>
        [HttpGet]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<AssetsSubCategoryDto>> GetAssetsSubCategories()
        {

            return await _subCategoryService.GetAssetsSubCategory();

        }
        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<AssetsSubCategoryDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _subCategoryService.GetPaging(getListPagingRequest);

        }

        // GET api/<RoleController>/5
        /// <summary>
        /// Lấy chi tiết model  theo Id
        /// </summary>
        [HttpGet("{id}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            if (await _subCategoryService.GetAssetsSubCategoryById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _subCategoryService.GetAssetsSubCategoryById(id));

        }
        /// <summary>
        /// Lấy chi tiết model  theo mã code
        /// </summary>
        [HttpGet("catcode/{code}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetCatByCode(string code)
        {
            if (await _subCategoryService.GetCode(code) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _subCategoryService.GetCode(code));

        }
        // POST api/<RoleController>
        /// <summary>
        /// Tạo model  mới
        /// </summary>
        [HttpPost]

        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetsSubCategoryDto createAssetsSubCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _subCategoryService.InsertAssetsSubCategory(createAssetsSubCategoryDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<RoleController>/5
        /// <summary>
        /// Cập nhật model 
        /// </summary>
        [HttpPut("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAssetsSubCategoryDto updateAssetsSubCategoryDto)
        {
            try
            {
                if (updateAssetsSubCategoryDto == null)
                    return BadRequest();
                if (await _subCategoryService.GetAssetsSubCategoryById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _subCategoryService.UpdateAssetsSubCategory(updateAssetsSubCategoryDto, id);

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
        /// Xóa model 
        /// </summary>
        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                if (await _subCategoryService.GetAssetsSubCategoryById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _subCategoryService.DeleteAssetsSubCategory(id);
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
