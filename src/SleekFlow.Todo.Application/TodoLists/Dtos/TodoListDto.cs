using SleekFlow.Todo.Application.TodoItems.Dtos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

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

	public DateTimeOffset LastModifiedAt
	{
		get
		{
			List<DateTimeOffset?> dates = new()
			{
				CreatedAt,
				UpdatedAt
			};

			dates.AddRange(Items.Select(x => (DateTimeOffset?)x.CreatedAt));
			dates.AddRange(Items.Select(x => x.UpdatedAt));

			return dates
				.Where(dateTime => dateTime.HasValue)
				.Max()!
				.Value;
		}
	}
}
