using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.CourseRole;
using WebLearning.Contract.Dtos.ImageCourse;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface ICourseRoleService
    {
        Task<IEnumerable<CourseRoleDto>> GetCourseRole(string accountName);
        Task<CourseRoleDto> GetCourseRoleById(Guid courseId, string accountName);
        Task<CourseRoleDto> AdminGetCourse(Guid id, Guid roleId);
        Task InsertCourseRole(CreateCourseRoleDto createCourseRoleDto);
        Task DeleteCourseRole(Guid id, Guid roleId);
        Task<PagedViewModel<CourseRoleDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

    }
    public class CourseRoleService : ICourseRoleService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

        private readonly IConfiguration _configuration;

        public CourseRoleService(WebLearningContext context, IMapper mapper, IAccountService accountService, IRoleService roleService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _accountService = accountService;
            _roleService = roleService;
            _configuration = configuration;
        }
        public async Task DeleteCourseRole(Guid id, Guid roleId)
        {
            var CourseRole = await _context.CourseRoles.FindAsync(id, roleId);
            _context.CourseRoles.Remove(CourseRole);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedViewModel<CourseRoleDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:CourseRole"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.CourseRoles.Count() / (double)pageResult);

            var query = _context.CourseRoles.Include(x => x.Course).ThenInclude(x => x.CourseImageVideo).AsQueryable();

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.RoleName.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));

                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }

            var imageCourse = _context.CourseImageVideos.AsQueryable();


            var imageDto = _mapper.Map<List<CourseImageDto>>(imageCourse);

            var courseRoleDto = _mapper.Map<List<CourseRoleDto>>(query);


            var totalRow = await query.CountAsync();

            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new CourseRoleDto()
                                    {
                                        Id = x.Id,

                                        CourseId = x.CourseId,

                                        RoleId = x.RoleId,

                                        RoleName = x.RoleName,

                                        CourseDto = new CourseDto()
                                        {
                                            Id = x.Course.Id,

                                            Name = x.Course.Name,

                                            Description = x.Course.Description,

                                            Active = x.Course.Active,

                                            Code = x.Code,
                                            DateCreated = x.Course.DateCreated,

                                            Alias = x.Course.Alias,

                                            CourseImageVideoDto = new List<CourseImageDto>(imageDto),
                                        },
                                        Code = x.Code,

                                    }).OrderByDescending(x => x.CourseDto.DateCreated)
                                    .ToListAsync();
            var roleResponse = new PagedViewModel<CourseRoleDto>
            {
                Items = data,

                PageIndex = getListPagingRequest.PageIndex,

                PageSize = getListPagingRequest.PageSize,

                TotalRecord = (int)pageCount,
            };

            return roleResponse;

        }
        public async Task<IEnumerable<CourseRoleDto>> GetCourseRole(string accountName)
        {
            var roleId = await _accountService.GetUserByKeyWord(accountName);


            var CourseRole = _context.CourseRoles.Where(x => x.RoleId.Equals(roleId.AccountDto.RoleId)).AsNoTracking().AsQueryable();


            var CourseRoleDto = _mapper.Map<List<CourseRoleDto>>(CourseRole);

            return CourseRoleDto;

        }

        public async Task<CourseRoleDto> GetCourseRoleById(Guid courseId, string accountName)
        {
            var roleId = await _accountService.GetUserByKeyWord(accountName);

            var exist = await _context.CourseRoles.Include(x => x.Course).ThenInclude(x => x.Lessions).FirstOrDefaultAsync(x => x.CourseId.Equals(courseId));

            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id.Equals(courseId));


            UpdateTotalWatched updateCourseDto = new();

            UpdateCourseRoleDto updateCourseRoleDto = new();

            using var transaction = _context.Database.BeginTransaction();


            if (exist != null)
            {
                var count = exist.NumWatched;

                count++;

                updateCourseRoleDto.NumWatched = count;



                _context.Update(_mapper.Map(updateCourseRoleDto, exist));

                await _context.SaveChangesAsync();
            }




            var courseRole = await _context.CourseRoles.Include(x => x.Role).Include(x => x.Course).ThenInclude(x => x.QuizCourse).Include(x => x.Course.Lessions).FirstOrDefaultAsync(x => x.CourseId.Equals(courseId) && x.RoleId.Equals(roleId.AccountDto.RoleId));

            var courseRoleDto = _mapper.Map<CourseRoleDto>(courseRole);

            var totalWatched = course.TotalWatched;

            if (courseRoleDto == null)
            {

                totalWatched++;

                updateCourseDto.TotalWatched = totalWatched;

                _context.Update(_mapper.Map(updateCourseDto, course));

                await _context.SaveChangesAsync();

                transaction.Commit();

                return default;
            }
            else
            {
                updateCourseDto.TotalWatched = updateCourseRoleDto.NumWatched;

                _context.Update(_mapper.Map(updateCourseDto, course));

                await _context.SaveChangesAsync();

                transaction.Commit();

                return courseRoleDto;

            }

        }

        public async Task InsertCourseRole(CreateCourseRoleDto createCourseRoleDto)
        {
            var roleId = await _roleService.GetRoleById(createCourseRoleDto.RoleId);

            createCourseRoleDto.Id = Guid.NewGuid();

            var code = _configuration.GetValue<string>("Code:CourseRole");

            var key = code + Utilities.GenerateStringDateTime();
            createCourseRoleDto.Code = key;

            createCourseRoleDto.RoleName = roleId.RoleName;

            CourseRole courseRole = _mapper.Map<CourseRole>(createCourseRoleDto);

            if (_context.CourseRoles.Any(x => x.CourseId.Equals(createCourseRoleDto.CourseId) && x.RoleId.Equals(createCourseRoleDto.RoleId)) == false)
            {
                _context.Add(courseRole);
                await _context.SaveChangesAsync();
            }


        }
        public async Task<CourseRoleDto> AdminGetCourse(Guid id, Guid roleId)
        {
            var courseRole = await _context.CourseRoles.Include(x => x.Role).Include(x => x.Course).FirstOrDefaultAsync(x => x.CourseId.Equals(id) && x.RoleId.Equals(roleId));

            return _mapper.Map<CourseRoleDto>(courseRole);
        }

    }
}
