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
			result => Ok(ApiResponse<TResponse>.Success(result)),
			error => HandleError(error));

	protected IActionResult MapResponse(ErrorOr<Unit> response)
		=> response.MatchFirst(
			result => Ok(BaseApiResponse.Success()),
			error => HandleError(error));

	private IActionResult HandleError(ErrorOr.Error error)
		=> error.Type switch
		{
			ErrorType.NotFound => NotFound(BaseApiResponse.Failure(error)),
			ErrorType.Unexpected => StatusCode(500, BaseApiResponse.Failure(error)),
			_ => BadRequest(BaseApiResponse.Failure(error))
		};
}