using ErrorOr;
using FluentValidation;
using Mapster;
using MediatR;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Constants;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.TodoLists.Commands;

public sealed record UpdateTodoList(Guid ListId, string Title) : IRequest<ErrorOr<Unit>>;

internal sealed class UpdateTodoListValidator : AbstractValidator<UpdateTodoList>
{
	public UpdateTodoListValidator()
	{
		ClassLevelCascadeMode = CascadeMode.Stop;

		RuleFor(request => request.ListId)
			.Must(value => !Guid.Empty.Equals(value))
				.WithMessage("{PropertyName} is invalid.");

		RuleFor(request => request.Title)
			.NotEmpty()
				.WithMessage("{PropertyName} cannot be empty.")
			.MaximumLength(DomainConstants.TodoList.TitleMaxLength)
				.WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");
	}
}

internal sealed class UpdateTodoListHandler : IRequestHandler<UpdateTodoList, ErrorOr<Unit>>
{
	private readonly ITodoListRepository _todoListRepository;

	public UpdateTodoListHandler(ITodoListRepository todoListRepository)
		=> _todoListRepository = todoListRepository;


	public async Task<ErrorOr<Unit>> Handle(UpdateTodoList request, CancellationToken cancellationToken)
	{
		TodoList? todoList = await _todoListRepository.GetByIdAsync(request.ListId, cancellationToken);

		if (todoList is null)
		{
			return Errors.TodoListErrors.NotFound(request.ListId);
		}

		todoList = request.Adapt<TodoList>();

		await _todoListRepository.UpdateAsync(todoList, cancellationToken);

		return Unit.Value;
	}
}