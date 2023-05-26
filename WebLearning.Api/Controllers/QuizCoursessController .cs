using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Quiz;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizCoursesController : ControllerBase
    {
        private readonly ILogger<QuizCoursesController> _logger;
        private readonly IQuizCourseService _quizCourseService;

        public QuizCoursesController(ILogger<QuizCoursesController> logger, IQuizCourseService quizCourseService)
        {
            _logger = logger;
            _quizCourseService = quizCourseService;
        }
        // GET: api/<QuizCoursesController>
        [HttpGet]
        public async Task<IEnumerable<QuizCourseDto>> Get()
        {
            return await _quizCourseService.GetQuizDtos();
        }
        [HttpGet("/QuizCoursePaging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<PagedViewModel<QuizCourseDto>> GetPagingQuizCourse([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _quizCourseService.GetPaging(getListPagingRequest);

        }
        [HttpGet("GetName")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetNameLession(Guid id)
        {
            if (await _quizCourseService.GetNameQuiz(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizCourseService.GetNameQuiz(id));
        }
        // GET api/<QuizCoursesController>/5
        [HttpGet("UserGetQuiz/{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetQuizCourseById(Guid id, string accountName)
        {
            if (await _quizCourseService.GetQuizById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizCourseService.GetQuizById(id, accountName));

        }
        [HttpGet("AdminGetQuiz/{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> AdminGetQuizCourseById(Guid id)
        {
            if (await _quizCourseService.AdminGetQuizCourseById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizCourseService.AdminGetQuizCourseById(id));

        }

        // POST api/<QuizCoursesController>
        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Post([FromBody] CreateQuizCourseDto createQuizCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createQuizCourseDto != null)
                {
                    createQuizCourseDto.ID = Guid.NewGuid();

                    await _quizCourseService.InsertQuizDto(createQuizCourseDto);

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

        // PUT api/<QuizCoursesController>/5
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UpdateQuizCourse(Guid id, [FromBody] UpdateQuizCourseDto updateQuizCourseDto)
        {
            try
            {
                if (updateQuizCourseDto == null)
                    return BadRequest();
                if (await _quizCourseService.AdminGetQuizCourseById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _quizCourseService.UpdateQuizDto(id, updateQuizCourseDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<QuizCoursesController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                if (await _quizCourseService.AdminGetQuizCourseById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _quizCourseService.DeleteQuizDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("ResetAnswer/{quizCourseId}")]
        public async Task<IActionResult> ResetQuizCourse(Guid quizCourseId, string accountName)
        {
            try
            {
                if (await _quizCourseService.AdminGetQuizCourseById(quizCourseId) == null)
                {
                    return NotFound($"User with Id = {quizCourseId} not found");
                }
                await _quizCourseService.ResetQuizCourse(quizCourseId, accountName);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
