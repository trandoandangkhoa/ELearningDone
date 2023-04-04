using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Course.CourseAdminView;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ApiBase
    {
        private readonly ILogger<CoursesController> _logger;
        private readonly ICourseService _courseService;

        public CoursesController(ILogger<CoursesController> logger, ICourseService courseService)
        {
            _logger = logger;
            _courseService = courseService;
        }
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.Guest, AuthorizeRole.TeacherRole)]
        public async Task<IEnumerable<CourseDto>> GetUsers()
        {

            return await _courseService.GetCourseDtos();

        }
        [HttpGet("GetName")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.Guest, AuthorizeRole.TeacherRole)]
        public async Task<IActionResult> GetNameLession(Guid id)
        {
            if (await _courseService.GetName(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _courseService.GetName(id));
        }
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetCourseById(Guid id)
        {
            if (await _courseService.GetCourseById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _courseService.GetCourseById(id));

        }
        [HttpGet("UserCourse/{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]
        public async Task<IActionResult> UserGetCourse(Guid id, string accountName)
        {
            if (await _courseService.UserCourse(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _courseService.UserCourse(id, accountName));

        }

        [HttpGet("/coursepaging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.Guest, AuthorizeRole.TeacherRole)]
        public async Task<PagedViewModel<CourseDto>> GetCourses([FromQuery] GetListPagingRequest getListPagingRequest, string accountName)
        {

            return await _courseService.GetPaging(getListPagingRequest, accountName);

        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Post([FromForm] CreateCourseDto createCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _courseService.InsertCourseDto(createCourseDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }


        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UpdateCourse(Guid id, [FromForm] UpdateCourseAdminView updateCourseAdminView)
        {
            try
            {
                if (updateCourseAdminView == null)
                    return BadRequest();
                if (await _courseService.GetCourseById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _courseService.UpdateCourseAdminView(id, updateCourseAdminView);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   e.Message);
            }
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            try
            {
                if (await _courseService.GetCourseById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _courseService.DeleteCourseDto(id);
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
