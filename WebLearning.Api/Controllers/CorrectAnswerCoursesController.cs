using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerCourse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrectAnswerCoursesController : ApiBase
    {
        private readonly ICorrectAnswerCourseService _correctAnswerCourse;

        private readonly ILogger<CorrectAnswerCoursesController> _logger;

        public CorrectAnswerCoursesController(ILogger<CorrectAnswerCoursesController> logger, ICorrectAnswerCourseService correctAnswerCourseService)
        {
            _logger = logger;
            _correctAnswerCourse = correctAnswerCourseService;
        }
        // GET: api/<CorrectAnswerCourseController>
        [HttpGet]
        public async Task<IEnumerable<CorrectAnswerCourseDto>> GetCorrectAnswerCourses()
        {

            return await _correctAnswerCourse.GetcorrectAnswer();

        }
        [HttpGet("paging")]
        public async Task<PagedViewModel<CorrectAnswerCourseDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _correctAnswerCourse.GetPaging(getListPagingRequest);

        }

        // GET api/<CorrectAnswerCourseController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCorrectAnswerCourseById(Guid id)
        {
            if (await _correctAnswerCourse.GetcorrectAnswerById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _correctAnswerCourse.GetcorrectAnswerById(id));

        }

        // POST api/<CorrectAnswerCourseController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCorrectAnswerCourseDto createCorrectAnswerCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _correctAnswerCourse.InsertcorrectAnswer(createCorrectAnswerCourseDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<CorrectAnswerCourseController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateCorrectAnswerCourseDto updateCorrectAnswerCourseDto)
        {
            try
            {
                if (updateCorrectAnswerCourseDto == null)
                    return BadRequest();
                if (await _correctAnswerCourse.GetcorrectAnswerById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _correctAnswerCourse.UpdatecorrectAnswer(updateCorrectAnswerCourseDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // DELETE api/<CorrectAnswerCourseController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorrectAnswerCourse(Guid id)
        {
            try
            {
                if (await _correctAnswerCourse.GetcorrectAnswerById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _correctAnswerCourse.DeletecorrectAnswer(id);
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
