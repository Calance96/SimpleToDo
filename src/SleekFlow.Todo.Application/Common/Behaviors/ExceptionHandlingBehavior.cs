using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using SleekFlow.Todo.Application.Common.Exceptions;

namespace SleekFlow.Todo.Application.Common.Behaviors;

internal sealed class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, ErrorOr<TResponse>>
	where TRequest : IRequest<ErrorOr<TResponse>>
{
	private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

	public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
		=> _logger = logger;

	public async Task<ErrorOr<TResponse>> Handle(TRequest request, RequestHandlerDelegate<ErrorOr<TResponse>> next, CancellationToken cancellationToken)
	{
		ErrorOr<TResponse> response;

		try
		{
			response = await next();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "A crtical error has occurred.");
			response = Errors.GeneralErrors.GeneralUnexpectedError;
		}

		return response;
	}
}