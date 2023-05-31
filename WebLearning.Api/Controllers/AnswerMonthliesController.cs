using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.ELearning.Services;
using WebLearning.Contract.Dtos.AnswerMonthly;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerMonthliesController : ControllerBase
    {
        private readonly ILogger<AnswerMonthliesController> _logger;
        private readonly IAnswerMonthlyService _answerMonthlyService;

        public AnswerMonthliesController(ILogger<AnswerMonthliesController> logger, IAnswerMonthlyService answerMonthlyService)
        {
            _logger = logger;
            _answerMonthlyService = answerMonthlyService;
        }
        // GET: api/<AnswerMonthliesController>
        [HttpGet]
        public async Task<IEnumerable<AnswerMonthlyDto>> Get()
        {
            return await _answerMonthlyService.GetAnswerDtos();
        }
        // GET api/<AnswerMonthliesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswerMonthlyById(Guid id, string accountName)
        {
            if (await _answerMonthlyService.GetAnswerById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _answerMonthlyService.GetAnswerById(id, accountName));

        }
        // POST api/<AnswerMonthliesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAnswerMonthlyDto createAnswerMonthlyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createAnswerMonthlyDto != null)
                {
                    createAnswerMonthlyDto.Id = Guid.NewGuid();

                    await _answerMonthlyService.InsertAnswerDto(createAnswerMonthlyDto);

                    return StatusCode(StatusCodes.Status200OK, "Create Successfully");
                }

                return StatusCode(StatusCodes.Status400BadRequest);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // PUT api/<AnswerMonthliesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswerMonthly(Guid id, string accountName, [FromBody] UpdateAnswerMonthlyDto updateAnswerMonthlyDto)
        {
            try
            {
                if (updateAnswerMonthlyDto == null)
                    return BadRequest();
                if (await _answerMonthlyService.GetAnswerById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _answerMonthlyService.UpdateAnswerDto(id, accountName, updateAnswerMonthlyDto);

                }

                return StatusCode(StatusCodes.Status200OK, "Update Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<AnswerMonthliesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id, string accountName)
        {
            try
            {
                if (await _answerMonthlyService.GetAnswerById(id, accountName) == null)
                {
                    return NotFound();
                }
                await _answerMonthlyService.DeleteAnswerDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpDelete("DeleteNonExist/{id}")]
        public async Task<IActionResult> DeleteAnswer(Guid id)
        {
            try
            {
                await _answerMonthlyService.DeleteAnswerDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
