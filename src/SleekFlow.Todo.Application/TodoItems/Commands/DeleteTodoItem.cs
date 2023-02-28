using ErrorOr;
using FluentValidation;
using MediatR;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.TodoItems.Commands;

public sealed record DeleteTodoItem(Guid ItemId) : IRequest<ErrorOr<Unit>>;

internal sealed class DeleteTodoItemValidator : AbstractValidator<DeleteTodoItem>
{
    public DeleteTodoItemValidator()
    {
		ClassLevelCascadeMode = CascadeMode.Stop;

		RuleFor(request => request.ItemId)
			.Must(value => !Guid.Empty.Equals(value))
				.WithMessage("{PropertyName} is invalid.");
	}
}

internal sealed class DeleteTodoItemHandler : IRequestHandler<DeleteTodoItem, ErrorOr<Unit>>
{
	private readonly ITodoItemRepository _todoItemRepository;

	public DeleteTodoItemHandler(ITodoItemRepository todoItemRepository)
		=> _todoItemRepository = todoItemRepository;

	public async Task<ErrorOr<Unit>> Handle(DeleteTodoItem request, CancellationToken cancellationToken)
	{
		TodoItem todoItem = await _todoItemRepository.GetByIdAsync(request.ItemId, cancellationToken);

		if (todoItem is null)
		{
			return Errors.TodoItemErrors.NotFound(request.ItemId);
		}

		await _todoItemRepository.DeleteAsync(request.ItemId, cancellationToken);

		return Unit.Value;
	}
}
