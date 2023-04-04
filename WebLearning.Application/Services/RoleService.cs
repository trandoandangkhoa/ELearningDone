using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetRole();
        Task<RoleDto> GetRoleById(Guid Id);
        Task InsertRole(CreateRoleDto createRoleDto);
        Task<RoleDto> GetName(string name);
        Task DeleteRole(Guid Id);
        Task UpdateRole(UpdateRoleDto updateRoleDto, Guid Id);
        Task<PagedViewModel<RoleDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<RoleDto> GetCode(string code);

    }
    public class RoleService : IRoleService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public RoleService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteRole(Guid Id)
        {
            using var transaction = _context.Database.BeginTransaction();

            var role = await _context.Roles.FindAsync(Id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            var reportScore = _context.ReportUserScoreMonthlies.Where(x => x.RoleId.Equals(Id));

            _context.ReportUserScoreMonthlies.RemoveRange(reportScore);
            await _context.SaveChangesAsync();

            transaction.Commit();
        }

        public async Task<RoleDto> GetCode(string code)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Code.Equals(code));

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> GetName(string name)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName.Equals(name));

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<PagedViewModel<RoleDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:Role");
            var pageCount = Math.Ceiling(_context.Roles.Count() / (double)pageResult);
            var query = _context.Roles.AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.RoleName.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new RoleDto()
                                    {
                                        Id = x.Id,
                                        RoleName = x.RoleName,
                                        Description = x.Description,
                                        Alias = x.Alias,
                                        Code = x.Code,
                                    })
                                    .OrderByDescending(x => x.RoleName).ToListAsync();
            var roleResponse = new PagedViewModel<RoleDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return roleResponse;

        }
        public async Task<IEnumerable<RoleDto>> GetRole()
        {
            var role = await _context.Roles.OrderByDescending(x => x.RoleName).AsNoTracking().ToListAsync();
            var roleDto = _mapper.Map<List<RoleDto>>(role);
            return roleDto;

        }

        public async Task<RoleDto> GetRoleById(Guid Id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id.Equals(Id));

            return _mapper.Map<RoleDto>(role);
        }
        public async Task InsertRole(CreateRoleDto createRoleDto)
        {
            var code = _configuration.GetValue<string>("Code:Role");

            var key = code + Utilities.GenerateStringDateTime();
            createRoleDto.Code = key;
            createRoleDto.Alias = Utilities.SEOUrl(createRoleDto.RoleName);
            Role role = _mapper.Map<Role>(createRoleDto);
            role.Alias = createRoleDto.Alias;
            role.Code = createRoleDto.Code;
            if (_context.Roles.Any(x => x.RoleName.Equals(createRoleDto.RoleName)) == false)
            {

                _context.Add(role);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task UpdateRole(UpdateRoleDto updateRoleDto, Guid Id)
        {
            var item = await _context.Roles.FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (item != null)
            {

                updateRoleDto.Alias = Utilities.SEOUrl(updateRoleDto.RoleName);
                _context.Roles.Update(_mapper.Map(updateRoleDto, item));

                item.Alias = updateRoleDto.Alias;

                await _context.SaveChangesAsync();
            }
        }
    }
}
