using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Requests.TodoItems;
using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Application.TodoItems.Dtos;

namespace SleekFlow.Todo.Api.Controllers;

[Authorize]
[Route("api/todo-items")]
public sealed class TodoItemsController : AppControllerBase
{
	private readonly IMediator _mediator;

    public TodoItemsController(IMediator mediator)
		=> _mediator = mediator;

    [HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoItemDto>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> AddTodoItem([FromBody] AddTodoItemRequest request, CancellationToken cancellationToken)
	{
		ErrorOr<TodoItemDto> response = await _mediator.Send(request.ToCommand(), cancellationToken);

		return MapResponse(response);
	}

	[HttpPut("{itemId:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> UpdateTodoItem(UpdateTodoItemRequest request, CancellationToken cancellationToken)
	{
		ErrorOr<Unit> response = await _mediator.Send(request.ToCommand(), cancellationToken);

		return MapResponse(response);
	}

	[HttpDelete("{itemId:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> DeleteTodoItem(DeleteTodoItemRequest request, CancellationToken cancellationToken)
	{
		ErrorOr<Unit> response = await _mediator.Send(request.ToCommand(), cancellationToken);

		return MapResponse(response);
	}
}