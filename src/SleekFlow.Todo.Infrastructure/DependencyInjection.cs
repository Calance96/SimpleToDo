using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Infrastructure.Persistence;
using SleekFlow.Todo.Infrastructure.Persistence.Interceptors;
using SleekFlow.Todo.Infrastructure.Repositories;
using SleekFlow.Todo.Infrastructure.Services;

namespace SleekFlow.Todo.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddDbContext<TodoDbContext>(builder =>
			{
				builder.UseSqlServer(configuration.GetConnectionString("TodoListDatabase"));
			});

		services
			.AddTransient<IClock, Clock>();
		
		services
			.AddScoped<AuditableEntitySaveChangesInterceptor>()
			.AddScoped<ITodoItemRepository, TodoItemRepository>()
			.AddScoped<ITodoListRepository, TodoListRepository>();

		return services;
	}
}