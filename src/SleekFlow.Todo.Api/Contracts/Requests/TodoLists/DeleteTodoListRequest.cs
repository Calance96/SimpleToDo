using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.TodoLists.Commands;

namespace SleekFlow.Todo.Api.Contracts.Requests.TodoLists;

public sealed record DeleteTodoListRequest(
	[FromRoute] Guid ListId) : ICommand<DeleteTodoList>
{
	public DeleteTodoList ToCommand()
		=> new(ListId);
}
