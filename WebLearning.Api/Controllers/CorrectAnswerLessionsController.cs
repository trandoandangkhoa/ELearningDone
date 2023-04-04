using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.CorrectAnswerLession;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrectAnswerLessionsController : ApiBase
    {
        private readonly ILogger<CorrectAnswerLessionsController> _logger;
        private readonly ICorrectAnswerService _correctAnswerLessionService;
        public CorrectAnswerLessionsController(ILogger<CorrectAnswerLessionsController> logger, ICorrectAnswerService correctAnswerLessionService)
        {
            _logger = logger;
            _correctAnswerLessionService = correctAnswerLessionService;
        }
        // GET: api/<CorrectAnswerLessionController>
        [HttpGet]
        public async Task<IEnumerable<CorrectAnswerLessionDto>> GetCorrectAnswerLessions()
        {

            return await _correctAnswerLessionService.GetcorrectAnswer();

        }
        [HttpGet("paging")]
        public async Task<PagedViewModel<CorrectAnswerLessionDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _correctAnswerLessionService.GetPaging(getListPagingRequest);

        }

        // GET api/<CorrectAnswerLessionController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCorrectAnswerLessionById(Guid id)
        {
            if (await _correctAnswerLessionService.GetcorrectAnswerById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _correctAnswerLessionService.GetcorrectAnswerById(id));

        }

        // POST api/<CorrectAnswerLessionController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCorrectAnswerLessionDto createCorrectAnswerLessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _correctAnswerLessionService.InsertcorrectAnswer(createCorrectAnswerLessionDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<CorrectAnswerLessionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateCorrectAnswerLessionDto updateCorrectAnswerLessionDto)
        {
            try
            {
                if (updateCorrectAnswerLessionDto == null)
                    return BadRequest();
                if (await _correctAnswerLessionService.GetcorrectAnswerById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _correctAnswerLessionService.UpdatecorrectAnswer(updateCorrectAnswerLessionDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // DELETE api/<CorrectAnswerLessionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorrectAnswerLession(Guid id)
        {
            try
            {
                if (await _correctAnswerLessionService.GetcorrectAnswerById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _correctAnswerLessionService.DeletecorrectAnswer(id);
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
