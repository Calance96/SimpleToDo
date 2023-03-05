using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Infrastructure.Configurations;
using SleekFlow.Todo.Infrastructure.Identities;
using SleekFlow.Todo.Infrastructure.Persistence;
using SleekFlow.Todo.Infrastructure.Persistence.Interceptors;
using SleekFlow.Todo.Infrastructure.Repositories;
using SleekFlow.Todo.Infrastructure.Services;
using SleekFlow.Todo.Infrastructure.Utilities;

namespace SleekFlow.Todo.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		TokenConfiguration tokenConfiguration = new();
		configuration.GetRequiredSection(TokenConfiguration.Section).Bind(tokenConfiguration);

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
			{
				opts.SaveToken = true;

				opts.TokenValidationParameters = new()
				{
					ValidateIssuer = false,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.FromSeconds(5),
					ValidAudience = tokenConfiguration.ValidAudience,
					IssuerSigningKey = KeyProvider.GetSecurityKey(),
					ValidTypes = new[] { JwtConstants.TokenType }
				};
			});

		services
			.AddDbContext<TodoDbContext>(builder =>
			{
				builder.UseSqlServer(configuration.GetConnectionString("TodoListDatabase")!);
			});

		services
			.Configure<TokenConfiguration>(configuration.GetRequiredSection(TokenConfiguration.Section));

		services
			.AddTransient<IClock, Clock>()
			.AddTransient<IAuthService, AuthService>()
			.AddTransient<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>()
			.AddTransient<ITokenService, TokenService>();
		
		services
			.AddScoped<AuditableEntitySaveChangesInterceptor>()
			.AddScoped<ITodoItemRepository, TodoItemRepository>()
			.AddScoped<ITodoListRepository, TodoListRepository>();

		return services;
	}
}