using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Application.TodoItems.Dtos;

public sealed class TodoItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTimeOffset? DueDate { get; set; }

    public string Status { get; set; }
}