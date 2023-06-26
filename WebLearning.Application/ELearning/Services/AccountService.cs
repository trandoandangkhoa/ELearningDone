using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Avatar;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAccount();
        Task<AccountDto> GetUserById(Guid Id);

        Task<UserAllInformationDto> GetUserByKeyWord(string keyword);

        Task<AccountDto> GetNameUser(string accountname);

        Task<string> AvatarImagePath(string accountName);
        Task InsertUserInfo(CreateAccountDto createUserInfoDto);

        Task DeleteUser(Guid Id);

        Task UpdateUser(UpdateAccountDto updateUserDetailDto, Guid Id);

        Task AddImage(Guid accountId, CreateAvatarDto createAvatarDto);

        Task UpdateImage(Guid accountId, UpdateAvatarDto updateAvatarDto);

        Task<AvatarDto> GetImageById(Guid accountId);

        Task<AccountDto> GetCode(string code);

        Task<PagedViewModel<AccountDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

    }
    public class AccountService : IAccountService
    {
        private readonly WebLearningContext _context;

        private readonly IMapper _mapper;

        private readonly IRoleService _roleService;

        private readonly IConfiguration _configuration;


        public AccountService(WebLearningContext context, IMapper mapper, IRoleService roleService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _roleService = roleService;
            _configuration = configuration;

        }
        public async Task DeleteUser(Guid Id)
        {
            var user = await _context.Accounts.FindAsync(Id);
            _context.Accounts.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AccountDto>> GetAccount()
        {
            var account = await _context.Accounts.Include(x => x.AccountDetail).Include(x => x.Role).OrderByDescending(x => x.DateCreated).ToListAsync();
            var accountDto = _mapper.Map<List<AccountDto>>(account);
            return accountDto;

        }

        public async Task<string> AvatarImagePath(string accountName)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Email.Equals(accountName));

            var path = await _context.Avatars.FirstOrDefaultAsync(x => x.AccountId.Equals(account.Id));

            if (path == null) return default;
            var pathDto = _mapper.Map<AvatarDto>(path);

            return pathDto.ImagePath;

        }
        public async Task<PagedViewModel<AccountDto>> GetPaging(GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Account"));
            }
            var pageResult = getListPagingRequest.PageSize;


            var pageCount = Math.Ceiling(_context.Accounts.Count() / (double)pageResult);
            var query = _context.Accounts.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.AccountDetail.FullName.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new AccountDto()
                                    {
                                        Id = x.Id,
                                        Email = x.Email,
                                        Password = x.Password,
                                        PasswordHased = x.PasswordHased,
                                        Active = x.Active,
                                        RoleId = x.RoleId,
                                        Code = x.Code,
                                        DateCreated = x.DateCreated,
                                        LastLogin = x.LastLogin,
                                        accountDetailDto = new AccountDetailDto()
                                        {
                                            Id = x.AccountDetail.Id,
                                            AccoundId = x.Id,
                                            FullName = x.AccountDetail.FullName,
                                            Phone = x.AccountDetail.Phone,
                                            Address = x.AccountDetail.Address
                                        },
                                        roleDto = new RoleDto
                                        {
                                            Id = x.RoleId,
                                            RoleName = x.Role.RoleName,
                                            Description = x.Role.Description,
                                        }

                                    })
                                    .OrderByDescending(x => x.DateCreated).ToListAsync();
            var roleResponse = new PagedViewModel<AccountDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return roleResponse;

        }
        public async Task<AccountDto> GetUserById(Guid Id)
        {
            var account = await _context.Accounts.Include(x => x.AccountDetail).Include(x => x.Role).FirstOrDefaultAsync(x => x.Id.Equals(Id));

            return _mapper.Map<AccountDto>(account);
        }
        public async Task<AccountDto> GetNameUser(string accountName)
        {
            return _mapper.Map<AccountDto>(await _context.Accounts.Include(x => x.AccountDetail).Include(x => x.Role).AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(accountName)));
        }
        public async Task<UserAllInformationDto> GetUserByKeyWord(string keyword)
        {
            var account = await _context.Accounts.Include(x => x.Role).Include(x => x.AccountDetail).Include(x => x.Avatar).AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(keyword));

            if (account == null)
            {
                return default;
            }

            var course =  _context.Courses.Include(x => x.QuizCourse).Include(x => x.CourseImageVideo).Include(x => x.CourseRoles).Where(x => x.Active == true).OrderByDescending(x => x.DateCreated).AsQueryable();

            UserAllInformationDto userAllInformationDto = new()
            {
                AccountDto = _mapper.Map<AccountDto>(account),
                CourseDtos = _mapper.Map<List<CourseDto>>(course),
                OwnCourseDtos = _mapper.Map<List<CourseDto>>(course.Where(x => x.CourseRoles.Any(x => x.RoleId.Equals(account.RoleId))).AsQueryable()),
                LessionDtos = _mapper.Map<List<LessionDto>>( _context.Lessions.Include(x => x.Courses).Include(x => x.LessionVideoImages).Include(x => x.Quizzes).Include(x => x.OtherFileUploads).Where(x => x.Active == true).OrderByDescending(x => x.DateCreated).AsQueryable()),
                QuizCourseDtos = _mapper.Map<List<QuizCourseDto>>(_context.QuizCourses.Include(x => x.Course).ThenInclude(x => x.CourseRoles).Include(x => x.QuestionFinals).Where(x => x.Active == true).AsQueryable()),
                QuizlessionDtos = _mapper.Map<List<QuizlessionDto>>(_context.QuizLessions.Include(x => x.Lession).Include(x => x.QuestionLessions).Where(x => x.Active == true).AsQueryable()),
                QuizMonthlyDtos = _mapper.Map<List<QuizMonthlyDto>>(_context.QuizMonthlies.Include(x => x.QuestionMonthlies).Where(x => x.RoleId.Equals(account.RoleId) && x.Active == true).AsQueryable()),
                ReportScoreCourseDtos = _mapper.Map<List<ReportScoreCourseDto>>(_context.ReportUserScoreFinals.OrderByDescending(x => x.CompletedDate).Where(x => x.UserName.Equals(account.Email)).AsNoTracking().AsQueryable()),
                ReportScoreLessionDtos = _mapper.Map<List<ReportScoreLessionDto>>(_context.ReportUsersScore.OrderByDescending(x => x.CompletedDate).Where(x => x.UserName.Equals(account.Email)).AsNoTracking().AsQueryable()),
                ReportScoreMonthlyDtos = _mapper.Map<List<ReportScoreMonthlyDto>>(_context.ReportUserScoreMonthlies.OrderByDescending(x => x.CompletedDate).Where(x => x.UserName.Equals(account.Email)).AsNoTracking().AsQueryable()),
                HistoryAddSlotDtos = _mapper.Map<List<HistoryAddSlotDto>>(_context.HistoryAddSlots.Include(x => x.Room).Where(x => x.Email.Equals(account.Email)).AsNoTracking().AsQueryable()),
            };



            return userAllInformationDto;
        }

        public async Task InsertUserInfo(CreateAccountDto createAccountDto)
        {

            Account account = _mapper.Map<Account>(createAccountDto);

            var roleName = await _roleService.GetRoleById(createAccountDto.RoleId);

            var code = _configuration.GetValue<string>("Code:Account");

            var key = code + Utilities.GenerateStringDateTime();
            createAccountDto.Code = key;
            account.Code = createAccountDto.Code;
            if (roleName != null)
            {
                if (roleName.RoleName == "Admin")
                {
                    createAccountDto.AuthorizeRole = AuthorizeRole.AdminRole;
                    account.AuthorizeRole = Domain.AuthorizeRoles.AdminRole;

                }
                else if (roleName.RoleName == "Manager")
                {

                    createAccountDto.AuthorizeRole = AuthorizeRole.ManagerRole;
                    account.AuthorizeRole = Domain.AuthorizeRoles.ManagerRole;

                }
                else if (roleName.RoleName == "Staff")
                {

                    createAccountDto.AuthorizeRole = AuthorizeRole.StaffRole;
                    account.AuthorizeRole = Domain.AuthorizeRoles.StaffRole;

                }
                else if (roleName.RoleName == "Guest")
                {

                    createAccountDto.AuthorizeRole = AuthorizeRole.Guest;
                    account.AuthorizeRole = Domain.AuthorizeRoles.Guest;

                }
                else if (roleName.RoleName == "Teacher")
                {

                    createAccountDto.AuthorizeRole = AuthorizeRole.TeacherRole;
                    account.AuthorizeRole = Domain.AuthorizeRoles.TeacherRole;

                }
                else if (roleName.RoleName == "IT")
                {

                    createAccountDto.AuthorizeRole = AuthorizeRole.ITRole;
                    account.AuthorizeRole = Domain.AuthorizeRoles.ITRole;

                }
                else
                {
                    createAccountDto.AuthorizeRole = AuthorizeRole.StudentRole;
                    account.AuthorizeRole = Domain.AuthorizeRoles.StudentRole; ;

                }
            }
            if (createAccountDto.Password != null)
            {
                account.PasswordHased = Password.HashedPassword(createAccountDto.Password);

            }
            account.PasswordHased = Password.HashedPassword(createAccountDto.Password);

            account.DateCreated = DateTime.Now;

            account.LastLogin = DateTime.Now;

            if (_context.Accounts.Any(x => x.Email.Equals(createAccountDto.Email)) == false)
            {
                _context.Add(account);

                await _context.SaveChangesAsync();
            }

        }


        public async Task UpdateUser(UpdateAccountDto updateUserDetailDto, Guid Id)
        {
            Account account = _mapper.Map(updateUserDetailDto, await _context.Accounts.Include(x => x.AccountDetail).Include(x => x.Role).FirstOrDefaultAsync(x => x.Id.Equals(Id)));

            if (account != null)
            {
                var roleName = await _roleService.GetRoleById(updateUserDetailDto.RoleId);

                if (roleName != null)
                {
                    if (roleName.RoleName == "Admin")
                    {
                        updateUserDetailDto.AuthorizeRole = AuthorizeRole.AdminRole;
                        account.AuthorizeRole = Domain.AuthorizeRoles.AdminRole;

                    }
                    else if (roleName.RoleName == "Manager")
                    {

                        updateUserDetailDto.AuthorizeRole = AuthorizeRole.ManagerRole;
                        account.AuthorizeRole = Domain.AuthorizeRoles.ManagerRole;

                    }
                    else if (roleName.RoleName == "Staff")
                    {

                        updateUserDetailDto.AuthorizeRole = AuthorizeRole.StaffRole;
                        account.AuthorizeRole = Domain.AuthorizeRoles.StaffRole;

                    }
                    else if (roleName.RoleName == "Teacher")
                    {

                        updateUserDetailDto.AuthorizeRole = AuthorizeRole.TeacherRole;
                        account.AuthorizeRole = Domain.AuthorizeRoles.TeacherRole;

                    }
                    else if (roleName.RoleName == "IT")
                    {

                        updateUserDetailDto.AuthorizeRole = AuthorizeRole.ITRole;
                        account.AuthorizeRole = Domain.AuthorizeRoles.ITRole;

                    }
                    else if (roleName.RoleName == "Guest")
                    {

                        updateUserDetailDto.AuthorizeRole = AuthorizeRole.Guest;
                        account.AuthorizeRole = Domain.AuthorizeRoles.Guest; ;

                    }
                    else
                    {
                        updateUserDetailDto.AuthorizeRole = AuthorizeRole.StudentRole;
                        account.AuthorizeRole = Domain.AuthorizeRoles.StudentRole; ;

                    }
                    if (updateUserDetailDto.Password != null)
                    {
                        account.PasswordHased = Password.HashedPassword(updateUserDetailDto.Password);

                    }
                    account.LastLogin = DateTime.Now;

                    account.DateCreated = DateTime.Now;

                    _context.Accounts.Update(account);
                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task UpdateImage(Guid accountId, UpdateAvatarDto updateAvatarDto)
        {
            string extension = Path.GetExtension(updateAvatarDto.Image.FileName);

            var avatar = await _context.Avatars.FirstOrDefaultAsync(x => x.AccountId.Equals(accountId));

            var name = await GetUserById(accountId);


            if (avatar == null)
                throw new Exception($"Cannot find an image with id {accountId}");

            if (updateAvatarDto.Image != null)
            {
                updateAvatarDto.Name = name.accountDetailDto.FullName;

                updateAvatarDto.FileSize = updateAvatarDto.Image.Length;

                updateAvatarDto.DateCreated = DateTime.Now;

                string image = Utilities.SEOUrl(updateAvatarDto.Name) + extension;


                //delete image from wwwroot/image
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "avatarImage", image);

                var fileExsit = File.Exists(imagePath);

                if (fileExsit == true)
                {
                    File.Delete(imagePath);

                }

                updateAvatarDto.ImagePath = await Utilities.UploadAvatar(updateAvatarDto.Image, "avatarImage", image);

                _context.Avatars.Update(_mapper.Map(updateAvatarDto, avatar));


            }

            await _context.SaveChangesAsync();
        }

        public async Task AddImage(Guid accountId, CreateAvatarDto createAvatarDto)
        {
            string extension = Path.GetExtension(createAvatarDto.Image.FileName);


            var name = await GetUserById(accountId);

            if (createAvatarDto.Image != null)
            {

                createAvatarDto.Id = Guid.NewGuid();

                createAvatarDto.Name = name.accountDetailDto.FullName;

                createAvatarDto.FileSize = createAvatarDto.Image.Length;

                createAvatarDto.DateCreated = DateTime.Now;

                string image = Utilities.SEOUrl(createAvatarDto.Name) + extension;

                createAvatarDto.ImagePath = await Utilities.UploadAvatar(createAvatarDto.Image, "avatarImage", image);

                Avatar avatarDto = _mapper.Map<Avatar>(createAvatarDto);

                _context.Avatars.Add(avatarDto);

            }

            await _context.SaveChangesAsync();
        }

        public Task<AvatarDto> GetImageById(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountDto> GetCode(string code)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<AccountDto>(account);
        }
    }
}
