using Microsoft.EntityFrameworkCore;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Infrastructure.Persistence;

namespace SleekFlow.Todo.Infrastructure.Repositories;

internal sealed class TodoListRepository : GenericRepository<TodoList>, ITodoListRepository
{
	private readonly TodoDbContext _context;

	public TodoListRepository(TodoDbContext context) : base(context)
		=> _context = context;

	public Task<List<TodoList>> GetAllAsync(CancellationToken cancellationToken)
		=> _context
			.TodoLists
			.Include(list => list.Items)
			.AsNoTrackingWithIdentityResolution()
			.ToListAsync(cancellationToken);

	public Task<TodoList?> GetByIdAsync(Guid listId, CancellationToken cancellationToken)
		=> _context
			.TodoLists
			.Include(list => list.Items)
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(list => list.Id == listId, cancellationToken);
}