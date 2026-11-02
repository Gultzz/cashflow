using CashFlow.Application.UseCases.Users.ChangePasswordUseCase;
using CashFlow.Application.UseCases.Users.DeleteAccount;
using CashFlow.Application.UseCases.Users.GetProfile;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request
    )
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> GetProfile(
       [FromServices] IGetUserProfileUseCase useCase
   )
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile(
       [FromServices] IGetUserProfileUseCase useCase
   )
    {
        await useCase.Execute();

        return NoContent();
    }

    [HttpPut("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(
       [FromServices] IChangePasswordUseCase useCase,
       [FromBody] RequestChangePasswordJson requestChangePassword
   )
    {
        await useCase.Execute(requestChangePassword);

        return NoContent();
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAccount(
       [FromServices] IDeleteAccountUseCase useCase
   )
    {
        await useCase.Execute();

        return NoContent();
    }
}
