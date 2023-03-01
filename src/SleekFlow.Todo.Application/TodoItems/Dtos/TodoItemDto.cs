using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Application.TodoItems.Dtos;

public sealed class TodoItemDto
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public DateTimeOffset? DueDate { get; private set; }

    public string Status { get; private set; }
}