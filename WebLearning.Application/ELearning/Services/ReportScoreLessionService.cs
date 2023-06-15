using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IReportScoreLessionService
    {
        Task<IEnumerable<ReportScoreLessionDto>> GetReportScoreLessionDtos();
        Task<IEnumerable<ReportScoreLessionExport>> ExportReportScoreLessionDtos(string fromDate, string toDate, bool passed);
        Task<ReportScoreLessionDto> CheckExist(Guid quizLessionId, string accountName);

        Task<IEnumerable<ReportScoreLessionDto>> UserGetReportScoreDtos(string accountName);

        Task InsertReportScoreLessionDto(Guid quizLessionId, string accountName, CreateReportScoreLessionDto createReportScoreLessionDto);
        Task UpdateReportScoreLessionDto(Guid quizLessionId, string accountName, UpdateReportScoreLessionDto updateReportScoreLessionDto);

        Task DeleteReportScoreLessionDto(Guid id);

        Task ResetReportScoreLessionDto(Guid quizLessionId, string accountName);

        Task<PagedViewModel<ReportScoreLessionExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
    }
    public class ReportScoreLessionService : IReportScoreLessionService
    {
        private readonly WebLearningContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IQuizLessionService _quizService;
        private readonly ILessionService _lessionService;
        private readonly IAccountService _accountService;

        private readonly IConfiguration _configuration;
        public ReportScoreLessionService(WebLearningContext context, IMapper mapper, IQuizLessionService quizService, ILessionService lessionService, IAccountService accountService,
                                         IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _quizService = quizService;
            _lessionService = lessionService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<ReportScoreLessionDto> CheckExist(Guid quizLessionId, string accountName)
        {
            var answer = await _context.ReportUsersScore.AsNoTracking().FirstOrDefaultAsync(x => x.QuizLessionId.Equals(quizLessionId) && x.UserName == accountName);
            return _mapper.Map<ReportScoreLessionDto>(answer);

        }
        public Task DeleteReportScoreLessionDto(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<ReportScoreLessionExport>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:ReportScoreLession"));
            }
            var pageResult = getListPagingRequest.PageSize; var pageCount = Math.Ceiling(_context.ReportUsersScore.Count() / (double)pageResult);
            var query = _context.ReportUsersScore.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }

            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new ReportScoreLessionExport()
                                    {
                                        Id = x.Id,
                                        LessionId = x.LessionId,
                                        QuizLessionId = x.QuizLessionId,
                                        UserName = x.UserName,
                                        TotalScore = x.TotalScore,
                                        CompletedDate = x.CompletedDate,
                                        FullName = x.FullName,
                                        Passed = x.Passed,
                                        IpAddress = x.IpAddress,
                                    })
                                    .OrderByDescending(x => x.CompletedDate).ToListAsync();
            var reportScore = new PagedViewModel<ReportScoreLessionExport>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return reportScore;
        }
        public async Task<IEnumerable<ReportScoreLessionDto>> GetReportScoreLessionDtos()
        {
            var report = await _context.ReportUsersScore.OrderByDescending(x => x.CompletedDate).AsNoTracking().ToListAsync();

            var reportDto = _mapper.Map<List<ReportScoreLessionDto>>(report);
            return reportDto;
        }
        public async Task<IEnumerable<ReportScoreLessionDto>> UserGetReportScoreDtos(string accountName)
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
                    var reportScore = await _context.ReportUsersScore.Include(x => x.QuizLessions).ThenInclude(x => x.Lession).OrderByDescending(x => x.CompletedDate).Where(x => x.UserName.Equals(accountName)).AsNoTracking().ToListAsync();

                    var reportScoreDto = _mapper.Map<List<ReportScoreLessionDto>>(reportScore);



                    return reportScoreDto;
                }
            }
        }
        public async Task InsertReportScoreLessionDto(Guid quizLessionId, string accountName, CreateReportScoreLessionDto createReportScoreLessionDto)
        {
            var quizDto = await _quizService.GetQuizLessionById(quizLessionId, accountName);

            var lessionId = quizDto.LessionId;

            var fullName = await _accountService.GetNameUser(accountName);

            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ipAddress == "::1")
            {
                ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            }
            var point = 0;

            if (quizDto != null)
            {
                foreach (var item in quizDto.HistorySubmitLessionDtos)
                {
                    point += item.TotalScore;
                }
                createReportScoreLessionDto.TotalScore = point;

                if (createReportScoreLessionDto.TotalScore > 0 && createReportScoreLessionDto.TotalScore >= quizDto.ScorePass)
                {
                    createReportScoreLessionDto.Passed = true;
                }
                else
                {
                    createReportScoreLessionDto.Passed = false;
                }
            }


            createReportScoreLessionDto.Id = Guid.NewGuid();

            createReportScoreLessionDto.LessionId = quizDto.LessionId;

            createReportScoreLessionDto.QuizLessionId = quizLessionId;

            createReportScoreLessionDto.UserName = accountName;

            createReportScoreLessionDto.FullName = fullName.accountDetailDto.FullName;

            createReportScoreLessionDto.IpAddress = ipAddress;

            createReportScoreLessionDto.CompletedDate = DateTime.Now;

            ReportUserScore reportScore = _mapper.Map<ReportUserScore>(createReportScoreLessionDto);

            _context.Add(reportScore);

            await _context.SaveChangesAsync();
        }


        public async Task UpdateReportScoreLessionDto(Guid quizLessionId, string accountName, UpdateReportScoreLessionDto updateReportScoreLessionDto)
        {
            var quizDto = await _quizService.GetQuizLessionById(quizLessionId, accountName);

            var lessionId = quizDto.LessionId;

            var point = 0;

            if (quizDto != null)
            {
                foreach (var item in quizDto.HistorySubmitLessionDtos)
                {
                    point += item.TotalScore;
                }
                updateReportScoreLessionDto.TotalScore = point;

                if (updateReportScoreLessionDto.TotalScore > 0 && updateReportScoreLessionDto.TotalScore >= quizDto.ScorePass)
                {
                    updateReportScoreLessionDto.Passed = true;
                }
                else
                {
                    updateReportScoreLessionDto.Passed = false;
                }
            }
            updateReportScoreLessionDto.QuizLessionId = quizLessionId;

            updateReportScoreLessionDto.UserName = accountName;

            updateReportScoreLessionDto.CompletedDate = DateTime.Now;

            ReportUserScore reportScore = _mapper.Map<ReportUserScore>(updateReportScoreLessionDto);

            _context.Update(reportScore);

            await _context.SaveChangesAsync();
        }

        public async Task ResetReportScoreLessionDto(Guid quizLessionId, string accountName)
        {
            var report = _context.ReportUsersScore.Where(x => x.QuizLessionId.Equals(quizLessionId) && x.UserName.Equals(accountName));

            if (report != null)
            {
                _context.ReportUsersScore.RemoveRange(report);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ReportScoreLessionExport>> ExportReportScoreLessionDtos(string fromDate, string toDate, bool passed)
        {
            var report = await _context.ReportUsersScore.OrderByDescending(x => x.CompletedDate).AsNoTracking().ToListAsync();

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateTime start = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                report = await _context.ReportUsersScore.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate >= start && x.CompletedDate <= endDate).ToListAsync();
            }
            else
            {
                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime start = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                    report = await _context.ReportUsersScore.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate >= start).ToListAsync();
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.GetCultureInfo("vi-VN"));

                    report = await _context.ReportUsersScore.OrderByDescending(x => x.CompletedDate).Where(x => x.CompletedDate <= endDate).ToListAsync();
                }
            }

            //if (!string.IsNullOrEmpty(passed.ToString()))
            //{

            //    report = await _context.ReportUsersScore.OrderByDescending(x => x.CompletedDate).Where(x => x.Passed == passed).ToListAsync();
            //}
            var reportDto = _mapper.Map<List<ReportScoreLessionDto>>(report);

            var reportScoreLessionExports = new List<ReportScoreLessionExport>();
            foreach (var reportDB in reportDto)
            {
                var nameQuiz = await _quizService.GetNameQuiz(reportDB.QuizLessionId);

                var nameLession = await _lessionService.GetName(reportDB.LessionId);
                reportScoreLessionExports.Add(new ReportScoreLessionExport()
                {
                    Id = reportDB.Id,
                    QuizName = nameQuiz.Name,
                    LessionName = nameLession.Name,
                    FullName = reportDB.FullName,
                    UserName = reportDB.UserName,
                    CompletedDate = reportDB.CompletedDate,
                    TotalScore = reportDB.TotalScore,
                    Passed = reportDB.Passed,
                    IpAddress = reportDB.IpAddress,
                });
            }

            return reportScoreLessionExports;
        }
    }
}
