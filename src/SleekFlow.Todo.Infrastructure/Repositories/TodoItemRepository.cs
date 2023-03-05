using Microsoft.EntityFrameworkCore;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Infrastructure.Persistence;

namespace SleekFlow.Todo.Infrastructure.Repositories;

internal sealed class TodoItemRepository : GenericRepository<TodoItem>, ITodoItemRepository
{
	private readonly TodoDbContext _context;
	private readonly ICurrentUserService _currentUserService;

	public TodoItemRepository(
		TodoDbContext context, 
		ICurrentUserService currentUserService) : base(context)
	{
		_context = context;
		_currentUserService = currentUserService;
	}

	public Task<TodoItem?> GetByIdAsync(Guid itemId, CancellationToken cancellationToken)
		=> _context
			.TodoItems
			.AsNoTrackingWithIdentityResolution()
			.FirstOrDefaultAsync(item => 
				item.Id == itemId && 
				item.CreatedBy == _currentUserService.UserId, cancellationToken);
}