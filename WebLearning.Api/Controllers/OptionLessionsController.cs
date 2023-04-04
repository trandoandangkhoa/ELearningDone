using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.OptionLession;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionLessionsController : ApiBase
    {
        private readonly ILogger<OptionLessionsController> _logger;
        private readonly IOptionLessionService _optionLessionService;
        public OptionLessionsController(ILogger<OptionLessionsController> logger, IOptionLessionService optionLessionService)
        {
            _logger = logger;
            _optionLessionService = optionLessionService;
        }
        // GET: api/<OptionLessionController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IEnumerable<OptionLessionDto>> GetOptionLessions()
        {

            return await _optionLessionService.GetOptionLession();

        }
        [HttpGet("paging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<PagedViewModel<OptionLessionDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _optionLessionService.GetPaging(getListPagingRequest);

        }

        // GET api/<OptionLessionController>/5
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetOptionLessionById(Guid id)
        {
            if (await _optionLessionService.GetOptionLessionById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _optionLessionService.GetOptionLessionById(id));

        }

        // POST api/<OptionLessionController>
        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Post([FromBody] CreateOptionLessionDto createOptionLessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _optionLessionService.InsertOptionLession(createOptionLessionDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT api/<OptionLessionController>/5
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateOptionLessionDto updateOptionLessionDto)
        {
            try
            {
                if (updateOptionLessionDto == null)
                    return BadRequest();
                if (await _optionLessionService.GetOptionLessionById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _optionLessionService.UpdateOptionLession(updateOptionLessionDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // DELETE api/<OptionLessionController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> DeleteOptionLession(Guid id, Guid questionLessionId)
        {
            try
            {
                if (await _optionLessionService.GetOptionLessionById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _optionLessionService.DeleteOptionLession(id, questionLessionId);
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
