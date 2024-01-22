using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Notification;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface INotificationService
    {

        Task<IEnumerable<NotificationResponseDto>> GetNotificationResponses(string accountName);

        Task<IEnumerable<NotificationResponseDto>> ShowNotification(string accountName);


        Task InsertNotificationResponse(CreateNotificationResponseDto createNotificationResponseDto, string accountName);

        Task UpdateNotificationResponse(UpdateNotificationResponseDto updateNotificationResponseDto, Guid id, string accountName);

        Task<NotificationResponseDto> GetResponseDetail(Guid id, string accountName);

        Task<List<NotificationResponseDto>> GetListResponse(string accountName);

        Task DeleteAllNotificationResponse(string accountName);

        Task DeleteItemInResponse(Guid id, string accountName);



    }
    public class NotificationService : INotificationService
    {
        private readonly WebLearningContext _context;

        private readonly IMapper _mapper;

        private readonly IAccountService _accountService;

        public NotificationService(WebLearningContext context, IMapper mapper, IAccountService accountService)
        {
            _context = context;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task UpdateNotificationResponse(UpdateNotificationResponseDto updateNotificationResponseDto, Guid id, string accountName)
        {
            var item = await _context.NotificationResponses.FirstOrDefaultAsync(x => x.TargetNotificationId.Equals(id) && x.AccountName.Equals(accountName));
            if (item != null)
            {
                updateNotificationResponseDto.Notify = false;
                _context.NotificationResponses.Update(_mapper.Map(updateNotificationResponseDto, item));

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationResponses(string accountName)
        {
            var getDetail = await _accountService.GetNameUser(accountName);

            var allCourse = await _context.Courses.Where(x => x.Active == true).Include(x => x.CourseImageVideo).Include(x => x.QuizCourse).Include(x => x.CourseRoles).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            var allLession = await _context.Lessions.Include(x => x.LessionVideoImages).Include(x => x.Quizzes).AsNoTracking().ToListAsync();

            var allQuizMonthly = await _context.QuizMonthlies.Where(x => x.Active == true).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

            var allCourseDto = _mapper.Map<List<CourseDto>>(allCourse);

            var allLessionDto = _mapper.Map<List<LessionDto>>(allLession);

            var allQuizMonthlyDto = _mapper.Map<List<QuizMonthlyDto>>(allQuizMonthly);

            List<NotificationResponseDto> notificationResponseDtos = new();


            foreach (var course in allCourseDto.Where(x => x.CourseRoleDtos.Any(x => x.RoleId.Equals(getDetail.RoleId)) && x.Active == true))
            {

                notificationResponseDtos.Add(new NotificationResponseDto
                {
                    Id = Guid.NewGuid(),

                    TargetNotificationId = course.Id,

                    FatherTargetId = course.Id,

                    FatherAlias = course.Alias,

                    AccountName = getDetail.Email,

                    TargetImagePath = course.CourseImageVideoDto.FirstOrDefault(x => x.CourseId.Equals(course.Id)).ImagePath,

                    TargetName = course.Name,

                    DateCreated = course.DateCreated,

                    RoleId = getDetail.RoleId,

                    Alias = course.Alias,

                    Notify = course.Notify,

                    DescNotify = course.DescNotify,
                });


                if (course.QuizCourseDto != null)
                {
                    if (course.QuizCourseDto.Notify == true && course.QuizCourseDto.Active == true && course.QuizCourseDto.CourseId.Equals(course.Id))
                    {
                        notificationResponseDtos.Add(new NotificationResponseDto
                        {
                            Id = Guid.NewGuid(),

                            TargetNotificationId = course.QuizCourseDto.ID,

                            FatherTargetId = course.Id,

                            FatherAlias = course.Alias,

                            AccountName = getDetail.Email,

                            TargetName = course.QuizCourseDto.Name,

                            TargetImagePath = course.CourseImageVideoDto.FirstOrDefault(x => x.CourseId.Equals(course.Id)).ImagePath,

                            DateCreated = course.QuizCourseDto.DateCreated,

                            RoleId = getDetail.RoleId,

                            Alias = course.QuizCourseDto.Alias,

                            Notify = course.QuizCourseDto.Notify,

                            DescNotify = course.QuizCourseDto.DescNotify,

                        });
                    }
                }

                foreach (var lession in allLessionDto.Where(x => x.Notify == true && x.Active == true && x.CourseId.Equals(course.Id)))
                {

                    notificationResponseDtos.Add(new NotificationResponseDto
                    {
                        Id = Guid.NewGuid(),

                        TargetNotificationId = lession.Id,

                        FatherAlias = course.Alias,

                        FatherTargetId = course.Id,

                        AccountName = getDetail.Email,

                        TargetName = lession.Name,

                        TargetImagePath = course.CourseImageVideoDto.FirstOrDefault(x => x.CourseId.Equals(course.Id)).ImagePath,

                        DateCreated = lession.DateCreated,

                        RoleId = getDetail.RoleId,

                        Alias = lession.Alias,

                        Notify = lession.Notify,

                        DescNotify = lession.DescNotify,
                    });

                    foreach (var videoLession in lession.LessionVideoDtos.Where(x => x.Notify == true && x.LessionId.Equals(lession.Id)))
                    {
                        notificationResponseDtos.Add(new NotificationResponseDto
                        {
                            Id = Guid.NewGuid(),

                            TargetNotificationId = videoLession.Id,

                            FatherTargetId = course.Id,

                            FatherAlias = course.Alias,

                            AccountName = getDetail.Email,

                            TargetName = videoLession.Caption,

                            DateCreated = videoLession.DateCreated,

                            TargetImagePath = course.CourseImageVideoDto.FirstOrDefault(x => x.CourseId.Equals(course.Id)).ImagePath,

                            RoleId = getDetail.RoleId,

                            Alias = lession.Alias,

                            Notify = videoLession.Notify,

                            DescNotify = videoLession.DescNotify,

                        });
                    }
                    if (lession.QuizlessionDtos != null)
                    {
                        foreach (var quizLession in lession.QuizlessionDtos.Where(x => x.Notify == true && x.Active == true && x.LessionId.Equals(lession.Id)))
                        {
                            notificationResponseDtos.Add(new NotificationResponseDto
                            {
                                Id = Guid.NewGuid(),

                                TargetNotificationId = quizLession.ID,

                                FatherTargetId = course.Id,

                                FatherAlias = course.Alias,

                                AccountName = getDetail.Email,

                                Notify = quizLession.Notify,

                                TargetImagePath = course.CourseImageVideoDto.FirstOrDefault(x => x.CourseId.Equals(course.Id)).ImagePath,

                                TargetName = quizLession.Name,

                                RoleId = getDetail.RoleId,

                                Alias = quizLession.Alias,

                                DateCreated = quizLession.DateCreated,

                                DescNotify = quizLession.DescNotify,

                            });
                        }
                    }

                }

            }

            foreach (var quizMonthly in allQuizMonthlyDto.Where(x => x.RoleId.Equals(getDetail.RoleId)).OrderByDescending(x => x.DateCreated))
            {
                if (quizMonthly != null && quizMonthly.Active == true)
                {
                    notificationResponseDtos.Add(new NotificationResponseDto
                    {
                        Id = Guid.NewGuid(),

                        TargetNotificationId = quizMonthly.ID,

                        FatherTargetId = quizMonthly.ID,

                        FatherAlias = quizMonthly.Alias,

                        AccountName = getDetail.Email,

                        Notify = quizMonthly.Notify,

                        TargetName = quizMonthly.Name,

                        DateCreated = quizMonthly.DateCreated,

                        RoleId = getDetail.RoleId,

                        Alias = quizMonthly.Alias,

                        DescNotify = quizMonthly.DescNotify,
                    });

                }
            }

            return notificationResponseDtos;


        }

        public async Task InsertNotificationResponse(CreateNotificationResponseDto createNotificationResponseDto, string accountName)
        {
            var list = await GetNotificationResponses(accountName);

            foreach (var item in list)
            {
                if (_context.NotificationResponses.Any(x => x.TargetNotificationId.Equals(item.TargetNotificationId) && x.AccountName.Equals(item.AccountName)) == false)
                {
                    createNotificationResponseDto.Id = item.Id;

                    createNotificationResponseDto.AccountName = item.AccountName;

                    createNotificationResponseDto.Notify = item.Notify;

                    createNotificationResponseDto.DescNotify = item.DescNotify;

                    createNotificationResponseDto.TargetNotificationId = item.TargetNotificationId;

                    createNotificationResponseDto.DateCreated = item.DateCreated;

                    createNotificationResponseDto.TargetName = item.TargetName;

                    createNotificationResponseDto.TargetImagePath = item.TargetImagePath;

                    createNotificationResponseDto.RoleId = item.RoleId;

                    createNotificationResponseDto.Alias = item.Alias;

                    createNotificationResponseDto.FatherAlias = item.FatherAlias;

                    createNotificationResponseDto.FatherTargetId = item.FatherTargetId;

                    NotificationResponse notificationResponse = _mapper.Map<NotificationResponse>(createNotificationResponseDto);

                    _context.NotificationResponses.Add(notificationResponse);

                    await _context.SaveChangesAsync();
                }


            }

        }

        public async Task<IEnumerable<NotificationResponseDto>> ShowNotification(string accountName)
        {
            if (accountName != null)
            {

                CreateNotificationResponseDto createNotificationResponseAccountDto = new CreateNotificationResponseDto();

                var total = await GetNotificationResponses(accountName);

                if (total.Any())
                {
                    await InsertNotificationResponse(createNotificationResponseAccountDto, accountName);
                }

                var db = await _context.NotificationResponses.Where(x => x.AccountName.Equals(accountName)).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();

                var notificationResponseDto = _mapper.Map<List<NotificationResponseDto>>(db);

                return notificationResponseDto;

            }
            return default;
        }

        public async Task DeleteAllNotificationResponse(string accountName)
        {
            var notification = _context.NotificationResponses.Where(x => x.AccountName.Equals(accountName));

            _context.NotificationResponses.RemoveRange(notification);

            await _context.SaveChangesAsync();
        }

        public async Task<NotificationResponseDto> GetResponseDetail(Guid id, string accountName)
        {
            var notification = await _context.NotificationResponses.FirstOrDefaultAsync(x => x.TargetNotificationId.Equals(id) && x.AccountName.Equals(accountName));

            return _mapper.Map<NotificationResponseDto>(notification);
        }

        public async Task<List<NotificationResponseDto>> GetListResponse(string accountName)
        {
            var notification = await _context.NotificationResponses.Where(x => x.AccountName.Equals(accountName)).OrderByDescending(x => x.DateCreated).ToListAsync();

            return _mapper.Map<List<NotificationResponseDto>>(notification);
        }

        public async Task DeleteItemInResponse(Guid id, string accountName)
        {
            var notification = await _context.NotificationResponses.FirstOrDefaultAsync(x => x.TargetNotificationId.Equals(id) && x.AccountName.Equals(accountName));

            _context.NotificationResponses.Remove(notification);

            await _context.SaveChangesAsync();
        }
    }
}
