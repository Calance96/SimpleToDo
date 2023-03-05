using ErrorOr;
using Mapster;
using MediatR;
using SleekFlow.Todo.Application.Common.Enums;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoLists.Dtos;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.TodoLists.Queries;

public sealed record GetAllTodoLists : IRequest<ErrorOr<List<TodoListDto>>>
{
	public TodoListSortableField SortBy { get; }
	public SortOrder SortOrder { get; }

	public GetAllTodoLists(string? sortBy, string? sortOrder)
	{
		SortBy = Enum.TryParse<TodoListSortableField>(sortBy, true, out var sortByValue) ?
			sortByValue :
			TodoListSortableField.Date;

		SortOrder = Enum.TryParse<SortOrder>(sortOrder, true, out var sortOrderValue) ?
			sortOrderValue :
			SortOrder.Desc;
	}
}

internal sealed class GetAllTodoListsHandler : IRequestHandler<GetAllTodoLists, ErrorOr<List<TodoListDto>>>
{
	private readonly ITodoListRepository _todoListRepository;

	public GetAllTodoListsHandler(ITodoListRepository todoListRepository)
		=> _todoListRepository = todoListRepository;

	public async Task<ErrorOr<List<TodoListDto>>> Handle(GetAllTodoLists request, CancellationToken cancellationToken)
	{
		List<TodoList> todoLists = await _todoListRepository.GetAllAsync(cancellationToken);

		List<TodoListDto> todoListDtos = Sort(
			todoLists.Select(list => list.Adapt<TodoListDto>()),
			request.SortBy,
			request.SortOrder);

		todoListDtos.ForEach(list =>
		{
			list.Items = list.Items
				.OrderBy(x => x.UpdatedAt)
				.ThenBy(x => x.CreatedAt)
				.ToList();
		});

		return todoListDtos;
	}

	private static List<TodoListDto> Sort(IEnumerable<TodoListDto> todoLists, TodoListSortableField sortBy, SortOrder sortOrder)
		=> (sortBy, sortOrder) switch
		{
			(TodoListSortableField.Title, SortOrder.Asc)
				=> todoLists
					.OrderBy(l => l.Title)
					.ToList(),
			(TodoListSortableField.Title, SortOrder.Desc)
				=> todoLists
					.OrderByDescending(l => l.Title)
					.ToList(),
			(TodoListSortableField.Date, SortOrder.Asc)
				=> todoLists
					.OrderBy(l => l.LastModifiedAt)
					.ToList(),
			_ => todoLists
					.OrderByDescending(l => l.LastModifiedAt)
					.ToList(),
		};
}