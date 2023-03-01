using SleekFlow.Todo.Domain.Common;
using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Domain.Entities;

public sealed class TodoItem : AuditableEntity
{
	public Guid Id { get; private set; }

	public string Name { get; private set; } = null!;

	public string Description { get; private set; } = null!;

	public DateTimeOffset? DueDate { get; private set; }

	public TodoItemStatus Status { get; private set; }

	public Guid ListId { get; private set; }

	public static TodoItem Create(string name, string description, DateTimeOffset? dueDate, Guid listId)
		=> new()
		{
			Id = Guid.NewGuid(),
			Name = name,
			Description = description,
			DueDate = dueDate,
			Status = TodoItemStatus.Pending,
			ListId = listId
		};
}