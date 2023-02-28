using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.Common.Interfaces;

public interface ITodoItemRepository
{
	Task<TodoItem> GetByIdAsync(Guid itemId, CancellationToken cancellationToken);

	Task AddAsync(TodoItem item, CancellationToken cancellationToken);

	Task UpdateAsync(TodoItem item, CancellationToken cancellationToken);

	Task DeleteAsync(Guid itemId, CancellationToken cancellationToken);
}