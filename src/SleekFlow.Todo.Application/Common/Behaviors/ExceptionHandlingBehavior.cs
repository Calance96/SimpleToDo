using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using SleekFlow.Todo.Application.Common.Exceptions;

namespace SleekFlow.Todo.Application.Common.Behaviors;

internal sealed class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : IErrorOr
{
	private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

	public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
		=> _logger = logger;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		TResponse response;

		try
		{
			response = await next();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "A crtical error has occurred.");
			response = (TResponse)(dynamic)Errors.GeneralErrors.GeneralUnexpectedError;
		}

		return response;
	}
}