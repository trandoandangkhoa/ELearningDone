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
    public class QuizMonthliesController : ControllerBase
    {
        private readonly ILogger<QuizMonthliesController> _logger;
        private readonly IQuizMonthlyService _quizMonthlyService;

        public QuizMonthliesController(ILogger<QuizMonthliesController> logger, IQuizMonthlyService quizMonthlyService)
        {
            _logger = logger;
            _quizMonthlyService = quizMonthlyService;
        }
        [HttpGet]
        public async Task<IEnumerable<QuizMonthlyDto>> Get()
        {
            return await _quizMonthlyService.GetQuizDtos();
        }
        [HttpGet("OwnQuizMonthly/{roleId}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<QuizMonthlyDto>> GetOwnQuiz(Guid roleId)
        {
            return await _quizMonthlyService.GetOwnQuizDtos(roleId);
        }
        [HttpGet("/QuizMonthlyPaging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<PagedViewModel<QuizMonthlyDto>> GetPagingQuizCourse([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _quizMonthlyService.GetPaging(getListPagingRequest);

        }
        [HttpGet("GetName")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetNameLession(Guid id)
        {
            if (await _quizMonthlyService.GetNameQuiz(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizMonthlyService.GetNameQuiz(id));
        }
        // GET api/<QuizCoursesController>/5
        [HttpGet("UserGetQuiz/{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetQuizCourseById(Guid id, string accountName)
        {
            if (await _quizMonthlyService.GetQuizById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizMonthlyService.GetQuizById(id, accountName));

        }
        [HttpGet("AdminGetQuiz/{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> AdminGetQuizCourseById(Guid id)
        {
            if (await _quizMonthlyService.AdminGetQuizMonthlyById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizMonthlyService.AdminGetQuizMonthlyById(id));

        }
        // POST api/<QuizCoursesController>
        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Post([FromBody] CreateQuizMonthlyDto createQuizMonthlyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createQuizMonthlyDto != null)
                {
                    createQuizMonthlyDto.ID = Guid.NewGuid();

                    await _quizMonthlyService.InsertQuizDto(createQuizMonthlyDto);

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

        public async Task<IActionResult> UpdateQuizCourse(Guid id, [FromBody] UpdateQuizMonthlyDto updateQuizMonthlyDto)
        {
            try
            {
                if (updateQuizMonthlyDto == null)
                    return BadRequest();
                if (await _quizMonthlyService.AdminGetQuizMonthlyById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _quizMonthlyService.UpdateQuizDto(id, updateQuizMonthlyDto);

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
                if (await _quizMonthlyService.AdminGetQuizMonthlyById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _quizMonthlyService.DeleteQuizDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
