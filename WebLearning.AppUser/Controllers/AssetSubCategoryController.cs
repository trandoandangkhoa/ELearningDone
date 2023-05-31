using Microsoft.AspNetCore.Mvc;
using WebLearning.Contract.Dtos.Assets.SubCategory;

namespace WebLearning.AppUser.Controllers
{
    public class AssetSubCategoryController : Controller
    {
        [HttpPost]
        [Route("chi-tiet-loai-tai-san/{AssetsCategoryId}/tao-moi-model")]
        public IActionResult Create(CreateAssetsSubCategoryDto createAssetsSubCategoryDto)
        {
            return View();
        }
    }
}
