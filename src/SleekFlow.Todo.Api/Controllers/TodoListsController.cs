using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Requests.TodoLists;
using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Application.TodoLists.Dtos;

namespace SleekFlow.Todo.Api.Controllers;

[Route("api/todo-lists")]
public sealed class TodoListsController : AppControllerBase
{
	private readonly IMediator _mediator;

	public TodoListsController(IMediator mediator)
		=> _mediator = mediator;

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<TodoListDto>>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> GetTodoLists(CancellationToken cancellationToken)
	{
		GetAllTodoListsRequest request = new(null);

		ErrorOr<List<TodoListDto>> response = await _mediator.Send(request.ToQuery(), cancellationToken);

		return MapResponse(response);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoListDto>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> CreateTodoList([FromBody] CreateTodoListRequest request, CancellationToken cancellationToken)
	{
		ErrorOr<TodoListDto> response = await _mediator.Send(request.ToCommand(), cancellationToken);

		return MapResponse(response);
	}

	[HttpPut("{listId:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> UpdateTodoList(UpdateTodoListRequest request, CancellationToken cancellationToken)
	{
		ErrorOr<Unit> response = await _mediator.Send(request.ToCommand(), cancellationToken);

		return MapResponse(response);
	}

	[HttpDelete("{listId:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseApiResponse))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseApiResponse))]
	public async Task<IActionResult> DeleteTodoList(DeleteTodoListRequest request, CancellationToken cancellationToken)
	{
		ErrorOr<Unit> response = await _mediator.Send(request.ToCommand(), cancellationToken);

		return MapResponse(response);
	}
}
