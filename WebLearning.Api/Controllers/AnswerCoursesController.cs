using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Services;
using WebLearning.Contract.Dtos.AnswerCourse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerCoursesController : ControllerBase
    {
        private readonly ILogger<AnswerCoursesController> _logger;
        private readonly IAnswerCourseService _AnswerCourseService;

        public AnswerCoursesController(ILogger<AnswerCoursesController> logger, IAnswerCourseService AnswerCourseService)
        {
            _logger = logger;
            _AnswerCourseService = AnswerCourseService;
        }
        // GET: api/<AnswerCoursesController>
        [HttpGet]
        public async Task<IEnumerable<AnswerCourseDto>> Get()
        {
            return await _AnswerCourseService.GetAnswerDtos();
        }
        // GET api/<AnswerCoursesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswerCourseById(Guid id, string accountName)
        {
            if (await _AnswerCourseService.GetAnswerById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _AnswerCourseService.GetAnswerById(id, accountName));

        }
        // POST api/<AnswerCoursesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAnswerCourseDto createAnswerCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createAnswerCourseDto != null)
                {
                    createAnswerCourseDto.Id = Guid.NewGuid();

                    await _AnswerCourseService.InsertAnswerDto(createAnswerCourseDto);

                    return StatusCode(StatusCodes.Status200OK);
                }

                return StatusCode(StatusCodes.Status400BadRequest);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // PUT api/<AnswerCoursesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswerCourse(Guid id, string accountName, [FromBody] UpdateAnswerCourseDto updateAnswerCourseDto)
        {
            try
            {
                if (updateAnswerCourseDto == null)
                    return BadRequest();
                if (await _AnswerCourseService.GetAnswerById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _AnswerCourseService.UpdateAnswerDto(id, accountName, updateAnswerCourseDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<AnswerCoursesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id, string accountName)
        {
            try
            {
                if (await _AnswerCourseService.GetAnswerById(id, accountName) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _AnswerCourseService.DeleteAnswerDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpDelete("DeleteNonExist/{id}")]
        public async Task<IActionResult> DeleteAnswer(Guid id)
        {
            try
            {
                await _AnswerCourseService.DeleteAnswerDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
