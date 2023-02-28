using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SleekFlow.Todo.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		return services;
	}
}