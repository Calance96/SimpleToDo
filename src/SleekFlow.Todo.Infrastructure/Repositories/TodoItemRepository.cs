using Microsoft.EntityFrameworkCore;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Infrastructure.Persistence;

namespace SleekFlow.Todo.Infrastructure.Repositories;

internal sealed class TodoItemRepository : GenericRepository<TodoItem>, ITodoItemRepository
{
	private readonly TodoDbContext _context;

	public TodoItemRepository(TodoDbContext context) : base(context)
		=> _context = context;

	public Task<TodoItem?> GetByIdAsync(Guid itemId, CancellationToken cancellationToken)
		=> _context
			.TodoItems
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(item => item.Id == itemId, cancellationToken);
}