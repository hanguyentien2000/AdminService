using AdminService.Business.Roles;
using AdminService.Business.User;
using AdminService.Insfrastructure;
using DataUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers.Role
{
    [ApiController]
    [Route("api/role")]

    public class RoleController : ControllerBase
    {
        private readonly IRoleHandler _roleHandler;
        public RoleController(AdminDataContext context)
        {
            var factory = new DatabaseFactory(context);
            _roleHandler = new RoleHandler(factory);
        }

        [AllowAnonymous, HttpGet, Route("byid/{{id}}")]
        public async Task<ActionResult<Response<RoleModel>>> GetById(Guid id)
        {
            var result = await _roleHandler.GetRolesByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [AllowAnonymous, HttpPost, Route("create")]
        public async Task<ActionResult<Response<RoleModel>>> Create(RoleModel model)
        {
            var result = await _roleHandler.CreateRoleAsync(model);
            return result.Success ? CreatedAtAction(nameof(GetById), new { id = model.RoleId }, result) : BadRequest(result);
        }
    }
}
