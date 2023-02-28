namespace SleekFlow.Todo.Application.TodoItems.Dtos;

public sealed record CreateTodoItemDto(
	string Name,
	string Description,
	DateTimeOffset? DueDate);