using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Contract.Dtos.Assets.Status;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetsDto>> GetAsset();
        Task<IEnumerable<AssetsDto>> GetAssetSelected(string[] id);
        Task<AssetsDto> GetAssetByIdForMoving(string id);

        Task<AssetsDto> GetAssetById(string id);
        Task InsertAsset(CreateAssetsDto createAssetsDto);
        Task<AssetsDto> GetName(string name);
        Task DeleteAsset(string id);
        Task UpdateAsset(UpdateAssetsDto updateAssetsDto, string id);

        Task<PagedViewModel<AssetsDto>> GetPaging(GetListPagingRequest getListPagingRequest);
        Task<IEnumerable<AssetsDto>> Export([FromQuery] GetListPagingRequest getListPagingRequest);

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
            using var transaction = _context.Database.BeginTransaction();
            //var assetMoved = _context.AssetsMoveds.AsNoTracking().Where(x => x.AssestsId.Equals(id));
            //_context.AssetsMoveds.RemoveRange(assetMoved);
            //await _context.SaveChangesAsync();

            //var assetRepaired = _context.AssetsRepaireds.AsNoTracking().Where(x => x.AssestsId.Equals(id));
            //_context.AssetsRepaireds.RemoveRange(assetRepaired);
            //await _context.SaveChangesAsync();

            var asset = await _context.Assests.FindAsync(id);
            _context.Assests.Remove(asset);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

        }

        public Task<IEnumerable<AssetsDto>> Export([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            var query = _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Include(x => x.AssetsDepartment).OrderBy(x => x.Id).AsNoTracking().AsQueryable();


            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.AssetName.Contains(getListPagingRequest.Keyword) || x.Id.Contains(getListPagingRequest.Keyword)
                                    || x.AssetId.Contains(getListPagingRequest.Keyword) || x.Customer.Contains(getListPagingRequest.Keyword) || x.AssetSubCategory.Contains(getListPagingRequest.Keyword));
            }
            if (getListPagingRequest.Active == false)
            {
                query = query.Where(x => x.Active == false);
            }
            if (getListPagingRequest.AssetsCategoryId != null && getListPagingRequest.AssetsCategoryId.Length > 0)
            {
                query = query.Where(x => getListPagingRequest.AssetsCategoryId.Contains(x.AssetsCategoryId));
            }
            if (getListPagingRequest.AssetsDepartmentId != null && getListPagingRequest.AssetsDepartmentId.Length > 0)
            {
                query = query.Where(x => getListPagingRequest.AssetsDepartmentId.Contains(x.AssetsDepartmentId));
            }

            if (getListPagingRequest.AssetsStatusId != null && getListPagingRequest.AssetsStatusId.Length > 0)
            {
                query = query.Where(x => getListPagingRequest.AssetsStatusId.Contains(x.AssetsStatusId));
            }
            var data = _mapper.Map<IEnumerable<AssetsDto>>(query);

            return Task.FromResult(data);
        }

        public async Task<IEnumerable<AssetsDto>> GetAsset()
        {
            var asset = await _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Include(x => x.AssetsDepartment).OrderBy(x => x.Id).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsDto> GetAssetById(string id)
        {
            var asset = await _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Include(x => x.AssetsDepartment).Include(x => x.AssetsSupplier).Include(x => x.AssetsRepaireds).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            var assetMoved = await _context.AssetsMoveds.Where(x => x.OldAssestsId.Equals(id) || x.AssestsId.Equals(id)).ToListAsync();

            asset.AssetsMoveds = assetMoved;

            return _mapper.Map<AssetsDto>(asset);
        }

        public async Task<AssetsDto> GetAssetByIdForMoving(string id)
        {
            var asset = await _context.Assests.Include(x => x.AssetsStatus).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            AssetsDto assetsDto = new()
            {

                AssetId = asset.AssetId,
                AssetName = asset.AssetName,
                Quantity = asset.Quantity,
                Note = asset.Note,
                Active = asset.Active,
                AssetsStatusDto = new AssetsStatusDto
                {
                    Name = asset.AssetsStatus.Name,
                },
            };
            return assetsDto;
        }

        public Task<IEnumerable<AssetsDto>> GetAssetSelected(string[] id)
        {
            var asset = _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Select(x => new Asset
            {
                Id = x.Id,
                AssetName = x.AssetName,
                Active = x.Active,
                Customer = x.Customer,
                Quantity = x.Quantity,
                AssetsDepartment = new AssetDepartment
                {
                    Name = x.AssetsDepartment.Name,
                },
                AssetsCategory = new AssetCategory
                {
                    Name = x.AssetsCategory.Name,
                },
                AssetSubCategory = x.AssetSubCategory,
                AssetsStatus = new AssetStatus { Name = x.AssetsStatus.Name },
                Note = x.Note,
            }).Where(x => id.Contains(x.Id)).OrderBy(x => x.Id).AsNoTracking().AsQueryable();

            var assetDto = _mapper.Map<List<AssetsDto>>(asset);

            return Task.FromResult<IEnumerable<AssetsDto>>(assetDto);
        }

        public async Task<AssetsDto> GetCode(string code)
        {
            var asset = await _context.Assests.Include(x => x.AssetsStatus).Include(x => x.AssetsCategory).Include(x => x.AssetsDepartment).AsNoTracking().FirstOrDefaultAsync(x => x.AssetId == code);

            return _mapper.Map<AssetsDto>(asset);
        }

        public Task<AssetsDto> GetName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<AssetsDto>> GetPaging(GetListPagingRequest getListPagingRequest)
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
                                    || x.AssetId.Contains(getListPagingRequest.Keyword) || x.Customer.Contains(getListPagingRequest.Keyword) || x.AssetSubCategory.Contains(getListPagingRequest.Keyword)
                                    || x.BusinessModel.Contains(getListPagingRequest.Keyword) || x.Region.Contains(getListPagingRequest.Keyword)).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.Active == false)
            {
                query = query.Where(x => x.Active == false).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.AssetsCategoryId != null && getListPagingRequest.AssetsCategoryId.Length > 0)
            {
                query = query.Where(x => getListPagingRequest.AssetsCategoryId.Contains(x.AssetsCategoryId)).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.AssetsDepartmentLocationId != null && getListPagingRequest.AssetsDepartmentLocationId.Length > 0)
            {
                query = query.Where(x => getListPagingRequest.AssetsDepartmentLocationId.Contains(x.AssetsDepartment.ParentCode)
                                   ).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.AssetsDepartmentId != null && getListPagingRequest.AssetsDepartmentId.Length > 0)
            {
                query = query.Where(x => getListPagingRequest.AssetsDepartmentId.Contains(x.AssetsDepartmentId)
                                   ).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            if (getListPagingRequest.AssetsStatusId != null && getListPagingRequest.AssetsStatusId.Length > 0)
            {
                query = query.Where(x => getListPagingRequest.AssetsStatusId.Contains(x.AssetsStatusId)).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new AssetsDto()
                                    {
                                        Id = x.Id,
                                        //AssetId = x.AssetId,
                                        AssetName = x.AssetName,
                                        Active = x.Active,
                                        //AssetsCategoryId = x.AssetsCategoryId,
                                        AssetSubCategory = x.AssetSubCategory,
                                        //AssetsDepartmentId = x.AssetsDepartmentId,
                                        Quantity = x.Quantity,
                                        Customer = x.Customer,
                                        //Manager = x.Manager,
                                        //AssetsStatusId = x.AssetsStatusId,
                                        //DateUsed = x.DateUsed,
                                        //DateChecked = x.DateChecked,
                                        //Spec = x.Spec,
                                        //Note = x.Spec,
                                        AssetsCategoryDto = new AssetsCategoryDto { Name = x.AssetsCategory.Name },
                                        LocationDepartment = x.AssetsDepartment.Name,

                                        AssetsDepartmentDto = new AssetsDepartmentDto { Name = x.AssetsDepartment.Name },
                                        AssetsStatusDto = new AssetsStatusDto { Name = x.AssetsStatus.Name, },
                                        BusinessModel = x.BusinessModel,
                                        Region = x.Region,
                                    })
                                    .OrderBy(x => x.Id).AsNoTracking().ToListAsync();

            //foreach(var item in data)
            //{
            //    var nameLo = await _departmentService.GetCode(item.LocationDepartment);
            //    if(nameLo == null) { continue; };
            //    item.LocationDepartment = nameLo.Name;
            //}
            var assetResponse = new PagedViewModel<AssetsDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
                TotalItems = query.Count(),
                TotalQuantity = query.Sum(x => x.Quantity),
                CheckBox = new CheckBox()
                {
                    AssetsCategoryDtos = _mapper.Map<List<AssetsCategoryDto>>(await _context.AssetsCategories.AsNoTracking().ToListAsync()).Select(x => new AssetsCategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).OrderBy(x => x.Name).ToList(),
                    AssetsDepartmentLocationDtos = _mapper.Map<List<AssetsDepartmentDto>>(await _context.AssetsDepartments.Where(x => x.Level == "1" && x.Code == x.ParentCode).AsNoTracking().ToListAsync()).Select(x => new AssetsDepartmentDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Level = x.Level,
                        ParentCode = x.ParentCode,
                        Code = x.Code,
                    }).OrderBy(x => x.Name).ToList(),
                    AssetsDepartmentDtos = _mapper.Map<List<AssetsDepartmentDto>>(await _context.AssetsDepartments.Where(x => x.Level == "2").AsNoTracking().ToListAsync()).Select(x => new AssetsDepartmentDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Level = x.Level,
                        ParentCode = x.ParentCode,
                        Code = x.Code,
                    }).OrderBy(x => x.Name).ToList(),
                    AssetsDepartmentDtosMoving = _mapper.Map<List<AssetsDepartmentDto>>(await _context.AssetsDepartments.Where(x => x.Level == "2").AsNoTracking().ToListAsync()).Select(x => new AssetsDepartmentDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                    }).OrderBy(x => x.Name).ToList(),
                    AssetsStatusDtos = _mapper.Map<List<AssetsStatusDto>>(await _context.AssetsStatuses.AsNoTracking().ToListAsync()).Select(x => new AssetsStatusDto
                    {
                        Id = x.Id,

                        Name = x.Name,
                    }).OrderBy(x => x.Name).ToList(),

                },

            };
            if (getListPagingRequest.AssetsDepartmentLocationId != null && getListPagingRequest.AssetsDepartmentLocationId.Length > 0)
            {
                assetResponse.CheckBox.AssetsDepartmentDtos = assetResponse.CheckBox.AssetsDepartmentDtos.Where(x => getListPagingRequest.AssetsDepartmentLocationId.Contains(x.ParentCode) && x.Level == "2").ToList();
            }
            return assetResponse;
        }

        public async Task InsertAsset(CreateAssetsDto createAssetsDto)
        {
            string s = _configuration.GetValue<string>("Code:Assets");

            if (createAssetsDto.DateBuyed != null)
            {
                createAssetsDto.DateExpired = createAssetsDto.DateBuyed.Value.AddMonths(createAssetsDto.ExpireDay);

            }
            //createAssetsDto.ExpireDayRemaining = Math.Abs((DateTime.Now - createAssetsDto.DateExpired).Days);
            createAssetsDto.DateCreated = DateTime.Now;

            if (createAssetsDto.Id == null || createAssetsDto.Id == "")
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

            Domain.Entites.Asset asset = _mapper.Map<Domain.Entites.Asset>(createAssetsDto);
            _context.Add(asset);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task UpdateAsset(UpdateAssetsDto updateAssetsDto, string id)
        {
            var item = await _context.Assests.Include(x => x.AssetsMoveds).FirstOrDefaultAsync(x => x.Id == id);
            updateAssetsDto.DateCreated = DateTime.Now;
            if (updateAssetsDto.DateBuyed != null)
            {
                updateAssetsDto.DateExpired = updateAssetsDto.DateBuyed.Value.AddMonths(updateAssetsDto.ExpireDay);
            }
            if (item != null)
            {
                //var quantity = updateAssetsDto.Quantity - item.AssetsMoveds.Count();
                //updateAssetsDto.Quantity = quantity;
                _context.Assests.Update(_mapper.Map(updateAssetsDto, item));

                await _context.SaveChangesAsync();
            }
        }

    }
}
