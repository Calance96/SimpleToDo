using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.TodoLists.Commands;

namespace SleekFlow.Todo.Api.Contracts.Requests.TodoLists;

public sealed class UpdateTodoListRequest : ICommand<UpdateTodoList>
{
	[FromRoute(Name = "listId")]
	public Guid ListId { get; init; }

	[FromBody]
	public UpdateTodoListRequestBody Body { get; init; } = new();

	public UpdateTodoList ToCommand()
		=> new(ListId, Body.Title);
}

public sealed class UpdateTodoListRequestBody
{
	public string Title { get; init; } = default!;
}