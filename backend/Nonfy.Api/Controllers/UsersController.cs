using Microsoft.AspNetCore.Mvc;
using Nonfy.Api.Contracts.Requests;
using Nonfy.Api.Contracts.Responses;
using Nonfy.Application.UseCases.RegisterUser;
using Nonfy.Domain.Exceptions;

namespace Nonfy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(RegisterUserHandler registerUserHandler) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var command = new RegisterUserCommand(request.Email, request.Password);
            var result = await registerUserHandler.HandleAsync(command, HttpContext.RequestAborted);

            var response = new CreateUserResponse(result.Id, result.Email, result.Slug);
            return Created($"/users/{result.Id}", response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (DuplicateEmailException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error" });
        }
    }
}
