using Microsoft.EntityFrameworkCore;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace SleekFlow.Todo.Infrastructure.Persistence;

internal sealed class TodoDbContext : DbContext
{
	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

	public TodoDbContext(
		DbContextOptions<TodoDbContext> options,
		AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
    {
		_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
	}

    public DbSet<TodoItem> TodoItems { get; private set; }
    public DbSet<TodoList> TodoLists { get; private set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(
			_auditableEntitySaveChangesInterceptor);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("TodoApp");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}