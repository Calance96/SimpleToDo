using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.TodoLists.Queries;

namespace SleekFlow.Todo.Api.Contracts.Requests.TodoLists;

public sealed record GetAllTodoListsRequest(string? SortBy, string? SortOrder) : IQuery<GetAllTodoLists>
{
	public GetAllTodoLists ToQuery()
		=> new(SortBy, SortOrder);
}