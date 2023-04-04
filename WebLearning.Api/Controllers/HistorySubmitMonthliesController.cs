using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Services;
using WebLearning.Contract.Dtos.HistorySubmit;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorySubmitMonthliesController : ControllerBase
    {
        private readonly ILogger<HistorySubmitMonthliesController> _logger;
        private readonly IHistorySubmitMonthlyService _historySubmitMonthlyService;
        private readonly IAnswerMonthlyService _answerMonthlyService;


        public HistorySubmitMonthliesController(ILogger<HistorySubmitMonthliesController> logger, IHistorySubmitMonthlyService historySubmitMonthlyService, IAnswerMonthlyService answerMonthlyService)
        {
            _logger = logger;
            _historySubmitMonthlyService = historySubmitMonthlyService;
            _answerMonthlyService = answerMonthlyService;
        }
        //[HttpGet("/HistorySubmitMonthlyPaging")]
        //public async Task<PagedViewModel<HistorySubmitMonthlyDto>> GetPagingHistorySubmitMonthly([FromQuery] GetListPagingRequest getListPagingRequest)
        //{

        //    return await _HistorySubmitMonthlieservice.GetPaging(getListPagingRequest);

        //}
        // GET api/<HistorySubmitMonthliesController>/5
        [HttpGet]
        public async Task<IActionResult> GetHistorySubmitMonthlyById(Guid quizMonthlyId, string accountName)
        {
            if (await _historySubmitMonthlyService.GetHistorySubmitMonthlyById(quizMonthlyId, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _historySubmitMonthlyService.GetHistorySubmitMonthlyById(quizMonthlyId, accountName));

        }
        // POST api/<HistorySubmitMonthliesController>
        [HttpPost]
        public async Task<IActionResult> Post(Guid questionMonthlyId, string accountName, [FromBody] CreateHistorySubmitMonthlyDto createHistorySubmitMonthlyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                var answer = await _answerMonthlyService.GetAnswerById(questionMonthlyId, accountName);

                if (answer == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);

                }
                else
                {
                    if (createHistorySubmitMonthlyDto != null)
                    {
                        createHistorySubmitMonthlyDto.Id = Guid.NewGuid();

                        await _historySubmitMonthlyService.InsertHistorySubmitMonthlyDto(questionMonthlyId, accountName, createHistorySubmitMonthlyDto);

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
    }
}
