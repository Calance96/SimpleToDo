using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SleekFlow.Todo.Api.Filters;
using SleekFlow.Todo.Api.Services;
using SleekFlow.Todo.Application;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
	options.SuppressInferBindingSourcesForParameters = true;
});

builder.Services
	.RegisterApplicationServices()
	.RegisterInfrastructureServices(builder.Configuration)
	.AddEndpointsApiExplorer()
	.AddSwaggerGen(opts =>
	{
		opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Name = "Authorization",
			Description = "JWT token authentication scheme",
			BearerFormat = "Bearer {token}",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.Http,
			Scheme = "bearer",
			Reference = new()
			{
				Id = "Bearer",
				Type = ReferenceType.SecurityScheme
			}
		});

		opts.OperationFilter<SwaggerSecurityOperationFilter>();
	})
	.AddHttpContextAccessor();

builder.Services
	.AddScoped<ICurrentUserService, CurrentUserService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
