using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerMonthly;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrectAnswerMonthliesController : ApiBase
    {
        private readonly ICorrectAnswerMonthlyService _correctAnswerMonthly;

        private readonly ILogger<CorrectAnswerMonthliesController> _logger;

        public CorrectAnswerMonthliesController(ILogger<CorrectAnswerMonthliesController> logger, ICorrectAnswerMonthlyService correctAnswerMonthlieservice)
        {
            _logger = logger;
            _correctAnswerMonthly = correctAnswerMonthlieservice;
        }
        // GET: api/<CorrectAnswerMonthlyController>
        [HttpGet]
        public async Task<IEnumerable<CorrectAnswerMonthlyDto>> GetCorrectAnswerMonthlies()
        {

            return await _correctAnswerMonthly.GetcorrectAnswer();

        }
        [HttpGet("paging")]
        public async Task<PagedViewModel<CorrectAnswerMonthlyDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _correctAnswerMonthly.GetPaging(getListPagingRequest);

        }

        // GET api/<CorrectAnswerMonthlyController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCorrectAnswerMonthlyById(Guid id)
        {
            if (await _correctAnswerMonthly.GetcorrectAnswerById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _correctAnswerMonthly.GetcorrectAnswerById(id));

        }

        // POST api/<CorrectAnswerMonthlyController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCorrectAnswerMonthlyDto createCorrectAnswerMonthlyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _correctAnswerMonthly.InsertcorrectAnswer(createCorrectAnswerMonthlyDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<CorrectAnswerMonthlyController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateCorrectAnswerMonthlyDto updateCorrectAnswerMonthlyDto)
        {
            try
            {
                if (updateCorrectAnswerMonthlyDto == null)
                    return BadRequest();
                if (await _correctAnswerMonthly.GetcorrectAnswerById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _correctAnswerMonthly.UpdatecorrectAnswer(updateCorrectAnswerMonthlyDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // DELETE api/<CorrectAnswerMonthlyController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorrectAnswerMonthly(Guid id)
        {
            try
            {
                if (await _correctAnswerMonthly.GetcorrectAnswerById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _correctAnswerMonthly.DeletecorrectAnswer(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

    }
}
