using ErrorOr;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Extensions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Constants;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Application.TodoItems.Commands;

public sealed record UpdateTodoItem(
	Guid ItemId,
	string Name,
	string Description,
	TodoItemStatus? Status,
	DateTimeOffset? DueDate) : IRequest<ErrorOr<Unit>>;

internal sealed class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItem>
{
    public UpdateTodoItemValidator()
    {
		ClassLevelCascadeMode = CascadeMode.Stop;

		RuleFor(request => request.ItemId)
			.Must(value => !Guid.Empty.Equals(value))
				.WithMessage("{PropertyName} is invalid.");

		RuleFor(request => request.Name)
			.NotEmpty()
				.WithMessage("{PropertyName} cannot be empty.")
			.MaximumLength(DomainConstants.TodoItem.NameMaxLength)
				.WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

		RuleFor(request => request.Description)
			.NotEmpty()
				.WithMessage("{PropertyName} cannot be empty")
			.MaximumLength(DomainConstants.TodoItem.DescriptionMaxLength)
				.WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

		RuleFor(request => request.Status)
			.Must(status => status.HasValue)
				.WithMessage($"{{PropertyName}} must have a valid status. e.g. {typeof(TodoItemStatus).GetCommaSeparatedEnumValues()}");

		RuleFor(request => request.DueDate)
			.Must(dueDate => dueDate!.Value >= DateTimeOffset.UtcNow)
			.When(request => request.DueDate.HasValue)
				.WithMessage("{PropertyName} cannot be in the past.");
	}
}

internal sealed class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItem, ErrorOr<Unit>>
{
	private readonly ITodoItemRepository _todoItemRepository;

	public UpdateTodoItemHandler(ITodoItemRepository todoItemRepository)
		=> _todoItemRepository = todoItemRepository;

	public async Task<ErrorOr<Unit>> Handle(UpdateTodoItem request, CancellationToken cancellationToken)
	{
		TodoItem? todoItem = await _todoItemRepository.GetByIdAsync(request.ItemId, cancellationToken);

		if (todoItem is null)
		{
			return Errors.TodoItemErrors.NotFound(request.ItemId);
		}

		todoItem.UpdateFrom(
			request.Name,
			request.Description,
			request.Status.GetValueOrDefault(TodoItemStatus.Pending),
			request.DueDate);

		await _todoItemRepository.UpdateAsync(todoItem, cancellationToken);

		return Unit.Value;
	}
}
