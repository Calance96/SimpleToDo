using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SleekFlow.Todo.Infrastructure.Persistence;

internal sealed class DbAutoMigrator : IHostedService
{
	private readonly IServiceProvider _serviceProvider;

	public DbAutoMigrator(IServiceProvider serviceProvider)
    {
		_serviceProvider = serviceProvider;
	}

    public async Task StartAsync(CancellationToken cancellationToken)
	{
		using IServiceScope scope = _serviceProvider.CreateAsyncScope();

		TodoDbContext dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

		IEnumerable<string> pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken: cancellationToken);

		if (pendingMigrations.Any())
		{
			await dbContext.Database.MigrateAsync(cancellationToken);
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}