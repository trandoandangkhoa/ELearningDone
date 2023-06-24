using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Assets.Services;
using WebLearning.Contract.Dtos.Assets.Chart;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly ILogger<ChartsController> _logger;
        private readonly IChartService _chartService;

        public ChartsController(ILogger<ChartsController> logger, IChartService chartService)
        {
            _logger = logger;
            _chartService = chartService;
        }
        [HttpGet("barchart")]
        public async Task<IEnumerable<TotalAsset>> GetTotalInCategory()
        {
            return await _chartService.TotalAssetsCategory();

        }
        [HttpGet("piechart")]
        public async Task<IEnumerable<TotalAsset>> GetTotalInStatus()
        {
            var total = await _chartService.TotalAssetsStatus();

            return total;
        }
        [HttpGet("donutchart")]
        public async Task<IEnumerable<TotalAsset>> GetTotalAvailable()
        {
            var total = await _chartService.TotalAssetsAvailable();

            return total;
        }
    }
}
