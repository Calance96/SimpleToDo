using ErrorOr;
using FluentValidation.Results;
using FluentValidation;
using MediatR;
using SleekFlow.Todo.Application.Common.Exceptions;

namespace SleekFlow.Todo.Application.Common.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, ErrorOr<TResponse>>
	where TRequest : IRequest<ErrorOr<TResponse>>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<ErrorOr<TResponse>> Handle(TRequest request, RequestHandlerDelegate<ErrorOr<TResponse>> next, CancellationToken cancellationToken)
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

			string errorCode = string.IsNullOrWhiteSpace(validationFailure.ErrorCode) ? Errors.GeneralErrors.GeneralValidationErrorCode : validationFailure.ErrorCode;

			return Error.Validation(errorCode, validationFailure.ErrorMessage);
		}

		return await next();
	}
}