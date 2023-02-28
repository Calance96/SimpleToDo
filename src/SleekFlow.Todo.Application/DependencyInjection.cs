using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SleekFlow.Todo.Application.Common.Behaviors;
using System.Reflection;

namespace SleekFlow.Todo.Application;

public static class DependencyInjection
{
	public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
	{
		services
			.AddMediatR(config =>
				config
					.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
					.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>))
					.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)))
			.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

		return services;
	}
}