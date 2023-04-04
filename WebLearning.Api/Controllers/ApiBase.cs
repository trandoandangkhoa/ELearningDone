using Microsoft.AspNetCore.Mvc;

namespace WebLearning.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize(Roles = "Admin,Sale")]
    public abstract class ApiBase : ControllerBase
    {
        protected OkObjectResult Success()
        {
            return Ok(new { success = true });
        }
    }
}
