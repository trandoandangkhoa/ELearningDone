using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.OptionMonthly;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionMonthliesController : ApiBase
    {
        private readonly ILogger<OptionMonthliesController> _logger;
        private readonly IOptionMonthlyService _optionMonthlyService;
        public OptionMonthliesController(ILogger<OptionMonthliesController> logger, IOptionMonthlyService optionMonthlyService)
        {
            _logger = logger;
            _optionMonthlyService = optionMonthlyService;
        }
        // GET: api/<OptionMonthlyController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IEnumerable<OptionMonthlyDto>> GetOptionMonthlys()
        {

            return await _optionMonthlyService.GetOptionMonthly();

        }
        [HttpGet("paging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<PagedViewModel<OptionMonthlyDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _optionMonthlyService.GetPaging(getListPagingRequest);

        }

        // GET api/<OptionMonthlyController>/5
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetOptionMonthlyById(Guid id)
        {
            if (await _optionMonthlyService.GetOptionMonthlyById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _optionMonthlyService.GetOptionMonthlyById(id));

        }

        // POST api/<OptionMonthlyController>
        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Post([FromBody] CreateOptionMonthlyDto createOptionMonthlyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _optionMonthlyService.InsertOptionMonthly(createOptionMonthlyDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PUT api/<OptionMonthlyController>/5
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateOptionMonthlyDto updateOptionMonthlyDto)
        {
            try
            {
                if (updateOptionMonthlyDto == null)
                    return BadRequest();
                if (await _optionMonthlyService.GetOptionMonthlyById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _optionMonthlyService.UpdateOptionMonthly(updateOptionMonthlyDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // DELETE api/<OptionMonthlyController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> DeleteOptionMonthly(Guid id, Guid questionMonthlyId)
        {
            try
            {
                if (await _optionMonthlyService.GetOptionMonthlyById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _optionMonthlyService.DeleteOptionMonthly(id, questionMonthlyId);
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
