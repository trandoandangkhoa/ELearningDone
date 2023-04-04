using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionCourseAdminView;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionCoursesController : ControllerBase
    {
        private readonly ILogger<QuestionCoursesController> _logger;
        private readonly IQuestionCourseService _QuestionCourseService;

        public QuestionCoursesController(ILogger<QuestionCoursesController> logger, IQuestionCourseService QuestionCourseService)
        {
            _logger = logger;
            _QuestionCourseService = QuestionCourseService;
        }
        // GET: api/<QuestionCoursesController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]
        public async Task<IEnumerable<QuestionCourseDto>> Get()
        {
            return await _QuestionCourseService.GetQuestionDtos();
        }
        [HttpGet("/QuestionCoursePaging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<PagedViewModel<QuestionCourseDto>> GetPagingQuestionCourse([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _QuestionCourseService.GetPaging(getListPagingRequest);

        }
        // GET api/<QuestionCoursesController>/5
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetQuestionCourseById(Guid id, string accountName)
        {
            if (await _QuestionCourseService.GetQuestionById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _QuestionCourseService.GetQuestionById(id, accountName));

        }
        [HttpPost("CreateConcerningQuestionCourse")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> CreateConcerningQuestionCourse([FromBody] CreateAllConcerningQuestionCourseDto createAllConcerningQuestionCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _QuestionCourseService.InsertConcerningQuestionCourseDto(createAllConcerningQuestionCourseDto);

                return StatusCode(StatusCodes.Status200OK);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }


        [HttpPost("{id}/CreateNewOptionAndCorrectAnswer")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> CreateNewOptionAndCorrectAnswer(Guid id, string accountName, [FromBody] UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto)
        {
            try
            {
                if (updateAllConcerningQuestionCourseDto == null)
                    return BadRequest();
                if (await _QuestionCourseService.GetQuestionById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _QuestionCourseService.NewOptionAndCorrectAnswerInUpdate(id, updateAllConcerningQuestionCourseDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UpdateQuestionCourse(Guid id, string accountName, [FromBody] UpdateAllConcerningQuestionCourseDto updateAllConcerningQuestionCourseDto)
        {
            try
            {
                if (updateAllConcerningQuestionCourseDto == null)
                    return BadRequest();
                if (await _QuestionCourseService.GetQuestionById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _QuestionCourseService.UpdateConcerningQuestionCourseDto(id, updateAllConcerningQuestionCourseDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<QuestionCoursesController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> DeleteRole(Guid id, string accountName)
        {
            try
            {
                if (await _QuestionCourseService.GetQuestionById(id, accountName) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _QuestionCourseService.DeleteQuestionDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
