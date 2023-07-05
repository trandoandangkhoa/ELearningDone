using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Assets.Department;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsDepartmentsController : ApiBase
    {
        private readonly ILogger<AssetsDepartmentsController> _logger;
        private readonly IDepartmentService _departmentService;
        public AssetsDepartmentsController(ILogger<AssetsDepartmentsController> logger, IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
        }
        // GET: api/<AssetDepartmentController>
        /// <summary>
        /// Danh sách tất cả các đơn vị sử dụng thiết bị
        /// </summary>
        [HttpGet]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IEnumerable<AssetsDepartmentDto>> GetAssetsDepartments()
        {

            return await _departmentService.GetAssetsDepartment();

        }
        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<AssetsDepartmentDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _departmentService.GetPaging(getListPagingRequest);

        }

        // GET api/<AssetDepartmentController>/5
        /// <summary>
        /// Lấy chi tiết đơn vị sử dụng theo Id
        /// </summary>
        [HttpGet("{id}")]

        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> GetAssetDepartmentById(Guid id)
        {
            if (await _departmentService.GetAssetsDepartmentById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _departmentService.GetAssetsDepartmentById(id));

        }
        /// <summary>
        /// Lấy chi tiết đơn vị sử dụng theo mã code
        /// </summary>
        [HttpGet("catcode/{code}")]

        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> GetCatByCode(string code)
        {
            if (await _departmentService.GetCode(code) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _departmentService.GetCode(code));

        }
        // POST api/<AssetDepartmentController>
        /// <summary>
        /// Tạo đơn vị sử dụng mới
        /// </summary>
        [HttpPost]

        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetsDepartmentDto createAssetsDepartmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _departmentService.InsertAssetsDepartment(createAssetsDepartmentDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<AssetDepartmentController>/5
        /// <summary>
        /// Cập nhật đơn vị sử dụng
        /// </summary>
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAssetDepartment(Guid id, [FromBody] UpdateAssetsDepartmentDto updateAssetsDepartmentDto)
        {
            try
            {
                if (updateAssetsDepartmentDto == null)
                    return BadRequest();
                if (await _departmentService.GetAssetsDepartmentById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _departmentService.UpdateAssetsDepartment(updateAssetsDepartmentDto, id);

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
        /// Xóa đơn vị sử dụng
        /// </summary>
        // DELETE api/<AssetDepartmentController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> DeleteAssetDepartment(Guid id)
        {
            try
            {
                if (await _departmentService.GetAssetsDepartmentById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _departmentService.DeleteAssetsDepartment(id);
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
