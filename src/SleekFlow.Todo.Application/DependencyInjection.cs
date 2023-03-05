using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SleekFlow.Todo.Application.Common.Behaviors;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SleekFlow.Todo.Application.UnitTests")]
namespace SleekFlow.Todo.Application;

public static class DependencyInjection
{
	public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
	{
		ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) => member?.Name;

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