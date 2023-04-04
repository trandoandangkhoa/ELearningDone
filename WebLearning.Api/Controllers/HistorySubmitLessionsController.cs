using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Services;
using WebLearning.Contract.Dtos.HistorySubmit;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorySubmitLessionsController : ControllerBase
    {
        private readonly ILogger<HistorySubmitLessionsController> _logger;
        private readonly IHistorySubmitLessionService _HistorySubmitLessionService;
        private readonly IAnswerLessionService _answerService;


        public HistorySubmitLessionsController(ILogger<HistorySubmitLessionsController> logger, IHistorySubmitLessionService HistorySubmitLessionService, IAnswerLessionService answerService)
        {
            _logger = logger;
            _HistorySubmitLessionService = HistorySubmitLessionService;
            _answerService = answerService;
        }
        // GET: api/<HistorySubmitLessionsController>
        //[HttpGet]
        //public async Task<IEnumerable<HistorySubmitLessionDto>> Get()
        //{
        //    return await _HistorySubmitLessionService.GetQuestionDtos();
        //}
        //[HttpGet("/HistorySubmitLessionPaging")]
        //public async Task<PagedViewModel<HistorySubmitLessionDto>> GetPagingHistorySubmitLession([FromQuery] GetListPagingRequest getListPagingRequest)
        //{

        //    return await _HistorySubmitLessionService.GetPaging(getListPagingRequest);

        //}
        // GET api/<HistorySubmitLessionsController>/5
        [HttpGet]
        public async Task<IActionResult> GetHistorySubmitLessionById(Guid quizLessionId, string accountName)
        {
            if (await _HistorySubmitLessionService.GetHistorySubmitLessionById(quizLessionId, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _HistorySubmitLessionService.GetHistorySubmitLessionById(quizLessionId, accountName));

        }
        // POST api/<HistorySubmitLessionsController>
        [HttpPost]
        public async Task<IActionResult> Post(Guid questionLessionId, string accountName, [FromBody] CreateHistorySubmitLessionDto createHistorySubmitLessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                var answer = await _answerService.GetAnswerById(questionLessionId, accountName);

                if (answer == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);

                }
                else
                {
                    if (createHistorySubmitLessionDto != null)
                    {
                        createHistorySubmitLessionDto.Id = Guid.NewGuid();

                        await _HistorySubmitLessionService.InsertHistorySubmitLessionDto(questionLessionId, accountName, createHistorySubmitLessionDto);

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

        // PUT api/<HistorySubmitLessionsController>/5
        [HttpPatch]
        public async Task<IActionResult> UpdateHistorySubmitLession(Guid questionLessionId, Guid quizLessionId, string accountName, [FromBody] UpdateHistorySubmitLessionDto updateHistorySubmitLessionDto)
        {
            try
            {
                if (updateHistorySubmitLessionDto == null)
                    return BadRequest();
                if (await _HistorySubmitLessionService.GetHistorySubmitLessionById(questionLessionId, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _HistorySubmitLessionService.UpdateHistorySubmitLession(questionLessionId, quizLessionId, accountName, updateHistorySubmitLessionDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<HistorySubmitLessionsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                if (_HistorySubmitLessionService.DeleteHistorySubmitLessionDto(id) == null)
                {
                    return NotFound();
                }
                await _HistorySubmitLessionService.DeleteHistorySubmitLessionDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpDelete("ResetAllHistory/{quizLessionId}")]
        public async Task<IActionResult> ResetAllHistory(Guid quizLessionId, string accountName)
        {
            try
            {
                await _HistorySubmitLessionService.ResetHistorySubmitLessionDto(quizLessionId, accountName);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
