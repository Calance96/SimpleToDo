using Microsoft.EntityFrameworkCore;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Infrastructure.Persistence;

internal sealed class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; private set; }
    public DbSet<TodoList> TodoLists { get; private set; }
}