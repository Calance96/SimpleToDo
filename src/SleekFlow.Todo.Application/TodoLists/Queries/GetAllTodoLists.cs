using ErrorOr;
using Mapster;
using MediatR;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoLists.Dtos;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.TodoLists.Queries;

public sealed record GetAllTodoLists(Guid? UserId) : IRequest<ErrorOr<List<TodoListDto>>>;

internal sealed class GetAllTodoListsHandler : IRequestHandler<GetAllTodoLists, ErrorOr<List<TodoListDto>>>
{
    private readonly ITodoListRepository _todoListRepository;

	public GetAllTodoListsHandler(ITodoListRepository todoListRepository)
		=> _todoListRepository = todoListRepository;
	

	public async Task<ErrorOr<List<TodoListDto>>> Handle(GetAllTodoLists request, CancellationToken cancellationToken)
	{
		List<TodoList> todoLists = await _todoListRepository.GetAllAsync(cancellationToken);

		return todoLists
			.Select(list => list.Adapt<TodoListDto>())
			.ToList();
	}
}
