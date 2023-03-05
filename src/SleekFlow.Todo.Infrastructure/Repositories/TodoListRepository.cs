using Microsoft.EntityFrameworkCore;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Infrastructure.Persistence;

namespace SleekFlow.Todo.Infrastructure.Repositories;

internal sealed class TodoListRepository : GenericRepository<TodoList>, ITodoListRepository
{
	private readonly TodoDbContext _context;
	private readonly ICurrentUserService _currentUserService;

	public TodoListRepository(TodoDbContext context, ICurrentUserService currentUserService) : base(context)
	{
		_context = context;
		_currentUserService = currentUserService;
	}

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
			.FirstOrDefaultAsync(list => 
				list.Id == listId &&
				list.CreatedBy == _currentUserService.UserId, cancellationToken);
}