using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.ELearning.Services;
using WebLearning.Contract.Dtos.HistorySubmit;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorySubmitCoursesController : ControllerBase
    {
        private readonly ILogger<HistorySubmitCoursesController> _logger;
        private readonly IHistorySubmitCourseService _HistorySubmitCourseService;
        private readonly IAnswerCourseService _answerService;


        public HistorySubmitCoursesController(ILogger<HistorySubmitCoursesController> logger, IHistorySubmitCourseService HistorySubmitCourseService, IAnswerCourseService answerService)
        {
            _logger = logger;
            _HistorySubmitCourseService = HistorySubmitCourseService;
            _answerService = answerService;
        }
        //[HttpGet("/HistorySubmitCoursePaging")]
        //public async Task<PagedViewModel<HistorySubmitCourseDto>> GetPagingHistorySubmitCourse([FromQuery] GetListPagingRequest getListPagingRequest)
        //{

        //    return await _HistorySubmitCourseService.GetPaging(getListPagingRequest);

        //}
        // GET api/<HistorySubmitCoursesController>/5
        [HttpGet]
        public async Task<IActionResult> GetHistorySubmitCourseById(Guid quizCourseId, string accountName)
        {
            if (await _HistorySubmitCourseService.GetHistorySubmitCourseById(quizCourseId, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _HistorySubmitCourseService.GetHistorySubmitCourseById(quizCourseId, accountName));

        }
        // POST api/<HistorySubmitCoursesController>
        [HttpPost]
        public async Task<IActionResult> Post(Guid questionCourseId, string accountName, [FromBody] CreateHistorySubmitCourseDto createHistorySubmitCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                var answer = await _answerService.GetAnswerById(questionCourseId, accountName);

                if (answer == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);

                }
                else
                {
                    if (createHistorySubmitCourseDto != null)
                    {
                        createHistorySubmitCourseDto.Id = Guid.NewGuid();

                        await _HistorySubmitCourseService.InsertHistorySubmitCourseDto(questionCourseId, accountName, createHistorySubmitCourseDto);

                        return StatusCode(StatusCodes.Status200OK);
                    }
                }


                return StatusCode(StatusCodes.Status400BadRequest);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
        // DELETE api/<HistorySubmitCoursesController>/5

    }
}
