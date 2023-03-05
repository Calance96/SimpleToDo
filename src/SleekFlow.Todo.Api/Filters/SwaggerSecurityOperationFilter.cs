using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SleekFlow.Todo.Api.Filters
{
	public class SwaggerSecurityOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			// Check if the controller has Authorize attribute
			bool isAuthorized = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ?? false;

			if (!isAuthorized)
			{
				// If controller no Authorize attribute, check if the action has Authorize attribute
				isAuthorized = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
			}
			else
			{
				// If controller has Authorize attribute, check if the action has AllowAnonymous attribute
				isAuthorized = !context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
			}

			if (isAuthorized)
			{
				operation.Security.Add(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer",
								Type = ReferenceType.SecurityScheme
							}
						}, new List<string>()
					}
				});
			}
		}
	}
}
