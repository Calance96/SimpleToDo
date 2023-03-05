using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Application.Common.Exceptions;

namespace SleekFlow.Todo.Api.Middlewares
{
	public static class ExceptionHandlingMiddlewareExtensions
	{
		public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
		{
			app.UseMiddleware<ExceptionHandlingMiddleware>();

			return app;
		} 
	}

	public sealed class ExceptionHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
			_logger = logger;
		}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			string requestPath = context.Request.Path.Value!;

			_logger.LogError(ex, "An uncaught exception has been thrown while processing request at path {RequestPath}.", requestPath);

			BaseApiResponse baseApiResponse = BaseApiResponse.Failure(Errors.GeneralErrors.GeneralUnexpectedError);

			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Response.ContentType = "application/json";

			await context.Response.WriteAsJsonAsync(baseApiResponse);
		}
	}
}
