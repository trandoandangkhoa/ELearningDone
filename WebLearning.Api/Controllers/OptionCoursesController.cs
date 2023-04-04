using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.OptionCourse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionCoursesController : ApiBase
    {
        private readonly ILogger<OptionCoursesController> _logger;
        private readonly IOptionCourseService _optionCourseService;
        public OptionCoursesController(ILogger<OptionCoursesController> logger, IOptionCourseService optionCourseService)
        {
            _logger = logger;
            _optionCourseService = optionCourseService;
        }
        // GET: api/<OptionCourseController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]
        public async Task<IEnumerable<OptionCourseDto>> GetOptionCourses()
        {

            return await _optionCourseService.GetOptionCourse();

        }
        [HttpGet("paging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]
        public async Task<PagedViewModel<OptionCourseDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _optionCourseService.GetPaging(getListPagingRequest);

        }

        // GET api/<OptionCourseController>/5
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetOptionCourseById(Guid id)
        {
            if (await _optionCourseService.GetOptionCourseById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _optionCourseService.GetOptionCourseById(id));

        }

        // POST api/<OptionCourseController>
        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Post([FromBody] CreateOptionCourseDto createOptionCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _optionCourseService.InsertOptionCourse(createOptionCourseDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT api/<OptionCourseController>/5
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateOptionCourseDto updateOptionCourseDto)
        {
            try
            {
                if (updateOptionCourseDto == null)
                    return BadRequest();
                if (await _optionCourseService.GetOptionCourseById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _optionCourseService.UpdateOptionCourse(updateOptionCourseDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // DELETE api/<OptionCourseController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> DeleteOptionCourse(Guid id, Guid questionCourseId)
        {
            try
            {
                if (await _optionCourseService.GetOptionCourseById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _optionCourseService.DeleteOptionCourse(id, questionCourseId);
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
