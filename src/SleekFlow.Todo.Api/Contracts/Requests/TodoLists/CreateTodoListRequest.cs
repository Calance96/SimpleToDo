using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Application.TodoLists.Commands;

namespace SleekFlow.Todo.Api.Contracts.Requests.TodoLists;

public sealed record CreateTodoListRequest(
	string Title,
	IEnumerable<CreateTodoItemDto>? Items) : ICommand<CreateTodoList>
{
	public CreateTodoList ToCommand()
		=> new(Title, Items);
}
