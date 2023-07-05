using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets.Repair;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsRepairedController : ApiBase
    {
        private readonly ILogger<AssetsRepairedController> _logger;
        private readonly IRepairedService _repairedService;
        public AssetsRepairedController(ILogger<AssetsRepairedController> logger, IRepairedService repairedService)
        {
            _logger = logger;
            _repairedService = repairedService;
        }
        // GET: api/<RoleController>
        /// <summary>
        /// Danh sách tất cả các tài sản sữa chữa thiết bị
        /// </summary>
        [HttpGet]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IEnumerable<AssetsRepairedDto>> GetAssetsStatues()
        {

            return await _repairedService.GetAssetsRepaired();

        }
        /// <summary>
        /// Phân trang
        /// </summary>
        [HttpGet("paging")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<PagedViewModel<AssetsRepairedDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _repairedService.GetPaging(getListPagingRequest);

        }

        // GET api/<RoleController>/5
        /// <summary>
        /// Lấy chi tiết tài sản sữa chữa theo Id
        /// </summary>
        [HttpGet("{id}")]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]
        public async Task<IActionResult> GetAssetsRepairedById(Guid id)
        {
            if (await _repairedService.GetAssetsRepairedById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _repairedService.GetAssetsRepairedById(id));

        }
        // POST api/<RoleController>
        /// <summary>
        /// Tạo tài sản sữa chữa mới
        /// </summary>
        [HttpPost]

        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> Post([FromBody] CreateAssetsRepairedDto createAssetsRepairedDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _repairedService.InsertAssetsRepaired(createAssetsRepairedDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT api/<RoleController>/5
        /// <summary>
        /// Cập nhật tài sản sữa chữa
        /// </summary>
        [HttpPut("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> UpdateAssetsRepaired(Guid id, [FromBody] UpdateAssetsRepairedDto updateAssetsRepairedDto)
        {
            try
            {
                if (updateAssetsRepairedDto == null)
                    return BadRequest();
                if (await _repairedService.GetAssetsRepairedById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _repairedService.UpdateAssetsRepaired(updateAssetsRepairedDto, id);

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
        /// Xóa tài sản sữa chữa
        /// </summary>
        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ITRole)]

        public async Task<IActionResult> DeleteAssetsRepaired(Guid id)
        {
            try
            {
                if (await _repairedService.GetAssetsRepairedById(id) == null)
                {
                    return NotFound($"AssetsRepaired with Id = {id} not found");
                }
                await _repairedService.DeleteAssetsRepaired(id);
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
