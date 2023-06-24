using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Course.CourseAdminView;
using WebLearning.Contract.Dtos.CourseRole;
using WebLearning.Contract.Dtos.ImageCourse;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Notification;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetCourseDtos();

        Task<CourseDto> GetCourseById(Guid id);

        Task<CourseDto> UserCourse(Guid id, string accountName);
        Task<CourseDto> GetName(Guid id);

        Task InsertCourseDto(CreateCourseDto createCourseDto/*, IFormFile courseImage*/);

        Task DeleteCourseDto(Guid id);

        Task UpdateCourseAdminView(Guid id, UpdateCourseAdminView updateCourseAdminView);

        Task<PagedViewModel<CourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest, string accountName);

        Task<CourseDto> GetCode(string code);
    }
    public class CourseService : ICourseService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        private readonly IConfiguration _configuration;
        public CourseService(WebLearningContext context, IMapper mapper,
            IAccountService accountService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _accountService = accountService;
            _configuration = configuration;
        }
        public async Task DeleteCourseDto(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();

            var course = await _context.Courses.FindAsync(id);

            var imageCourse = _context.CourseImageVideos.Where(x => x.CourseId.Equals(id));

            foreach (var item in imageCourse)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "imageCourse", item.ImagePath);

                var fileExsit = File.Exists(imagePath);

                if (fileExsit == true)
                {
                    File.Delete(imagePath);

                }
            }
            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();

            var reportScore = _context.ReportUserScoreFinals.Where(x => x.CourseId.Equals(id));

            _context.ReportUserScoreFinals.RemoveRange(reportScore);
            await _context.SaveChangesAsync();

            transaction.Commit();

        }

        public async Task<CourseDto> GetCourseById(Guid id)
        {
            var course = await _context.Courses.Include(x => x.QuizCourse).ThenInclude(x => x.QuestionFinals).ThenInclude(x => x.Options).Include(x => x.CourseRoles).Include(x => x.CourseImageVideo).Include(x => x.Lessions).FirstOrDefaultAsync(x => x.Id.Equals(id));

            var courseDto = _mapper.Map<CourseDto>(course);

            return courseDto;


        }

        public async Task<CourseDto> UserCourse(Guid id, string accountName)
        {
            var course = await _context.Courses.Include(x => x.QuizCourse).Include(x => x.CourseImageVideo).Include(x => x.Lessions).Include(x => x.CourseRoles).FirstOrDefaultAsync(x => x.Id.Equals(id));

            var roleId = await _accountService.GetUserByKeyWord(accountName);

            var imageCourse = _context.CourseImageVideos.Where(x => x.CourseId.Equals(course.Id)).OrderBy(x => x.DateCreated).AsNoTracking().AsQueryable();

            var lessionCourse = _context.Lessions.Where(x => x.CourseId.Equals(course.Id)).OrderBy(x => x.DateCreated).AsNoTracking().AsQueryable();

            var quizCourse = await _context.QuizCourses.FirstOrDefaultAsync(x => x.CourseId.Equals(course.Id));

            var courseRole = _context.CourseRoles.Where(x => x.CourseId.Equals(course.Id) && x.RoleId.Equals(roleId.AccountDto.RoleId)).AsNoTracking().AsQueryable();

            var courseImageDto = _mapper.Map<List<CourseImageDto>>(imageCourse);

            var lessionDto = _mapper.Map<List<LessionDto>>(lessionCourse);

            var courseRoleDto = _mapper.Map<List<CourseRoleDto>>(courseRole);


            if (quizCourse != null)
            {
                CourseDto courseDto = new()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    Active = course.Active,

                    DateCreated = course.DateCreated,
                    Alias = course.Alias,

                    LessionDtos = new List<LessionDto>(lessionDto),

                    CourseImageVideoDto = new List<CourseImageDto>(courseImageDto),

                    QuizCourseDto = new QuizCourseDto()
                    {
                        ID = course.QuizCourse.ID,
                        CourseId = course.QuizCourse.ID,
                        Name = course.QuizCourse.Name,
                        Description = course.QuizCourse.Description,
                        Active = course.QuizCourse.Active,
                        DateCreated = course.QuizCourse.DateCreated,
                        TimeToDo = course.QuizCourse.TimeToDo,
                        ScorePass = course.QuizCourse.ScorePass,
                        Passed = course.QuizCourse.Passed,


                    },
                    CourseRoleDtos = new List<CourseRoleDto>(courseRoleDto)

                };


                return courseDto;
            }
            else
            {
                CourseDto courseDto = new()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    Active = course.Active,

                    DateCreated = course.DateCreated,
                    Alias = course.Alias,

                    LessionDtos = new List<LessionDto>(lessionDto),

                    CourseImageVideoDto = new List<CourseImageDto>(courseImageDto),
                    CourseRoleDtos = new List<CourseRoleDto>(courseRoleDto)
                };


                return courseDto;
            }

        }

        public async Task<IEnumerable<CourseDto>> GetCourseDtos()
        {
            var course = await _context.Courses.Include(x => x.QuizCourse).Include(x => x.CourseImageVideo).Include(x => x.CourseRoles).Include(x => x.Lessions).ThenInclude(x => x.LessionVideoImages).ToListAsync();

            var courseDto = _mapper.Map<List<CourseDto>>(course);

            return courseDto;
        }

        public async Task<PagedViewModel<CourseDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest, string accountName)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Course"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.Courses.Count() / (double)pageResult);

            var query = _context.Courses.Include(x => x.CourseImageVideo).Include(x => x.CourseRoles).AsQueryable();

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));

                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }

            var totalRow = await query.CountAsync();

            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new CourseDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        Description = x.Description,
                                        Active = x.Active,
                                        Code = x.Code,
                                        DateCreated = x.DateCreated,
                                        Alias = x.Alias,
                                        TotalWatched = x.TotalWatched,
                                        CourseImageVideoDto = new List<CourseImageDto>(_mapper.Map<List<CourseImageDto>>(x.CourseImageVideo)),
                                    })
                                    .OrderByDescending(x => x.DateCreated).ToListAsync();
            var roleResponse = new PagedViewModel<CourseDto>
            {
                Items = data,

                PageIndex = getListPagingRequest.PageIndex,

                PageSize = getListPagingRequest.PageSize,

                TotalRecord = (int)pageCount,
            };

            return roleResponse;
        }

        public async Task InsertCourseDto([FromForm] CreateCourseDto createCourseDto/*, IFormFile courseImage*/)
        {
            string extension = Path.GetExtension(createCourseDto.Image.FileName);

            string image = Utilities.SEOUrl(createCourseDto.Name) + extension;

            var code = _configuration.GetValue<string>("Code:Course");

            var key = code + Utilities.GenerateStringDateTime();
            createCourseDto.Code = key;

            createCourseDto.Id = Guid.NewGuid();

            createCourseDto.DateCreated = DateTime.Now;

            createCourseDto.Alias = Utilities.SEOUrl(createCourseDto.Name);

            Course account = _mapper.Map<Course>(createCourseDto);

            if (createCourseDto.Image != null)
            {
                account.CourseImageVideo = new List<CourseImageVideo>()
                {
                    new CourseImageVideo()
                    {
                        Id = Guid.NewGuid(),

                        CourseId = createCourseDto.Id,

                        Caption = createCourseDto.Name,

                        DateCreated = DateTime.Now,

                        FileSize = createCourseDto.Image.Length,

                        ImagePath = await Utilities.UploadFile(createCourseDto.Image, "imageCourse", image),

                    }
                };
            }
            _context.Courses.Add(account);

            await _context.SaveChangesAsync();


        }

        public async Task UpdateCourseAdminView(Guid id, [FromForm] UpdateCourseAdminView updateCourseAdminView)
        {
            using var transaction = _context.Database.BeginTransaction();

            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id.Equals(id));


            updateCourseAdminView.UpdateCourseDto.Alias = Utilities.SEOUrl(updateCourseAdminView.UpdateCourseDto.Name);

            updateCourseAdminView.UpdateCourseDto.DateCreated = DateTime.Now;

            Course courseUpdate = _mapper.Map(updateCourseAdminView.UpdateCourseDto, course);

            if (updateCourseAdminView.UpdateCourseImageDto.ImageFile != null)
            {
                string extension = Path.GetExtension(updateCourseAdminView.UpdateCourseImageDto.ImageFile.FileName);

                string image = Utilities.SEOUrl(updateCourseAdminView.UpdateCourseDto.Name) + extension;

                var thumbnailImage = await _context.CourseImageVideos.FirstOrDefaultAsync(i => i.CourseId.Equals(courseUpdate.Id));

                if (thumbnailImage != null)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "imageCourse", image);

                    var fileExsit = File.Exists(imagePath);

                    if (fileExsit == true)
                    {
                        File.Delete(imagePath);

                    }
                    thumbnailImage.FileSize = updateCourseAdminView.UpdateCourseImageDto.ImageFile.Length;

                    thumbnailImage.ImagePath = await Utilities.UploadFile(updateCourseAdminView.UpdateCourseImageDto.ImageFile, "imageCourse", image);

                    thumbnailImage.DateCreated = DateTime.Now;

                    thumbnailImage.Caption = updateCourseAdminView.UpdateCourseDto.Name;



                    _context.CourseImageVideos.Update(thumbnailImage);
                }
            }



            _context.Courses.Update(courseUpdate);
            await _context.SaveChangesAsync();


            var notificationResponeDb = _context.NotificationResponses.Where(x => x.TargetNotificationId.Equals(id));

            foreach (var item in notificationResponeDb)
            {
                if (item.Notify == false)
                {
                    UpdateNotificationResponseDto updateNotificationResponseDto = new();

                    updateNotificationResponseDto.Notify = true;

                    updateNotificationResponseDto.DateCreated = DateTime.Now;

                    NotificationResponse notificationResponseDto = _mapper.Map(updateNotificationResponseDto, item);

                    _context.NotificationResponses.Update(notificationResponseDto);

                    await _context.SaveChangesAsync();
                }
            }
            transaction.Commit();
        }

        public async Task<CourseDto> GetName(Guid id)
        {
            var name = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            var lessionDto = _mapper.Map<CourseDto>(name);

            return lessionDto;
        }

        public async Task<CourseDto> GetCode(string code)
        {
            var account = await _context.Courses.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<CourseDto>(account);
        }
    }
}
