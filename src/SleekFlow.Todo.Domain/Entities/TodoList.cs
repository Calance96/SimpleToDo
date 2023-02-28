using SleekFlow.Todo.Domain.Common;

namespace SleekFlow.Todo.Domain.Entities;

public sealed class TodoList : AuditableEntity
{
	public Guid Id { get; private set; }
	public string Title { get; private set; } = null!;
	public ICollection<TodoItem> Items { get; private set; } = new List<TodoItem>();

	public static TodoList Create(string title)
		=> new()
		{
			Id = Guid.NewGuid(),
			Title = title
		};
}