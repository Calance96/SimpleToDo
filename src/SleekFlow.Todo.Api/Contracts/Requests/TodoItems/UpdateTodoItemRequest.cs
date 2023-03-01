using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Commands;
using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Api.Contracts.Requests.TodoItems;

public sealed class UpdateTodoItemRequest : ICommand<UpdateTodoItem>
{
	[FromRoute]
	public Guid ItemId { get; init; }

	[FromBody]
	public UpdateTodoItemRequestBody TodoItemDetails { get; init; } = new();

	public UpdateTodoItem ToCommand()
		=> new 
		(
			ItemId,
			TodoItemDetails.Name,
			TodoItemDetails.Description,
			Enum.TryParse<TodoItemStatus>(TodoItemDetails.Status, out var status) ? status : null,
			TodoItemDetails.DueDate
		);
}

public sealed class UpdateTodoItemRequestBody
{
	public string Name { get; init; } = default!;
	public string Description { get; init; } = default!;
	public string Status { get; init; } = default!;
	public DateTimeOffset? DueDate { get; init; }
}