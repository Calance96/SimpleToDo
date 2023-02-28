using SleekFlow.Todo.Application.TodoItems.Dtos;

namespace SleekFlow.Todo.Application.TodoLists.Dtos;

public sealed class TodoListDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public IReadOnlyCollection<TodoItemDto> Items { get; set; } = new List<TodoItemDto>();
	public string CreatedBy { get; set; } = null!;
	public DateTimeOffset CreatedAt { get; set; }
	public string? UpdatedBy { get; set; } = null!;
	public DateTimeOffset? UpdatedAt { get; set; }
}
