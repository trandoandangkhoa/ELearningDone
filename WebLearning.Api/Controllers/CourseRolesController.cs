using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.CourseRole;


namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseRolesController : ApiBase
    {
        private readonly ILogger<CourseRolesController> _logger;
        private readonly ICourseRoleService _CourseRoleService;
        public CourseRolesController(ILogger<CourseRolesController> logger, ICourseRoleService CourseRoleService)
        {
            _logger = logger;
            _CourseRoleService = CourseRoleService;
        }
        // GET: api/<CourseRoleController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IEnumerable<CourseRoleDto>> GetCourseRoles(string accountName)
        {

            return await _CourseRoleService.GetCourseRole(accountName);

        }
        [HttpGet("/CourseRolePaging")]
        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<PagedViewModel<CourseRoleDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _CourseRoleService.GetPaging(getListPagingRequest);

        }
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> AdminGetCourse(Guid id, Guid roleId)
        {
            if (await _CourseRoleService.AdminGetCourse(id, roleId) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _CourseRoleService.AdminGetCourse(id, roleId));

        }
        // GET api/<CourseRoleController>/5
        [HttpGet("UserGetCourseRole/{courseId}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetCourseRoleById(Guid courseId, string accountName)
        {
            try
            {
                var course = await _CourseRoleService.GetCourseRoleById(courseId, accountName);

                if (course == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                return Ok(course);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




        }

        // POST api/<CourseRoleController>
        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> Post([FromBody] CreateCourseRoleDto createCourseRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _CourseRoleService.InsertCourseRole(createCourseRoleDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT api/<CourseRoleController>/5


        // DELETE api/<CourseRoleController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]
        public async Task<IActionResult> DeleteCourseRole(Guid id, Guid roleId)
        {
            try
            {
                if (await _CourseRoleService.AdminGetCourse(id, roleId) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _CourseRoleService.DeleteCourseRole(id, roleId);
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
