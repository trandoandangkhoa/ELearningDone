using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Certificate;
using WebLearning.Contract.Dtos.ReportScore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportScoreMonthliesController : ControllerBase
    {
        private readonly ILogger<ReportScoreMonthliesController> _logger;
        private readonly IReportScoreMonthlyService _ReportScoreMonthlyService;

        public ReportScoreMonthliesController(ILogger<ReportScoreMonthliesController> logger, IReportScoreMonthlyService ReportScoreMonthlyService)
        {
            _logger = logger;
            _ReportScoreMonthlyService = ReportScoreMonthlyService;
        }
        [HttpGet("ExportExcel")]
        public async Task<IEnumerable<ReportScoreMonthlyExport>> ExportV2(CancellationToken cancellationToken, string fromDate, string toDate, bool passed)
        {
            var data = await _ReportScoreMonthlyService.ExportReportScoreMonthlyDtos(fromDate, toDate, passed);

            if (data != null)
            {
                return data;
            }
            return default;
        }
        // GET: api/<ReportScoreMonthlysController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<ReportScoreMonthlyDto>> Get()
        {
            return await _ReportScoreMonthlyService.GetReportScoreMonthlyDtos();
        }
        [HttpGet("ReportScoreMonthlyPaging")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<ReportScoreMonthlyExport>> GetPagingReportScoreMonthly([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _ReportScoreMonthlyService.GetPaging(getListPagingRequest);

        }
        [HttpGet("GetCertificate/{quizMonthlyId}/{accountName}")]

        public async Task<CertificateMonthly> GetCertificate(Guid quizMonthlyId, string accountName)
        {
            var result = await _ReportScoreMonthlyService.GetCertificate(quizMonthlyId, accountName);

            if (result == null)
                return default;

            return result;

        }
        [HttpGet("UserGetReportScores")]
        public async Task<IEnumerable<ReportScoreMonthlyDto>> UserGetReportScore(string accountName)
        {
            return await _ReportScoreMonthlyService.UserGetReportScoreDtos(accountName);
        }
        // GET api/<ReportScoreMonthlysController>/5
        [HttpGet("CheckExist")]

        public async Task<IActionResult> GetReportScoreMonthlyById(Guid quizMonthlyId, string accountName)
        {
            if (await _ReportScoreMonthlyService.CheckExistReportMonthly(quizMonthlyId, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _ReportScoreMonthlyService.CheckExistReportMonthly(quizMonthlyId, accountName));

        }
        [HttpPost]

        public async Task<IActionResult> Post(Guid quizMonthlyId, string accountName, [FromBody] CreateReportScoreMonthlyDto createReportScoreMonthlyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createReportScoreMonthlyDto != null)
                {
                    createReportScoreMonthlyDto.Id = Guid.NewGuid();

                    await _ReportScoreMonthlyService.InsertReportScoreMonthlyDto(quizMonthlyId, accountName, createReportScoreMonthlyDto);

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
    }
}
