using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionMonthliesController : ControllerBase
    {
        private readonly ILogger<QuestionMonthliesController> _logger;
        private readonly IQuestionMonthlyService _questionMonthlyService;

        public QuestionMonthliesController(ILogger<QuestionMonthliesController> logger, IQuestionMonthlyService questionMonthlyService)
        {
            _logger = logger;
            _questionMonthlyService = questionMonthlyService;
        }
        // GET: api/<QuestionMonthlysController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IEnumerable<QuestionMonthlyDto>> Get()
        {
            return await _questionMonthlyService.GetQuestionDtos();
        }
        [HttpGet("/QuestionMonthlyPaging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<PagedViewModel<QuestionMonthlyDto>> GetPagingQuestionMonthly([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _questionMonthlyService.GetPaging(getListPagingRequest);

        }
        // GET api/<QuestionMonthlysController>/5
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetQuestionMonthlyById(Guid id, string accountName)
        {
            if (await _questionMonthlyService.GetQuestionById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _questionMonthlyService.GetQuestionById(id, accountName));

        }
        [HttpPost("CreateConcerningQuestionMonthly")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> CreateConcerningQuestionMonthly([FromBody] CreateAllConcerningQuestionMonthlyDto createAllConcerningQuestionMonthlyDto)
        {
            try
            {
                await _questionMonthlyService.InsertConcerningQuestionMonthlyDto(createAllConcerningQuestionMonthlyDto);

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

        public async Task<IActionResult> CreateNewOptionAndCorrectAnswer(Guid id, string accountName, [FromBody] UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto)
        {
            try
            {
                if (updateAllConcerningQuestionMonthlyDto == null)
                    return BadRequest();
                if (await _questionMonthlyService.GetQuestionById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _questionMonthlyService.NewOptionAndCorrectAnswerInUpdate(id, updateAllConcerningQuestionMonthlyDto);

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

        public async Task<IActionResult> UpdateQuestionMonthly(Guid id, string accountName, [FromBody] UpdateAllConcerningQuestionMonthlyDto updateAllConcerningQuestionMonthlyDto)
        {
            try
            {
                if (updateAllConcerningQuestionMonthlyDto == null)
                    return BadRequest();
                if (await _questionMonthlyService.GetQuestionById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _questionMonthlyService.UpdateConcerningQuestionMonthlyDto(id, updateAllConcerningQuestionMonthlyDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<QuestionMonthlysController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> DeleteRole(Guid id, string accountName)
        {
            try
            {
                if (await _questionMonthlyService.GetQuestionById(id, accountName) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _questionMonthlyService.DeleteQuestionDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
