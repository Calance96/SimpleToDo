using ErrorOr;
using FluentValidation.Results;
using FluentValidation;
using MediatR;
using SleekFlow.Todo.Application.Common.Exceptions;

namespace SleekFlow.Todo.Application.Common.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : IErrorOr
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		IEnumerable<Task<ValidationResult>> validationTasks = _validators
			.Select(v => v.ValidateAsync(request, cancellationToken));

		IEnumerable<ValidationResult> validationResults = await Task.WhenAll(validationTasks);

		if (validationResults.Any(result => !result.IsValid))
		{
			ValidationFailure validationFailure = validationResults
				.First(x => !x.IsValid)
				.Errors
				.First();

			string errorCode = Errors.GeneralErrors.GeneralValidationErrorCode;

			return (TResponse)(dynamic)Error.Validation(errorCode, validationFailure.ErrorMessage);
		}

		return await next();
	}
}