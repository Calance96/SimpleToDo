using ErrorOr;
using FluentValidation;
using Mapster;
using MediatR;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Domain.Constants;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.TodoItems.Commands;

public sealed record AddTodoItem(Guid ListId, CreateTodoItemDto Item) : IRequest<ErrorOr<TodoItemDto>>;

internal sealed class AddTodoItemValidator : AbstractValidator<AddTodoItem>
{
    public AddTodoItemValidator()
    {
		ClassLevelCascadeMode = CascadeMode.Stop;

		RuleFor(request => request.ListId)
			.Must(value => !Guid.Empty.Equals(value))
				.WithMessage("{PropertyName} is invalid.");

		RuleFor(request => request.Item)
			.NotNull()
				.WithMessage("{PropertyName} cannot be null.");

		RuleFor(request => request.Item.Name)
			.NotEmpty()
				.WithMessage("{PropertyName} cannot be empty.")
			.MaximumLength(DomainConstants.TodoItem.NameMaxLength)
				.WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

		RuleFor(request => request.Item.Description)
			.NotEmpty()
				.WithMessage("{PropertyName} cannot be empty")
			.MaximumLength(DomainConstants.TodoItem.DescriptionMaxLength)
				.WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

		RuleFor(request => request.Item.DueDate)
			.Must(dueDate => dueDate!.Value >= DateTimeOffset.UtcNow)
			.When(request => request.Item.DueDate.HasValue)
				.WithMessage("{PropertyName} cannot be in the past.");
	}
}

internal sealed class AddTodoItemHandler : IRequestHandler<AddTodoItem, ErrorOr<TodoItemDto>>
{
	private readonly ITodoListRepository _todoListRepository;
	private readonly ITodoItemRepository _todoItemRepository;

	public AddTodoItemHandler(
		ITodoListRepository todoListRepository,
		ITodoItemRepository todoItemRepository)
	{
		_todoListRepository = todoListRepository;
		_todoItemRepository = todoItemRepository;
	}

	public async Task<ErrorOr<TodoItemDto>> Handle(AddTodoItem request, CancellationToken cancellationToken)
	{
		TodoList todoList = await _todoListRepository.GetByIdAsync(request.ListId, cancellationToken);

		if (todoList is null)
		{
			return Errors.TodoListErrors.NotFound(request.ListId);
		}

		TodoItem todoItem = TodoItem.Create(
			request.Item.Name,
			request.Item.Description,
			request.Item.DueDate);

		await _todoItemRepository.AddAsync(todoItem, cancellationToken);

		return todoItem.Adapt<TodoItemDto>();
	}
}
