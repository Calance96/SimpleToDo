using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.Common.Interfaces;

public interface ITodoListRepository
{
	Task<List<TodoList>> GetAllAsync(CancellationToken cancellationToken);

	Task<TodoList> GetByIdAsync(Guid listId, CancellationToken cancellationToken);

	Task AddAsync(TodoList todoList, CancellationToken cancellationToken);

	Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken);

	Task DeleteAsync(Guid listId, CancellationToken cancellationToken);
}