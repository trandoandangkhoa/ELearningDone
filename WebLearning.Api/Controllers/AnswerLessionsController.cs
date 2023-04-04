using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Services;
using WebLearning.Contract.Dtos.AnswerLession;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerLessionsController : ControllerBase
    {
        private readonly ILogger<AnswerLessionsController> _logger;
        private readonly IAnswerLessionService _AnswerLessionService;

        public AnswerLessionsController(ILogger<AnswerLessionsController> logger, IAnswerLessionService AnswerLessionService)
        {
            _logger = logger;
            _AnswerLessionService = AnswerLessionService;
        }
        // GET: api/<AnswerLessionsController>
        [HttpGet]
        public async Task<IEnumerable<AnswerLessionDto>> Get()
        {
            return await _AnswerLessionService.GetAnswerDtos();
        }
        // GET api/<AnswerLessionsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswerLessionById(Guid id, string accountName)
        {
            if (await _AnswerLessionService.GetAnswerById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _AnswerLessionService.GetAnswerById(id, accountName));

        }
        // POST api/<AnswerLessionsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAnswerLessionDto createAnswerLessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createAnswerLessionDto != null)
                {
                    createAnswerLessionDto.Id = Guid.NewGuid();

                    await _AnswerLessionService.InsertAnswerDto(createAnswerLessionDto);

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

        // PUT api/<AnswerLessionsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswerLession(Guid id, string accountName, [FromBody] UpdateAnswerLessionDto updateAnswerLessionDto)
        {
            try
            {
                if (updateAnswerLessionDto == null)
                    return BadRequest();
                if (await _AnswerLessionService.GetAnswerById(id, accountName) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _AnswerLessionService.UpdateAnswerDto(id, accountName, updateAnswerLessionDto);

                }

                return StatusCode(StatusCodes.Status200OK, "Update Successfully");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        // DELETE api/<AnswerLessionsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id, string accountName)
        {
            try
            {
                if (await _AnswerLessionService.GetAnswerById(id, accountName) == null)
                {
                    return NotFound();
                }
                await _AnswerLessionService.DeleteAnswerDto(id);
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
                await _AnswerLessionService.DeleteAnswerDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [HttpDelete("ResetAllAnswer/{quizLessionId}")]
        public async Task<IActionResult> ResetAllAnswer(Guid quizLessionId, string accountName)
        {
            try
            {
                await _AnswerLessionService.ResetAllAnswer(quizLessionId, accountName);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
