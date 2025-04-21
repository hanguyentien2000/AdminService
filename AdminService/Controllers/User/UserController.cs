using AdminService.Business.User;
using AdminService.Insfrastructure;
using DataUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AdminService.Controllers.User
{
    public class UserController : ControllerBase
    {
        private readonly IUserHandler _userHandler;
        public UserController(AdminDataContext context)
        {
            var factory = new DatabaseFactory(context);
            _userHandler = new UserHandler(factory);
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<UserModel>>>> GetAll()
        {
            var result = await _userHandler.GetAllUsersAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<UserModel>>> GetById(Guid id)
        {
            var result = await _userHandler.GetUserByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<ActionResult<Response<UserModel>>> Create(UserModel User)
        {
            var result = await _userHandler.CreateUserAsync(User);
            return result.Success ? CreatedAtAction(nameof(GetById), new { id = User.Id }, result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<UserModel>>> Update(Guid id, UserModel User)
        {
            if (id != User.Id)
                return BadRequest(Response<UserModel>.Fail("Mismatched ID"));

            var result = await _userHandler.UpdateUserAsync(User);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> Delete(Guid id)
        {
            var result = await _userHandler.DeleteUserAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
