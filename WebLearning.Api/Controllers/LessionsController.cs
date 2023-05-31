using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Lession.LessionAdminView;
using WebLearning.Contract.Dtos.LessionFileDocument;
using WebLearning.Contract.Dtos.VideoLession;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessionsController : ApiBase
    {
        private readonly ILogger<LessionsController> _logger;
        private readonly ILessionService _lessionService;
        public LessionsController(ILogger<LessionsController> logger, ILessionService lessionService)
        {
            _logger = logger;
            _lessionService = lessionService;
        }
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IEnumerable<LessionDto>> GetAllLession()
        {

            return await _lessionService.GetLessionDtos();

        }


        [HttpGet("Lessionpaging")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<PagedViewModel<LessionDto>> GetCourses([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _lessionService.GetPaging(getListPagingRequest);

        }

        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetLessionById(Guid id)
        {
            if (await _lessionService.GetLessionById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _lessionService.GetLessionById(id));

        }
        [HttpGet("GetName")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]
        public async Task<IActionResult> GetNameLession(Guid id)
        {
            if (await _lessionService.GetName(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _lessionService.GetName(id));
        }

        [HttpGet("UserGetLessionById/{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UserGetLessionById(Guid id, string accountName)
        {
            if (await _lessionService.UserGetLessionById(id, accountName) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _lessionService.UserGetLessionById(id, accountName));

        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [Consumes("multipart/form-data")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Post([FromForm] CreateLessionAdminView createLessionAdminView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _lessionService.CreateLessionAdminView(createLessionAdminView);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }



        [HttpPut("{id}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [Consumes("multipart/form-data")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateLessionDto updateLessionDto)
        {
            try
            {
                if (updateLessionDto == null)
                    return BadRequest();
                if (await _lessionService.GetLessionById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"User with Id = {id} not found");
                }
                if (ModelState.IsValid)
                {
                    await _lessionService.UpdateLessionDto(id, updateLessionDto);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }



        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (await _lessionService.GetLessionById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _lessionService.DeleteLessionDto(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

        [HttpPost("VideoLessions/{lessionId}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> CreateImage(Guid lessionId, [FromForm] CreateLessionVideoDto createLessionVideoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _lessionService.AddImage(lessionId, createLessionVideoDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }

        }

        [HttpPost("FileDoucment/{lessionId}")]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> CreateFileDocument(Guid lessionId, [FromForm] CreateLessionFileDocumentDto createLessionFileDocumentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _lessionService.AddDoucment(lessionId, createLessionFileDocumentDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }

        }

        [HttpPut("VideoLessions/{imageId}")]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]

        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> UpdateImage(Guid imageId, [FromForm] UpdateLessionVideoDto updateLessionVideoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _lessionService.UpdateImage(imageId, updateLessionVideoDto);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }


        [HttpDelete("VideoLessions/{imageId}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> RemoveImage(Guid imageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _lessionService.RemoveImage(imageId);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }

        }

        [HttpDelete("DocumentLessions/{imageId}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> RemoveDocument(Guid imageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _lessionService.RemoveDocument(imageId);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }

        }

        [HttpGet("VideoLessions/{imageId}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<IActionResult> GetImageById(Guid courseId, Guid imageId)
        {
            var image = await _lessionService.GetImageById(imageId);
            if (image == null)
                return BadRequest("Cannot find image");
            return Ok(image);
        }
    }
}
