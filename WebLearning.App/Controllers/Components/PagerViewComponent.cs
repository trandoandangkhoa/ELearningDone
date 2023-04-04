using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Ultities;

namespace WebLearning.App.Controllers.Component
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagingBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
