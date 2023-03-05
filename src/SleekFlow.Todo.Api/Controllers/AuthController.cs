using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Requests.Auths;
using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Application.Auths.Dtos;

namespace SleekFlow.Todo.Api.Controllers;

[Route("api/auth")]
public sealed class AuthController : AppControllerBase
{
	private readonly IMediator _mediator;

	public AuthController(IMediator mediator)
		=> _mediator = mediator;

	/// <summary>
	/// To register new user 
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>Access token</returns>
	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TokenResponseDto>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
	{
		ErrorOr<TokenResponseDto> response = await _mediator.Send(request.ToCommand(), cancellationToken);

		return MapResponse(response);
	}

	/// <summary>
	/// To login with existing user
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>Access token</returns>
	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TokenResponseDto>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
	{
		ErrorOr<TokenResponseDto> response = await _mediator.Send(request.ToCommand(), cancellationToken);

		return MapResponse(response);
	}

}