using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Services;
using SleekFlow.Todo.Application;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});

builder.Services
	.RegisterApplicationServices()
	.RegisterInfrastructureServices(builder.Configuration)
	.AddEndpointsApiExplorer()
	.AddSwaggerGen()
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

app.UseAuthorization();

app.MapControllers();

app.Run();
