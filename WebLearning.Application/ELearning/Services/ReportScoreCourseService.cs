using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Certificate;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IReportScoreCourseService
    {
        Task<IEnumerable<ReportScoreCourseDto>> GetReportScoreCourseDtos();

        Task<IEnumerable<ReportScoreCourseExport>> ExportReportScoreCourseDtos(string fromDate, string toDate, bool passed);

        Task<ReportScoreCourseDto> CheckExistReportCourse(Guid quizCourseId, string accountName);

        Task<CertificateCourse> GetCertificate(Guid quizCourseId, string accountName);
        Task<IEnumerable<ReportScoreCourseDto>> UserGetReportScoreDtos(string accountName);

        Task InsertReportScoreCourseDto(Guid quizCourseId, string accountName, CreateReportScoreCourseDto createReportScoreCourseDto);

        Task DeleteReportScoreCourseDto(Guid id);

        Task<PagedViewModel<ReportScoreCourseExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

    }
    public class ReportScoreCourseService : IReportScoreCourseService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IQuizCourseService _quizService;
        private readonly ICourseService _CourseService;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ReportScoreCourseService(WebLearningContext context, IMapper mapper, IConfiguration configuration, IQuizCourseService quizService, ICourseService CourseService, IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _quizService = quizService;
            _CourseService = CourseService;
            _accountService = accountService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task DeleteReportScoreCourseDto(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<ReportScoreCourseExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:ReportScoreCourse");
            var pageCount = Math.Ceiling(_context.ReportUserScoreFinals.Count() / (double)pageResult);
            var query = _context.ReportUserScoreFinals.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new ReportScoreCourseExport()
                                    {
                                        Id = x.Id,
                                        QuizCourseId = x.QuizCourseId,
                                        CourseId = x.CourseId,
                                        UserName = x.UserName,
                                        TotalScore = x.TotalScore,
                                        CompletedDate = x.CompletedDate,
                                        FullName = x.FullName,
                                        Passed = x.Passed,
                                        IpAddress = x.IpAddress,
                                    })
                                    .OrderByDescending(x => x.CompletedDate).ToListAsync();
            var reportScore = new PagedViewModel<ReportScoreCourseExport>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return reportScore;
        }

        public async Task<IEnumerable<ReportScoreCourseDto>> UserGetReportScoreDtos(string accountName)
        {
            var checkGuest = await _accountService.GetUserByKeyWord(accountName);
            if (checkGuest == null)
            {
                return default;
            }
            else
            {

                if (checkGuest.AccountDto.AuthorizeRole.Equals(AuthorizeRole.Guest))
                {
                    return default;
                }
                else
                {
                    var reportScore = await _context.ReportUserScoreFinals.OrderByDescending(x => x.CompletedDate).Where(x => x.UserName.Equals(accountName)).AsNoTracking().ToListAsync();
                    var reportScoreDto = _mapper.Map<List<ReportScoreCourseDto>>(reportScore);
                    return reportScoreDto;
                }
            }
        }
        public async Task<IEnumerable<ReportScoreCourseDto>> GetReportScoreCourseDtos()
        {
            var report = await _context.ReportUserScoreFinals.OrderByDescending(x => x.CompletedDate).AsNoTracking().ToListAsync();

            var reportDto = _mapper.Map<List<ReportScoreCourseDto>>(report);

            return reportDto;
        }

        public async Task InsertReportScoreCourseDto(Guid quizCourseId, string accountName, CreateReportScoreCourseDto createReportScoreCourseDto)
        {
            var quizDto = await _quizService.GetQuizById(quizCourseId, accountName);

            var CourseId = quizDto.CourseId;

            var fullName = await _accountService.GetNameUser(accountName);

            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ipAddress == "::1")
            {
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            }

            var point = 0;

            if (quizDto != null)
            {
                foreach (var item in quizDto.HistorySubmitCourseDtos)
                {
                    point += item.TotalScore;
                }
                createReportScoreCourseDto.TotalScore = point;

                if (createReportScoreCourseDto.TotalScore > 0 && createReportScoreCourseDto.TotalScore >= quizDto.ScorePass)
                {
                    createReportScoreCourseDto.Passed = true;
                }
                else
                {
                    createReportScoreCourseDto.Passed = false;
                }
            }


            createReportScoreCourseDto.Id = Guid.NewGuid();

            createReportScoreCourseDto.CourseId = quizDto.CourseId;

            createReportScoreCourseDto.FullName = fullName.accountDetailDto.FullName;

            createReportScoreCourseDto.IpAddress = ipAddress;

            createReportScoreCourseDto.QuizCourseId = quizCourseId;

            createReportScoreCourseDto.UserName = accountName;

            createReportScoreCourseDto.CompletedDate = DateTime.Now;

            ReportUserScoreFinal reportScore = _mapper.Map<ReportUserScoreFinal>(createReportScoreCourseDto);

            _context.Add(reportScore);

            await _context.SaveChangesAsync();
        }

        public async Task<ReportScoreCourseDto> CheckExistReportCourse(Guid quizCourseId, string accountName)
        {
            var answer = await _context.ReportUserScoreFinals.AsNoTracking().FirstOrDefaultAsync(x => x.QuizCourseId.Equals(quizCourseId) && x.UserName == accountName);
            return _mapper.Map<ReportScoreCourseDto>(answer);

        }

        public async Task<CertificateCourse> GetCertificate(Guid quizCourseId, string accountName)
        {
            var report = await _context.ReportUserScoreFinals.FirstOrDefaultAsync(x => x.QuizCourseId.Equals(quizCourseId) && x.UserName.Equals(accountName));

            var infoUser = await _accountService.GetNameUser(report.UserName);

            CertificateCourse certificate = new();

            certificate.NameQuiz = (await _quizService.GetNameQuiz(quizCourseId)).Name;

            certificate.NameCourse = (await _CourseService.GetCourseById(report.CourseId)).Name;

            certificate.FullName = infoUser.accountDetailDto.FullName;

            certificate.RoleName = infoUser.roleDto.Description;

            certificate.CompletedDate = report.CompletedDate;

            certificate.TotalScore = report.TotalScore;

            return certificate;
        }
        public async Task<IEnumerable<ReportScoreCourseExport>> ExportReportScoreCourseDtos(string fromDate, string toDate, bool passed)
        {
            var report = await _context.ReportUserScoreFinals.OrderByDescending(x => x.CompletedDate).AsNoTracking().ToListAsync();
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateTime start = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                report = await _context.ReportUserScoreFinals.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate >= start && x.CompletedDate <= endDate).ToListAsync();
            }
            else
            {
                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime start = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                    report = await _context.ReportUserScoreFinals.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate >= start).ToListAsync();
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                    report = await _context.ReportUserScoreFinals.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate <= endDate).ToListAsync();
                }
            }

            //if (!string.IsNullOrEmpty(passed.ToString()))
            //{

            //    report = await _context.ReportUserScoreFinals.OrderByDescending(x => x.CompletedDate).Where(x => x.Passed == passed).ToListAsync();
            //}
            var reportDto = _mapper.Map<List<ReportScoreCourseDto>>(report);

            var ReportScoreCourseExports = new List<ReportScoreCourseExport>();
            foreach (var reportDB in reportDto)
            {
                var nameQuiz = await _quizService.GetNameQuiz(reportDB.QuizCourseId);

                var nameCourse = await _CourseService.GetName(reportDB.CourseId);
                ReportScoreCourseExports.Add(new ReportScoreCourseExport()
                {
                    Id = reportDB.Id,
                    QuizName = nameQuiz.Name,
                    CourseName = nameCourse.Name,
                    FullName = reportDB.FullName,
                    UserName = reportDB.UserName,
                    CompletedDate = reportDB.CompletedDate,
                    TotalScore = reportDB.TotalScore,
                    Passed = reportDB.Passed,
                    IpAddress = reportDB.IpAddress,
                });
            }

            return ReportScoreCourseExports;
        }
    }
}
