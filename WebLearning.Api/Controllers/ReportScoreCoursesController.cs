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
    public class ReportScoreCoursesController : ControllerBase
    {
        private readonly ILogger<ReportScoreCoursesController> _logger;
        private readonly IReportScoreCourseService _ReportScoreCourseService;

        public ReportScoreCoursesController(ILogger<ReportScoreCoursesController> logger, IReportScoreCourseService ReportScoreCourseService)
        {
            _logger = logger;
            _ReportScoreCourseService = ReportScoreCourseService;
        }
        // GET: api/<ReportScoreCoursesController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<ReportScoreCourseDto>> Get()
        {
            return await _ReportScoreCourseService.GetReportScoreCourseDtos();
        }
        [HttpGet("ExportExcel")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IEnumerable<ReportScoreCourseExport>> ExportV2(CancellationToken cancellationToken, string fromDate, string toDate, bool passed)
        {
            var data = await _ReportScoreCourseService.ExportReportScoreCourseDtos(fromDate, toDate, passed);

            if (data != null)
            {
                return data;
            }
            return default;
        }
        [HttpGet("GetCertificate/{quizCourseId}/{accountName}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<CertificateCourse> GetCertificate(Guid quizCourseId, string accountName)
        {
            var result = await _ReportScoreCourseService.GetCertificate(quizCourseId, accountName);

            if (result == null)
                return default;
            return result;

        }
        [HttpGet("ReportScoreCoursePaging")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<ReportScoreCourseExport>> GetPagingReportScoreCourse([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _ReportScoreCourseService.GetPaging(getListPagingRequest);

        }
        [HttpGet("UserGetReportScores")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<ReportScoreCourseDto>> UserGetReportScore(string accountName)
        {
            return await _ReportScoreCourseService.UserGetReportScoreDtos(accountName);
        }
        // GET api/<ReportScoreCoursesController>/5
        [HttpGet("CheckExist")]

        public async Task<IActionResult> GetReportScoreCourseById(Guid quizCourseId, string accountName)
        {
            if (await _ReportScoreCourseService.CheckExistReportCourse(quizCourseId, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _ReportScoreCourseService.CheckExistReportCourse(quizCourseId, accountName));

        }
        [HttpPost]

        public async Task<IActionResult> Post(Guid quizCourseId, string accountName, [FromBody] CreateReportScoreCourseDto createReportScoreCourseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }

                if (createReportScoreCourseDto != null)
                {
                    createReportScoreCourseDto.Id = Guid.NewGuid();

                    await _ReportScoreCourseService.InsertReportScoreCourseDto(quizCourseId, accountName, createReportScoreCourseDto);

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
