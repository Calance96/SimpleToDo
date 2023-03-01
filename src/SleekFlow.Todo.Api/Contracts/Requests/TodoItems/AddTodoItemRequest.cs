using Mapster;
using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Commands;

namespace SleekFlow.Todo.Api.Contracts.Requests.TodoItems;

public sealed record AddTodoItemRequest(
	Guid ListId,
	string Name,
	string Description,
	DateTimeOffset? DueDate) : ICommand<AddTodoItem>
{
	public AddTodoItem ToCommand()
		=> new(
			ListId,
			new(Name, Description, DueDate));
}