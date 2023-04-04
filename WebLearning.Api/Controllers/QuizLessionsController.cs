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
    public class QuizLessionsController : ControllerBase
    {
        private readonly ILogger<QuizLessionsController> _logger;
        private readonly IQuizLessionService _quizLessionService;

        public QuizLessionsController(ILogger<QuizLessionsController> logger, IQuizLessionService quizLessionService)
        {
            _logger = logger;
            _quizLessionService = quizLessionService;
        }
        // GET: api/<QuizLessionsController>
        [HttpGet]
        public async Task<IEnumerable<QuizlessionDto>> Get()
        {
            return await _quizLessionService.GetQuizLessionDtos();
        }
        [HttpGet("/QuizLessionPaging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]
        public async Task<PagedViewModel<QuizlessionDto>> GetPagingQuizLession([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _quizLessionService.GetPaging(getListPagingRequest);

        }
        [HttpGet("FindIndexQuiz/{id}")]

        public async Task<IActionResult> GetQuizLessionById(Guid id, Guid lessionId)
        {
            if (await _quizLessionService.FindIndex(id, lessionId) < 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizLessionService.FindIndex(id, lessionId));

        }
        [HttpGet("GetName")]

        public async Task<IActionResult> GetNameLession(Guid id)
        {
            if (await _quizLessionService.GetNameQuiz(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizLessionService.GetNameQuiz(id));
        }
        [HttpGet("CheckQuizPassed/{id}")]

        public async Task<IActionResult> CheckQuizPassed(Guid id, string accountName, Guid lessionId)
        {

            return Ok(await _quizLessionService.CheckQuizPassed(id, accountName, lessionId));
        }
        // GET api/<QuizLessionsController>/5
        [HttpGet("UserGetQuiz/{id}")]

        public async Task<IActionResult> GetQuizLessionById(Guid id, string accountName)
        {
            if (await _quizLessionService.GetQuizLessionById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizLessionService.GetQuizLessionById(id, accountName));

        }
        [HttpGet("AdminGetQuiz/{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> AdminGetQuizLessionById(Guid id)
        {
            if (await _quizLessionService.AdminGetQuizLessionById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _quizLessionService.AdminGetQuizLessionById(id));

        }
        // POST api/<QuizLessionsController>
        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Post([FromBody] CreateQuizLessionDto createQuizLessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createQuizLessionDto != null)
                {
                    createQuizLessionDto.ID = Guid.NewGuid();

                    await _quizLessionService.InsertQuizLessionDto(createQuizLessionDto);

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

        // PUT api/<QuizLessionsController>/5
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UpdateQuizLession(Guid id, [FromBody] UpdateQuizLessionDto updateQuizLessionDto)
        {
            try
            {
                if (updateQuizLessionDto == null)
                    return BadRequest();
                if (await _quizLessionService.AdminGetQuizLessionById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _quizLessionService.UpdateQuizLessionDto(id, updateQuizLessionDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<QuizLessionsController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                if (await _quizLessionService.AdminGetQuizLessionById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _quizLessionService.DeleteQuizLessionDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
