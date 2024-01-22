using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Contract.Dtos.Assets.Moved;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{

    public interface IAssetMovedService
    {
        Task<IEnumerable<AssetsMovedDto>> GetAssetsMoved();
        Task<AssetsMovedDto> GetAssetsMovedById(Guid id);

        Task<AssetMovedPrintView> GetAssetsMovedByCode(string code, string accountName);

        Task<string> InsertAssetsMoved(CreateAssetsMovedDto createAssetsMovedDto);

        Task<IEnumerable<AssetMovedHistoryDto>> GetAssetsMovedHistory();
        Task<string> GetPrintCode();

        Task<AssetsMovedDto> GetName(string name);
        Task DeleteAssetsMoved(Guid id);
        Task UpdateAssetsMoved(UpdateAssetsMovedDto updateAssetsMovedDto, Guid id);
        Task<PagedViewModel<AssetsMovedDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

    }

    public class AssetMovedService : IAssetMovedService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAssetService _assetService;
        private readonly IDepartmentService _departmentService;
        private readonly IAccountService _accountService;
        public AssetMovedService(WebLearningContext context, IMapper mapper, IConfiguration configuration, IAssetService assetService, IDepartmentService departmentService, IAccountService accountService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _assetService = assetService;
            _departmentService = departmentService;
            _accountService = accountService;
        }
        public async Task DeleteAssetsMoved(Guid id)
        {

            var asset = await _context.AssetsMoveds.FindAsync(id);

            _context.AssetsMoveds.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsMovedDto>> GetAssetsMoved()
        {
            var asset = await _context.AssetsMoveds.Include(x => x.AssetsMovedStatus).OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsMovedDto>>(asset);
            return assetDto;
        }

        public async Task<AssetMovedPrintView> GetAssetsMovedByCode(string code,string accountName)
        {
            var asset = await _context.AssetsMoveds.Where(x => x.Code == code).OrderByDescending(x => x.DateMoved).ToListAsync();
            List<AssetMovedTicket> assetMovedTickets = new();
            List<string> oldDep = new();
            foreach (var  item in asset)
            {
                assetMovedTickets.Add(new AssetMovedTicket
                {
                    Id = item.NumBravo,
                    Name = ( await _assetService.GetAssetByIdForMoving(item.OldAssestsId)).AssetName,
                    Note = item.AssetNote,
                    Quantity = item.Quantity,
                    Status = item.AssetStatus,
                    Unit = item.Unit,
                });
                 var a = (await _departmentService.GetAssetsDepartmentById(item.OldDepartmentId)).Name + ", ";
                oldDep.Add(a);
            };
            

            AssetMovedPrintView assetMovedPrintView = new()
            {
                ReasonMove = asset[0].Description,
                NewDep = (await _departmentService.GetAssetsDepartmentById(asset[0].NewDepartmentId)).Name,
                Receiver = asset[0].Receiver,
                Sender = (await _accountService.GetNameUser(accountName)).accountDetailDto.FullName,
                RoleSenderName = (await _accountService.GetNameUser(accountName)).roleDto.RoleName,
                AssetMovedTickets = assetMovedTickets,
                OldDep = oldDep,
                Id = asset[0].Code,
            };

            return assetMovedPrintView;
        }

        public async Task<AssetsMovedDto> GetAssetsMovedById(Guid id)
        {
            var asset = await _context.AssetsMoveds.Include(x => x.Assests).FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<AssetsMovedDto>(asset);
        }

        public async Task<IEnumerable<AssetMovedHistoryDto>> GetAssetsMovedHistory()
        {
            var asset = await _context.AssetMovedHistories.OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetMovedHistoryDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsMovedDto> GetName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<AssetsMovedDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Assets"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var query = _context.AssetsMoveds.Include(x => x.AssetsMovedStatus).OrderBy(x => x.Id).AsNoTracking().AsQueryable();

            var pageCount = Math.Ceiling(query.Count() / (double)pageResult);

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.SenderPhoneNumber.Contains(getListPagingRequest.Keyword) || x.Receiver.Contains(getListPagingRequest.Keyword)
                                    || x.ReceiverPhoneNumber.Contains(getListPagingRequest.Keyword) || x.AssestsId.Contains(getListPagingRequest.Keyword)).AsNoTracking();
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }

            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new AssetsMovedDto()
                                    {
                                        Id = x.Id,
                                        AssestsId = x.AssestsId,
                                        Receiver = x.Receiver,
                                        SenderPhoneNumber = x.SenderPhoneNumber,
                                        ReceiverPhoneNumber = x.ReceiverPhoneNumber,
                                        Description = x.Description,
                                        DateMoved = x.DateMoved,
                                        NewDepartmentId = x.NewDepartmentId,
                                        OldDepartmentId = x.OldDepartmentId,
                                        AssetsMovedStatusId = x.AssetsMovedStatusId,
                                        DateUsed = x.DateUsed,
                                        AssetsMovedStatusDto = _mapper.Map<AssetsMovedStatusDto>(x.AssetsMovedStatus),
                                    })
                                    .OrderBy(x => x.Id).AsNoTracking().ToListAsync();


            var assetMoveResponse = new PagedViewModel<AssetsMovedDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
                TotalItems = query.Count(),


            };
            return assetMoveResponse;
        }

        public async Task<string> GetPrintCode()
        {
            string s = _configuration.GetValue<string>("Code:PrintCode");

            CreateAssetMovedHistoryDto createAssetMovedHistoryDto = new();

            var list = await _context.AssetMovedHistories.AsNoTracking().OrderByDescending(x => x.Id).ToListAsync();

            if (list.Count == 0) { createAssetMovedHistoryDto.Id = s + "00000001"; }
            
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
                createAssetMovedHistoryDto.Id = s;
            }
            createAssetMovedHistoryDto.DateCreated = DateTime.Now;

            AssetMovedHistory asset = _mapper.Map<AssetMovedHistory>(createAssetMovedHistoryDto);
            _context.Add(asset);
            await _context.SaveChangesAsync();
            
            return createAssetMovedHistoryDto.Id;
        }

        public async Task<string> InsertAssetsMoved(CreateAssetsMovedDto createAssetsMovedDto)
        {
            string result = "";
            using var transaction = _context.Database.BeginTransaction();

            if (createAssetsMovedDto.OldAssestsId != null)
            {
                var asset = await _context.Assests.Include(x => x.AssetsStatus).AsNoTracking().FirstOrDefaultAsync(x => x.Id == createAssetsMovedDto.OldAssestsId);



                if (asset == null)
                {
                    result = "Không tìm thấy tài sản";
                    return result;

                };
                createAssetsMovedDto.Id = Guid.NewGuid();

                createAssetsMovedDto.AssetsMovedStatusId = 1;

                createAssetsMovedDto.DateMoved = DateTime.Now;

                createAssetsMovedDto.OldDepartmentId = asset.AssetsDepartmentId;

                createAssetsMovedDto.AssetStatus = asset.AssetsStatus.Name;

                createAssetsMovedDto.AssetNote = asset.Note;

                createAssetsMovedDto.NumBravo = asset.AssetId;

                UpdateAssetsDto updateAssetsDto = _mapper.Map<UpdateAssetsDto>(asset);

                updateAssetsDto.DateMoved = createAssetsMovedDto.DateMoved;

                updateAssetsDto.Quantity = asset.Quantity - createAssetsMovedDto.Quantity;

                if (createAssetsMovedDto.NewDepartmentId.Equals(asset.AssetsDepartmentId) == false)
                {
                    CreateAssetsDto createAssetsDto = _mapper.Map<CreateAssetsDto>(asset);

                    var ItemExistInNewDep = await _context.Assests.AsNoTracking().FirstOrDefaultAsync(x => x.AssetId == createAssetsMovedDto.NumBravo && x.AssetsDepartmentId.Equals(createAssetsMovedDto.NewDepartmentId) && x.Customer == createAssetsMovedDto.Receiver);

                    if (ItemExistInNewDep == null)
                    {
                        createAssetsDto.Id = null;

                        createAssetsDto.AssetsDepartmentId = createAssetsMovedDto.NewDepartmentId;

                        createAssetsDto.Customer = createAssetsMovedDto.Receiver;

                        createAssetsDto.DateUsed = createAssetsMovedDto.DateUsed;

                        createAssetsDto.Active = true;

                        createAssetsDto.DateMoved = createAssetsMovedDto.DateMoved;

                        createAssetsDto.DateUsed = createAssetsMovedDto.DateUsed;

                        createAssetsDto.Quantity = createAssetsMovedDto.Quantity;


                        await _assetService.InsertAsset(createAssetsDto);
                    }
                    else
                    {
                        UpdateAssetsDto updateAssetsExistNewDep = _mapper.Map<UpdateAssetsDto>(ItemExistInNewDep);


                        updateAssetsExistNewDep.Quantity = ItemExistInNewDep.Quantity + createAssetsMovedDto.Quantity;

                        updateAssetsExistNewDep.DateUsed = createAssetsMovedDto.DateUsed;

                        updateAssetsExistNewDep.DateMoved = createAssetsMovedDto.DateMoved;

                        updateAssetsExistNewDep.Customer = createAssetsMovedDto.Receiver;

                        if (ItemExistInNewDep.Quantity == 0 && ItemExistInNewDep.Active == false) { updateAssetsExistNewDep.Active = true; updateAssetsExistNewDep.AssetsStatusId = 2; }


                        _context.Assests.Update(_mapper.Map(updateAssetsExistNewDep, ItemExistInNewDep));

                        await _context.SaveChangesAsync();

                    }

                }
                else
                {
                    result = "Mã đơn vị bị trùng với đơn vị sử dụng hiện tại";

                    return result;
                }
                if (asset.Quantity == createAssetsMovedDto.Quantity)
                {
                    asset.DateMoved = createAssetsMovedDto.DateMoved;

                    asset.Customer = createAssetsMovedDto.Receiver;

                    asset.AssetsDepartmentId = createAssetsMovedDto.NewDepartmentId;
                }


                if (updateAssetsDto.Quantity == 0)
                {
                    updateAssetsDto.Customer = null;

                    updateAssetsDto.Active = false;

                    updateAssetsDto.AssetsStatusId = 3;

                }
                _context.Assests.Update(_mapper.Map(updateAssetsDto, asset));

                await _context.SaveChangesAsync();


            }
            var ItemMovedExist = await _context.AssetsMoveds.AsNoTracking().FirstOrDefaultAsync(x => x.NumBravo == createAssetsMovedDto.NumBravo && x.NewDepartmentId.Equals(createAssetsMovedDto.NewDepartmentId) && x.Receiver == createAssetsMovedDto.Receiver && x.OldAssestsId == createAssetsMovedDto.OldAssestsId);

            if (ItemMovedExist != null)
            {
                UpdateAssetsMovedDto updateAssetsMoved = _mapper.Map<UpdateAssetsMovedDto>(ItemMovedExist);

                updateAssetsMoved.DateUsed = createAssetsMovedDto.DateUsed;

                updateAssetsMoved.Quantity = ItemMovedExist.Quantity + createAssetsMovedDto.Quantity;

                updateAssetsMoved.Description = createAssetsMovedDto.Description;

                updateAssetsMoved.ReceiverPhoneNumber = createAssetsMovedDto.ReceiverPhoneNumber;

                updateAssetsMoved.SenderPhoneNumber = createAssetsMovedDto.SenderPhoneNumber;

                _context.AssetsMoveds.Update(_mapper.Map(updateAssetsMoved, ItemMovedExist));

                await _context.SaveChangesAsync();
            }
            else
            {
                var movedBiggestId = await _context.Assests.OrderByDescending(x => x.Id).Select(x => x.Id).ToListAsync();

                createAssetsMovedDto.AssestsId = movedBiggestId[0];
                AssetMoved assetMoved = _mapper.Map<AssetMoved>(createAssetsMovedDto);

                _context.Add(assetMoved);

                await _context.SaveChangesAsync();

            }

            await transaction.CommitAsync();

            result = "Cập nhật thành công!";

            return result;
        }

        public async Task UpdateAssetsMoved(UpdateAssetsMovedDto updateAssetsMovedDto, Guid id)
        {
            var item = await _context.AssetsMoveds.FirstOrDefaultAsync(x => x.Id.Equals(id));

            var firstItem = await _context.AssetsMoveds.AsNoTracking().OrderByDescending(x => x.DateMoved).Select(x => x.NumBravo).FirstAsync();
            if (item != null)
            {
                using var transaction = _context.Database.BeginTransaction();

                var asset = await _context.Assests.FirstOrDefaultAsync(x => x.Id == updateAssetsMovedDto.AssestsId);

                if (asset == null) return;

                updateAssetsMovedDto.AssetsMovedStatusId = 1;

                //updateAssetsMovedDto.Code = asset.AssetId;

                _context.AssetsMoveds.Update(_mapper.Map(updateAssetsMovedDto, item));

                await _context.SaveChangesAsync();

                var firstItemAfterUpdate = await _context.AssetsMoveds.AsNoTracking().OrderByDescending(x => x.DateMoved).Select(x => x.NumBravo).FirstAsync();

                if (item.NumBravo.Equals(firstItem) || item.NumBravo.Equals(firstItemAfterUpdate))
                {
                    UpdateAssetsDto updateAssetsDto = new()
                    {
                        AssetId = asset.AssetId,
                        Active = asset.Active,
                        AssetName = asset.AssetName,
                        AssetsCategoryId = asset.AssetsCategoryId,
                        AssetsDepartmentId = updateAssetsMovedDto.NewDepartmentId,

                        AssetsStatusId = asset.AssetsStatusId,
                        AssetSubCategory = asset.AssetSubCategory,
                        Customer = updateAssetsMovedDto.Receiver,
                        DateBuyed = asset.DateBuyed,
                        DateChecked = asset.DateChecked,
                        DateCreated = asset.DateCreated,
                        DateExpired = asset.DateExpired,
                        DateMoved = updateAssetsMovedDto.DateMoved,
                        DateRepaired = asset.DateRepaired,
                        DateUsed = updateAssetsMovedDto.DateUsed,
                        ExpireDay = asset.ExpireDay,
                        Manager = asset.Manager,
                        Note = asset.Note,
                        OrderNumber = asset.OrderNumber,
                        Price = asset.Price,
                        Quantity = updateAssetsMovedDto.Quantity,
                        RepairLocation = asset.RepairLocation,
                        SeriNumber = asset.SeriNumber,
                        Spec = asset.Spec,
                        AssetsSupplierId = asset.AssetsSupplierId,
                        BusinessModel = asset.BusinessModel,
                        Region = asset.Region,
                    };
                    _context.Assests.Update(_mapper.Map(updateAssetsDto, asset));

                    await _context.SaveChangesAsync();
                }




                await transaction.CommitAsync();
            }
        }

    }
}
