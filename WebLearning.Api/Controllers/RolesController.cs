using Microsoft.AspNetCore.Mvc;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Role;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ApiBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRoleService _roleService;
        public RolesController(ILogger<RolesController> logger, IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }
        // GET: api/<RoleController>
        [HttpGet]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]

        public async Task<IEnumerable<RoleDto>> GetRoles()
        {

            return await _roleService.GetRole();

        }
        [HttpGet("/paging")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<PagedViewModel<RoleDto>> GetUsers([FromQuery] GetListPagingRequest getListPagingRequest)
        {

            return await _roleService.GetPaging(getListPagingRequest);

        }

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StudentRole)]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            if (await _roleService.GetRoleById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _roleService.GetRoleById(id));

        }

        // POST api/<RoleController>
        [HttpPost]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> Post([FromBody] CreateRoleDto createRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                await _roleService.InsertRole(createRoleDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                if (updateRoleDto == null)
                    return BadRequest();
                if (await _roleService.GetRoleById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                if (ModelState.IsValid)
                {
                    await _roleService.UpdateRole(updateRoleDto, id);

                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole)]

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                if (await _roleService.GetRoleById(id) == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
                await _roleService.DeleteRole(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
