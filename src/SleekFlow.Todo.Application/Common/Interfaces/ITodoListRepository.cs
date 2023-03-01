using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.Common.Interfaces;

public interface ITodoListRepository : IGenericRepository<TodoList>
{
	Task<TodoList?> GetByIdAsync(Guid listId, CancellationToken cancellationToken);

	Task<List<TodoList>> GetAllAsync(CancellationToken cancellationToken);
}