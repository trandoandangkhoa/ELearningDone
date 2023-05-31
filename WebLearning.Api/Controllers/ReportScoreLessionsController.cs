using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.ReportScore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportScoreLessionsController : ControllerBase
    {
        private readonly ILogger<ReportScoreLessionsController> _logger;
        private readonly IReportScoreLessionService _ReportScoreLessionService;

        public ReportScoreLessionsController(ILogger<ReportScoreLessionsController> logger, IReportScoreLessionService ReportScoreLessionService)
        {
            _logger = logger;
            _ReportScoreLessionService = ReportScoreLessionService;
        }
        [HttpGet("ExportExcel")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IEnumerable<ReportScoreLessionExport>> ExportV2(CancellationToken cancellationToken, string fromDate, string toDate, bool passed)
        {
            var data = await _ReportScoreLessionService.ExportReportScoreLessionDtos(fromDate, toDate, passed);

            if (data != null)
            {
                return data;
            }
            return default;
        }

        // GET: api/<ReportScoreLessionsController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<ReportScoreLessionDto>> Get()
        {
            return await _ReportScoreLessionService.GetReportScoreLessionDtos();
        }
        [HttpGet("ReportScoreLessionPaging")]

        public async Task<PagedViewModel<ReportScoreLessionExport>> GetPagingReportScoreLession([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _ReportScoreLessionService.GetPaging(getListPagingRequest);

        }
        [HttpGet("UserGetReportScores")]

        public async Task<IEnumerable<ReportScoreLessionDto>> UserGetReportScore(string accountName)
        {
            return await _ReportScoreLessionService.UserGetReportScoreDtos(accountName);
        }
        // POST api/<ReportScoreLessionsController>
        [HttpGet("CheckExist")]

        public async Task<IActionResult> GetReportScoreLessionById(Guid quizLessionId, string accountName)
        {
            if (await _ReportScoreLessionService.CheckExist(quizLessionId, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _ReportScoreLessionService.CheckExist(quizLessionId, accountName));

        }
        [HttpPost]

        public async Task<IActionResult> Post(Guid quizLessionId, string accountName, [FromBody] CreateReportScoreLessionDto createReportScoreLessionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createReportScoreLessionDto != null)
                {
                    createReportScoreLessionDto.Id = Guid.NewGuid();

                    await _ReportScoreLessionService.InsertReportScoreLessionDto(quizLessionId, accountName, createReportScoreLessionDto);

                    return StatusCode(StatusCodes.Status200OK);
                }

                return StatusCode(StatusCodes.Status400BadRequest);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // PUT api/<ReportScoreLessionsController>/5
        [HttpPut]

        public async Task<IActionResult> UpdateReportScoreLession(Guid quizLessionId, string accountName, [FromBody] UpdateReportScoreLessionDto updateReportScoreLessionDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _ReportScoreLessionService.UpdateReportScoreLessionDto(quizLessionId, accountName, updateReportScoreLessionDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        [HttpDelete("ResetAllReport/{quizLessionId}")]

        public async Task<IActionResult> ResetAllHistory(Guid quizLessionId, string accountName)
        {
            try
            {
                await _ReportScoreLessionService.ResetReportScoreLessionDto(quizLessionId, accountName);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
