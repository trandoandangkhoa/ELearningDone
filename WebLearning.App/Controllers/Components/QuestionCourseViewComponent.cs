using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Ultities;

namespace WebLearning.App.Controllers.Components
{
    public class QuestionCourseViewComponent : ViewComponent
    {


        private readonly IQuestionCourseService _questionCourseService;




        public QuestionCourseViewComponent(IQuestionCourseService questionCourseService)
        {
            _questionCourseService = questionCourseService;

        }
        public async Task<IViewComponentResult> InvokeAsync(string keyword, int pageIndex = 1)
        {
            var request = new GetListPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
            };
            var Roke = await _questionCourseService.GetPaging(request);

            return View(Roke);
        }

    }
}
