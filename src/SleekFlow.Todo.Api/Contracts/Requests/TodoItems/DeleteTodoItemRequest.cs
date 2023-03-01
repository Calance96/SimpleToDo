using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Commands;

namespace SleekFlow.Todo.Api.Contracts.Requests.TodoItems;

public sealed record DeleteTodoItemRequest(
	[FromRoute] Guid ItemId) : ICommand<DeleteTodoItem>
{
	public DeleteTodoItem ToCommand()
		=> new(ItemId);
}
