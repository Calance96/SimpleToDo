using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.Common.Interfaces;

public interface ITodoItemRepository : IGenericRepository<TodoItem>
{
	Task<TodoItem?> GetByIdAsync(Guid itemId, CancellationToken cancellationToken);
}