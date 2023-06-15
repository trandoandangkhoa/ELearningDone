using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Contract.Dtos.Assets.Status;
using WebLearning.Contract.Dtos.BookingCalender.Room;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetsDto>> GetAsset();
        Task<AssetsDto> GetAssetById(string id);
        Task InsertAsset(CreateAssetsDto createAssetsDto);
        Task<AssetsDto> GetName(string name);
        Task DeleteAsset(string id);
        Task UpdateAsset(UpdateAssetsDto updateAssetsDto, string id);
        Task<PagedViewModel<AssetsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<AssetsDto> GetCode(string code);
    }
    public class AssetService : IAssetService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ICategoryService _categoryService;
        private readonly IDepartmentService _departmentService;
        private readonly IStatusService _statusService;

        public AssetService(WebLearningContext context, IMapper mapper, IConfiguration configuration, ICategoryService categoryService, IDepartmentService departmentService, IStatusService statusService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _categoryService = categoryService;
            _departmentService = departmentService;
            _statusService = statusService;
        }
        public async Task DeleteAsset(string id)
        {

            var asset = await _context.Assests.FindAsync(id);
            _context.Assests.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsDto>> GetAsset()
        {
            var asset = await _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Include(x => x.AssetsDepartment).OrderBy(x => x.Id).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsDto> GetAssetById(string id)
        {
            var asset = await _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Include(x => x.AssetsDepartment).FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<AssetsDto>(asset);
        }

        public async Task<AssetsDto> GetCode(string code)
        {
            var asset = await _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Include(x => x.AssetsDepartment).FirstOrDefaultAsync(x => x.AssetId == code);

            return _mapper.Map<AssetsDto>(asset);
        }

        public Task<AssetsDto> GetName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<AssetsDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Assets"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var query = _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Include(x => x.AssetsDepartment).OrderBy(x => x.Id).AsNoTracking().AsQueryable();

            var pageCount = Math.Ceiling(query.Count() / (double)pageResult);

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.AssetName.Contains(getListPagingRequest.Keyword) || x.Id.Contains(getListPagingRequest.Keyword)
                                    || x.AssetId.Contains(getListPagingRequest.Keyword) || x.Customer.Contains(getListPagingRequest.Keyword) || x.AssetSubCategory.Contains(getListPagingRequest.Keyword)).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.Active == false)
            {
                query = query.Where(x => x.Active == false).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.AssetsCategoryId != null && getListPagingRequest.AssetsCategoryId.Length > 0) {
                query = query.Where(x => getListPagingRequest.AssetsCategoryId.Contains(x.AssetsCategoryId)).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.AssetsDepartmentId != null && getListPagingRequest.AssetsDepartmentId.Length > 0) {
                query = query.Where(x => getListPagingRequest.AssetsDepartmentId.Contains(x.AssetsDepartmentId)).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.AssetsStatusId != null && getListPagingRequest.AssetsStatusId.Length > 0) {
                query = query.Where(x => getListPagingRequest.AssetsStatusId.Contains(x.AssetsStatusId)).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new AssetsDto()
                                    {
                                        Id = x.Id,
                                        AssetId = x.AssetId,
                                        AssetName = x.AssetName,
                                        Active = x.Active,
                                        AssetsCategoryId = x.AssetsCategoryId,
                                        AssetSubCategory = x.AssetSubCategory,
                                        AssetsDepartmentId = x.AssetsDepartmentId,
                                        Quantity = x.Quantity,
                                        Customer = x.Customer,
                                        Manager = x.Manager,
                                        AssetsStatusId = x.AssetsStatusId,
                                        DateUsed = x.DateUsed,
                                        DateChecked = x.DateChecked,
                                        Spec = x.Spec,
                                        Note = x.Spec,
                                        AssetsCategoryDto = _mapper.Map<AssetsCategoryDto>(x.AssetsCategory),
                                        AssetsDepartmentDto = _mapper.Map<AssetsDepartmentDto>(x.AssetsDepartment),
                                        AssetsStatusDto = _mapper.Map<AssetsStatusDto>(x.AssetsStatus)
                                    })
                                    .OrderBy(x => x.Id).AsNoTracking().ToListAsync();


            var assetResponse = new PagedViewModel<AssetsDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
                TotalItems = query.Count(),
                
                CheckBox = new CheckBox()
                {
                    AssetsCategoryDtos = _mapper.Map<List<AssetsCategoryDto>>(await _context.AssetsCategories.AsNoTracking().ToListAsync()).Select(x => new AssetsCategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList(),
                    AssetsDepartmentDtos = _mapper.Map<List<AssetsDepartmentDto>>(await _context.AssetsDepartments.AsNoTracking().ToListAsync()).Select(x => new AssetsDepartmentDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList(),
                    AssetsStatusDtos = _mapper.Map<List<AssetsStatusDto>>(await _context.AssetsStatuses.AsNoTracking().ToListAsync()).Select(x => new AssetsStatusDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList(),

                },
                //HistoryChecked = new CheckBox()
                //{
                //    AssetsCategoryDtos = _mapper.Map<List<AssetsCategoryDto>>(catFilterHistory),

                //    AssetsSubCategoryDtos = _mapper.Map<List<AssetsSubCategoryDto>>(subFiterHistory),

                //    AssetsDepartmentDtos = _mapper.Map<List<AssetsDepartmentDto>>(depFilterHistory),

                //    AssetsStatusDtos = _mapper.Map<List<AssetsStatusDto>>(statusFilterHistory),
                //}
            };
            return assetResponse;
        }

        public async Task InsertAsset(CreateAssetsDto createAssetsDto)
        {
            string s = _configuration.GetValue<string>("Code:Assets");

            if(createAssetsDto.DateBuyed != null)
            {
                createAssetsDto.DateExpired = createAssetsDto.DateBuyed.Value.AddMonths(createAssetsDto.ExpireDay);

            }
            //createAssetsDto.ExpireDayRemaining = Math.Abs((DateTime.Now - createAssetsDto.DateExpired).Days);

            if (createAssetsDto.Id == null)
            {
                var list = await _context.Assests.AsNoTracking().OrderByDescending(x => x.Id).ToListAsync();

                if (list.Count == 0) { createAssetsDto.Id = s + "00000001"; }

                else if (list.Count > 0)
                {
                    var x = list[0].Id.Substring(2, 8);

                    int k = Convert.ToInt32(list[0].Id.Substring(2, 8)) + 1;
                    if (k < 10) s += "0000000";
                    else if (k < 100)
                        s += "000000";
                    else if (k < 1000)
                        s += "00000";
                    else if (k < 10000)
                        s += "0000";
                    else if (k < 100000)
                        s += "000";
                    else if (k < 1000000)
                        s += "00";
                    else if (k < 10000000)
                        s += "0";
                    s += k.ToString();
                    createAssetsDto.Id = s;
                }
            }

            Assests asset = _mapper.Map<Assests>(createAssetsDto);
            _context.Add(asset);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task UpdateAsset(UpdateAssetsDto updateAssetsDto, string id)
        {
            var item = await _context.Assests.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {

                _context.Assests.Update(_mapper.Map(updateAssetsDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
