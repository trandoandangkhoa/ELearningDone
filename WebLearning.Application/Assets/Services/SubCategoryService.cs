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
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Assets.SubCategory;
using WebLearning.Contract.Dtos.BookingCalender.Room;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Assets.Services
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<AssetsSubCategoryDto>> GetAssetsSubCategory();
        Task<AssetsSubCategoryDto> GetAssetsSubCategoryById(Guid id);
        Task InsertAssetsSubCategory(CreateAssetsSubCategoryDto createAssetsSubCategoryDto);
        Task<AssetsSubCategoryDto> GetName(string name);
        Task DeleteAssetsSubCategory(Guid id);
        Task UpdateAssetsSubCategory(UpdateAssetsSubCategoryDto updateAssetsSubCategoryDto, Guid id);
        Task<PagedViewModel<AssetsSubCategoryDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<AssetsSubCategoryDto> GetCode(string code);
    }
    public class SubCategoryService : ISubCategoryService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public SubCategoryService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteAssetsSubCategory(Guid id)
        {

            var asset = await _context.AssetsSubCategories.FindAsync(id);
            _context.AssetsSubCategories.Remove(asset);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<AssetsSubCategoryDto>> GetAssetsSubCategory()
        {
            var asset = await _context.AssetsSubCategories.Include(x => x.AssetsCategory).OrderByDescending(x => x.SubCode).AsNoTracking().ToListAsync();
            var assetDto = _mapper.Map<List<AssetsSubCategoryDto>>(asset);
            return assetDto;
        }

        public async Task<AssetsSubCategoryDto> GetAssetsSubCategoryById(Guid id)
        {
            var asset = await _context.AssetsSubCategories.Include(x => x.AssetsCategory).FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<AssetsSubCategoryDto>(asset);
        }

        public async Task<AssetsSubCategoryDto> GetCode(string code)
        {
            var asset = await _context.AssetsSubCategories.Include(x => x.AssetsCategory).FirstOrDefaultAsync(x => x.SubCode == code);

            return _mapper.Map<AssetsSubCategoryDto>(asset);
        }

        public Task<AssetsSubCategoryDto> GetName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedViewModel<AssetsSubCategoryDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var pageResult = _configuration.GetValue<float>("PageSize:AssetsSubCategory");
            var pageCount = Math.Ceiling(_context.AssetsSubCategories.Count() / (double)pageResult);
            var query = _context.AssetsSubCategories.OrderByDescending(x => x.SubCode).AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword) || x.SubCode.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * (int)pageResult)
                                    .Take((int)pageResult)
                                    .Select(x => new AssetsSubCategoryDto()
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        SubCode = x.SubCode,
                                    })
                                    .OrderByDescending(x => x.SubCode).ToListAsync();
            var assetResponse = new PagedViewModel<AssetsSubCategoryDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return assetResponse;
        }

        public async Task InsertAssetsSubCategory(CreateAssetsSubCategoryDto createAssetsSubCategoryDto)
        {
            createAssetsSubCategoryDto.Id = Guid.NewGuid();
            string s = _configuration.GetValue<string>("Code:AssetsSubCategory");


            if (_context.AssetsSubCategories.Any(x => x.Name.Equals(createAssetsSubCategoryDto.Name)) == false)
            {
                var list = await _context.AssetsSubCategories.AsNoTracking().OrderByDescending(x => x.SubCode).ToListAsync();

                if(list.Count == 0) { createAssetsSubCategoryDto.SubCode = s+"00000001"; }

                else if(list.Count > 0)
                {
                int k = Convert.ToInt32(list[0].SubCode.Substring(6, 8)) + 1;
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
                    s +=  "0";
                s += k.ToString();
                    createAssetsSubCategoryDto.SubCode = s;
                }
                AssetsSubCategory asset = _mapper.Map<AssetsSubCategory>(createAssetsSubCategoryDto);

                _context.Add(asset);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task UpdateAssetsSubCategory(UpdateAssetsSubCategoryDto updateAssetsSubCategoryDto, Guid id)
        {
            var item = await _context.AssetsSubCategories.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {

                _context.AssetsSubCategories.Update(_mapper.Map(updateAssetsSubCategoryDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
