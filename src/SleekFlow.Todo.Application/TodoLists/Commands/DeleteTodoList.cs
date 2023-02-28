using ErrorOr;
using FluentValidation;
using MediatR;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.TodoLists.Commands;

public sealed record DeleteTodoList(Guid ListId) : IRequest<ErrorOr<Unit>>;

internal sealed class DeleteTodoListValidator : AbstractValidator<DeleteTodoList>
{
	public DeleteTodoListValidator()
	{
		ClassLevelCascadeMode = CascadeMode.Stop;

		RuleFor(request => request.ListId)
			.Must(value => !Guid.Empty.Equals(value))
				.WithMessage("{PropertyName} is invalid.");
	}
}

internal sealed class DeleteTodoListHandler : IRequestHandler<DeleteTodoList, ErrorOr<Unit>>
{
	private readonly ITodoListRepository _todoListRepository;

	public DeleteTodoListHandler(ITodoListRepository todoListRepository)
		=> _todoListRepository = todoListRepository;
	

	public async Task<ErrorOr<Unit>> Handle(DeleteTodoList request, CancellationToken cancellationToken)
	{
		TodoList todoList = await _todoListRepository.GetByIdAsync(request.ListId, cancellationToken);

		if (todoList is null)
		{
			return Errors.TodoListErrors.NotFound(request.ListId);
		}

		await _todoListRepository.DeleteAsync(request.ListId, cancellationToken);

		return Unit.Value;
	}
}
