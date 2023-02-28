using SleekFlow.Todo.Application;
using SleekFlow.Todo.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
	.RegisterApplicationServices()
	.RegisterInfrastructureServices(builder.Configuration)
	.AddEndpointsApiExplorer()
	.AddSwaggerGen();


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
