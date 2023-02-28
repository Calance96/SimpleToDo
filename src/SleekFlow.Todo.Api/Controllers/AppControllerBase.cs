using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Responses;
using System.Net.Mime;

namespace SleekFlow.Todo.Api.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public abstract class AppControllerBase : ControllerBase
{
	protected IActionResult MapResponse<TResponse>(ErrorOr<TResponse> response)
		=> response.MatchFirst(
			result => Ok(ApiResponse.Success(result)),
			error => error.Type switch
			{
				ErrorType.NotFound => NotFound(ApiResponse.Failure(error)),
				ErrorType.Unexpected => StatusCode(500, ApiResponse.Failure(error)),
				_ => BadRequest(ApiResponse.Failure(error))
			});

	protected IActionResult MapResponse(ErrorOr<Unit> response)
		=> response.MatchFirst(
			result => Ok(ApiResponse.Success(null)),
			error => error.Type switch
			{
				ErrorType.NotFound => NotFound(ApiResponse.Failure(error)),
				ErrorType.Unexpected => StatusCode(500, ApiResponse.Failure(error)),
				_ => BadRequest(ApiResponse.Failure(error))
			});
}