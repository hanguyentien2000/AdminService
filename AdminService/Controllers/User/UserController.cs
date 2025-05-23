﻿using AdminService.Business.User;
using AdminService.Insfrastructure;
using AdminService.Insfrastructure.Databases;
using DataUtils;
using EventBusRabbitMqueue.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserHandler _userHandler;
        public UserController(AdminDataContext context)
        {
            var factory = new DatabaseFactory(context);
            _userHandler = new UserHandler(factory);
        }

        [AllowAnonymous, HttpGet, Route("")]
        public async Task<ActionResult<Response<IEnumerable<UserModel>>>> GetAll()
        {
            var result = await _userHandler.GetAllUsersAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous, HttpGet, Route("byid/{{id}}")]
        public async Task<ActionResult<Response<UserModel>>> GetById(Guid id)
        {
            var result = await _userHandler.GetUserByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [AllowAnonymous, HttpPost, Route("create")]
        public async Task<ActionResult<Response<IdmUserInRoleModel>>> Create(IdmUserInRoleModel User)
        {
            var result = await _userHandler.CreateUserAsync(User);
            return result.Success ? CreatedAtAction(nameof(GetById), new { id = User.UserId }, result) : BadRequest(result);
        }

        [AllowAnonymous, HttpPut, Route("update")]
        public async Task<ActionResult<Response<UserModel>>> Update(Guid id, UserModel User)
        {
            if (id != User.UserId)
                return BadRequest(Response<UserModel>.Fail("Mismatched ID"));

            var result = await _userHandler.UpdateUserAsync(User);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous, HttpDelete, Route("delete")]
        public async Task<ActionResult<Response<bool>>> Delete(Guid id)
        {
            var result = await _userHandler.DeleteUserAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost("register")]
        public IActionResult Register([FromServices] IEventPublisher publisher)
        {
            var ev = new UserCreatedEvent
            {
                UserId = Guid.NewGuid(),
                Email = "test@example.com",
                RegisteredAt = DateTime.UtcNow
            };
            publisher.Publish(ev);
            return Ok("User created and event published");
        }
    }
}
