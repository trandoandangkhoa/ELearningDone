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

namespace WebLearning.Application.Services
{
    public interface IReportScoreMonthlyService
    {
        Task<IEnumerable<ReportScoreMonthlyDto>> GetReportScoreMonthlyDtos();

        Task<ReportScoreMonthlyDto> CheckExistReportMonthly(Guid quizMonthlyId, string accountName);

        Task<IEnumerable<ReportScoreMonthlyExport>> ExportReportScoreMonthlyDtos(string fromDate, string toDate, bool passed);

        Task<IEnumerable<ReportScoreMonthlyDto>> UserGetReportScoreDtos(string accountName);

        Task InsertReportScoreMonthlyDto(Guid quizMonthlyId, string accountName, CreateReportScoreMonthlyDto createReportScoreMonthlyDto);

        Task DeleteReportScoreMonthlyDto(Guid id);

        Task<CertificateMonthly> GetCertificate(Guid quizCourseId, string accountName);

        Task<PagedViewModel<ReportScoreMonthlyExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class ReportScoreMonthlyService : IReportScoreMonthlyService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IQuizMonthlyService _quizService;
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ReportScoreMonthlyService(WebLearningContext context, IMapper mapper, IQuizMonthlyService quizService, IRoleService roleService, IAccountService accountService
                                        , IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _quizService = quizService;
            _roleService = roleService;
            _accountService = accountService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task DeleteReportScoreMonthlyDto(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<ReportScoreMonthlyExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:ReportScoreMonthly");
            var pageCount = Math.Ceiling(_context.ReportUserScoreMonthlies.Count() / (double)pageResult);
            var query = _context.ReportUserScoreMonthlies.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new ReportScoreMonthlyExport()
                                    {
                                        Id = x.Id,
                                        QuizMonthlyId = x.QuizMonthlyId,
                                        RoleId = x.RoleId,
                                        UserName = x.UserName,
                                        TotalScore = x.TotalScore,
                                        CompletedDate = x.CompletedDate,
                                        FullName = x.FullName,
                                        Passed = x.Passed,
                                        IpAddress = x.IpAddress,
                                    })
                                    .OrderByDescending(x => x.CompletedDate).ToListAsync();
            var reportScore = new PagedViewModel<ReportScoreMonthlyExport>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return reportScore;
        }

        public async Task<IEnumerable<ReportScoreMonthlyDto>> UserGetReportScoreDtos(string accountName)
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
                    var reportScore = await _context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).Where(x => x.UserName.Equals(accountName)).AsNoTracking().ToListAsync();
                    var reportScoreDto = _mapper.Map<List<ReportScoreMonthlyDto>>(reportScore);
                    return reportScoreDto;
                }
            }
        }
        public async Task<IEnumerable<ReportScoreMonthlyDto>> GetReportScoreMonthlyDtos()
        {
            var report = await _context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).AsNoTracking().ToListAsync();

            var reportDto = _mapper.Map<List<ReportScoreMonthlyDto>>(report);

            return reportDto;
        }

        public async Task InsertReportScoreMonthlyDto(Guid quizMonthlyId, string accountName, CreateReportScoreMonthlyDto createReportScoreMonthlyDto)
        {
            var quizDto = await _quizService.GetQuizById(quizMonthlyId, accountName);

            var roleId = quizDto.RoleId;

            var fullName = await _accountService.GetNameUser(accountName);

            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ipAddress == "::1")
            {
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            }

            var point = 0;

            if (quizDto != null)
            {
                foreach (var item in quizDto.HistorySubmitMonthlyDtos)
                {
                    point += item.TotalScore;
                }
                createReportScoreMonthlyDto.TotalScore = point;

                if (createReportScoreMonthlyDto.TotalScore > 0 && createReportScoreMonthlyDto.TotalScore >= quizDto.ScorePass)
                {
                    createReportScoreMonthlyDto.Passed = true;
                }
                else
                {
                    createReportScoreMonthlyDto.Passed = false;
                }
            }


            createReportScoreMonthlyDto.Id = Guid.NewGuid();

            createReportScoreMonthlyDto.RoleId = quizDto.RoleId;

            createReportScoreMonthlyDto.QuizMonthlyId = quizMonthlyId;

            createReportScoreMonthlyDto.UserName = accountName;

            createReportScoreMonthlyDto.FullName = fullName.accountDetailDto.FullName;

            createReportScoreMonthlyDto.IpAddress = ipAddress;

            createReportScoreMonthlyDto.CompletedDate = DateTime.Now;

            ReportUserScoreMonthly reportScore = _mapper.Map<ReportUserScoreMonthly>(createReportScoreMonthlyDto);

            _context.Add(reportScore);

            await _context.SaveChangesAsync();
        }

        public async Task<ReportScoreMonthlyDto> CheckExistReportMonthly(Guid quizMonthlyId, string accountName)
        {
            var answer = await _context.ReportUserScoreMonthlies.AsNoTracking().FirstOrDefaultAsync(x => x.QuizMonthlyId.Equals(quizMonthlyId) && x.UserName == accountName);
            return _mapper.Map<ReportScoreMonthlyDto>(answer);

        }
        public async Task<CertificateMonthly> GetCertificate(Guid quizMonthlyId, string accountName)
        {
            var report = await _context.ReportUserScoreMonthlies.FirstOrDefaultAsync(x => x.QuizMonthlyId.Equals(quizMonthlyId) && x.UserName.Equals(accountName));

            var infoUser = await _accountService.GetNameUser(report.UserName);

            CertificateMonthly certificate = new();

            certificate.NameQuiz = (await _quizService.GetNameQuiz(quizMonthlyId)).Name;

            certificate.FullName = infoUser.accountDetailDto.FullName;

            certificate.RoleName = infoUser.roleDto.Description;

            certificate.CompletedDate = report.CompletedDate;

            certificate.TotalScore = report.TotalScore;

            return certificate;
        }
        public async Task<IEnumerable<ReportScoreMonthlyExport>> ExportReportScoreMonthlyDtos(string fromDate, string toDate, bool passed)
        {
            var report = await _context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).AsNoTracking().ToListAsync();

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateTime start = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                report = await _context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate >= start && x.CompletedDate <= endDate).ToListAsync();
            }
            else
            {
                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime start = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                    report = await _context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate >= start).ToListAsync();
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                    report = await _context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate <= endDate).ToListAsync();
                }
            }

            //if (!string.IsNullOrEmpty(passed.ToString()))
            //{

            //    report = await _context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).Where(x => x.Passed == passed).ToListAsync();
            //}
            var reportDto = _mapper.Map<List<ReportScoreMonthlyDto>>(report);

            var ReportScoreMonthlyExports = new List<ReportScoreMonthlyExport>();
            foreach (var reportDB in reportDto)
            {
                var nameQuiz = await _quizService.GetNameQuiz(reportDB.QuizMonthlyId);

                ReportScoreMonthlyExports.Add(new ReportScoreMonthlyExport()
                {
                    Id = reportDB.Id,
                    QuizName = nameQuiz.Name,
                    FullName = reportDB.FullName,
                    UserName = reportDB.UserName,
                    CompletedDate = reportDB.CompletedDate,
                    TotalScore = reportDB.TotalScore,
                    Passed = reportDB.Passed,
                    IpAddress = reportDB.IpAddress,
                });
            }

            return ReportScoreMonthlyExports;
        }
    }
}
