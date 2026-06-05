using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieStreaming.Application.Command.UserCommands;
using MovieStreaming.Application.DTOs;
using MovieStreaming.Application.Queries.UserQueries;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Domain.Common.Result;
using System.Net.NetworkInformation;

namespace MovieStreaming.WebApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UserController : Controller
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var command = new GetUserByIdQuery(id);
            var result= await _sender.Send(command);
            return result==null ? NotFound() : Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserDto dto)
        {
            Guid id = Guid.NewGuid();
            var command = new CreateUserCommand(id,dto);
            var result =await _sender.Send(command);
            if(result.IsSuccess)return Ok(result);
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);


        }
    }
}
