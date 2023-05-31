using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionLessionAdminView;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionLessionsController : ControllerBase
    {
        private readonly ILogger<QuestionLessionsController> _logger;
        private readonly IQuestionLessionService _QuestionLessionService;

        public QuestionLessionsController(ILogger<QuestionLessionsController> logger, IQuestionLessionService QuestionLessionService)
        {
            _logger = logger;
            _QuestionLessionService = QuestionLessionService;
        }
        // GET: api/<QuestionLessionsController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IEnumerable<QuestionLessionDto>> Get()
        {
            return await _QuestionLessionService.GetQuestionDtos();
        }
        [HttpGet("/QuestionLessionPaging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<PagedViewModel<QuestionLessionDto>> GetPagingQuestionLession([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _QuestionLessionService.GetPaging(getListPagingRequest);

        }
        // GET api/<QuestionLessionsController>/5
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetQuestionLessionById(Guid id, string accountName)
        {
            if (await _QuestionLessionService.GetQuestionById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _QuestionLessionService.GetQuestionById(id, accountName));

        }
        [HttpPost("CreateConcerningQuestionLession")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> CreateConcerningQuestionLession([FromBody] CreateAllConcerningQuestionLessionDto createAllConcerningQuestionLessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _QuestionLessionService.InsertConcerningQuestionLessionDto(createAllConcerningQuestionLessionDto);

                return StatusCode(StatusCodes.Status200OK);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    e.Message);
            }
        }


        [HttpPost("{id}/CreateNewOptionAndCorrectAnswer")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> CreateNewOptionAndCorrectAnswer(Guid id, string accountName, [FromBody] UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto)
        {
            try
            {
                if (updateAllConcerningQuestionLesstionDto == null)
                    return BadRequest();
                if (await _QuestionLessionService.GetQuestionById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _QuestionLessionService.NewOptionAndCorrectAnswerInUpdate(id, updateAllConcerningQuestionLesstionDto);

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
        public async Task<IActionResult> UpdateQuestionLession(Guid id, string accountName, [FromBody] UpdateAllConcerningQuestionLesstionDto updateAllConcerningQuestionLesstionDto)
        {
            try
            {
                if (updateAllConcerningQuestionLesstionDto == null)
                    return BadRequest();
                if (await _QuestionLessionService.GetQuestionById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _QuestionLessionService.UpdateConcerningQuestionLessionDto(id, updateAllConcerningQuestionLesstionDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<QuestionLessionsController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]
        public async Task<IActionResult> DeleteRole(Guid id, string accountName)
        {
            try
            {
                if (await _QuestionLessionService.GetQuestionById(id, accountName) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _QuestionLessionService.DeleteQuestionDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
