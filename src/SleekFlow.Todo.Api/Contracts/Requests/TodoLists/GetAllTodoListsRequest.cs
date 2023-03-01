using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.TodoLists.Queries;

namespace SleekFlow.Todo.Api.Contracts.Requests.TodoLists;

public sealed record GetAllTodoListsRequest(Guid? UserId) : IQuery<GetAllTodoLists>
{
	public GetAllTodoLists ToQuery()
		=> new(UserId);
}
