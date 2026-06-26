using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStreaming.Application.Command.UserCommands;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Queries.UserQueries;

namespace MovieStreaming.WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _sender.Send(query);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            Guid id = Guid.NewGuid();
            var command = new CreateUserCommand(id, dto);
            var result = await _sender.Send(command);

            if (result.IsSuccess) return Ok(result.Value);
            return BadRequest(result.Error); // Avoid hardcoding 500 errors for predictable domain failures
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var command = new LoginUserCommand(dto);
            var result = await _sender.Send(command);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}