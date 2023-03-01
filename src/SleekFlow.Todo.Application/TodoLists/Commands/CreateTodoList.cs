using ErrorOr;
using FluentValidation;
using Mapster;
using MediatR;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Application.TodoLists.Dtos;
using SleekFlow.Todo.Domain.Constants;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.TodoLists.Commands;

public sealed record CreateTodoList(string Title, IEnumerable<CreateTodoItemDto>? Items) : IRequest<ErrorOr<TodoListDto>>;

internal sealed class CreateTodoListValidator : AbstractValidator<CreateTodoList>
{
	public CreateTodoListValidator()
	{
		ClassLevelCascadeMode = CascadeMode.Stop;

		RuleFor(request => request.Title)
			.NotEmpty()
				.WithMessage("{PropertyName} cannot be empty.")
			.MaximumLength(DomainConstants.TodoList.TitleMaxLength)
				.WithMessage("{PropertyName} cannot exceed {MaxLength} characters.");

		RuleForEach(request => request.Items)
			.ChildRules(childValidator =>
			{
				childValidator.RuleFor(item => item.Name)
					.NotEmpty()
						.WithMessage("Item {CollectionIndex}: {PropertyName} cannot be empty")
					.MaximumLength(DomainConstants.TodoItem.NameMaxLength)
						.WithMessage("Item {CollectionIndex}: {PropertyName} cannot exceed {MaxLength} characters.");

				childValidator.RuleFor(item => item.Description)
					.NotEmpty()
						.WithMessage("Item {CollectionIndex}: {PropertyName} cannot be empty")
					.MaximumLength(DomainConstants.TodoItem.DescriptionMaxLength)
						.WithMessage("Item {CollectionIndex}: {PropertyName} cannot exceed {MaxLength} characters.");

				childValidator.RuleFor(item => item.DueDate)
					.Must(dueDate => dueDate!.Value >= DateTimeOffset.UtcNow)
					.When(item => item.DueDate.HasValue)
						.WithMessage("Item {CollectionIndex}: {PropertyName} cannot be in the past.");
			})
			.When(request => request.Items is not null && request.Items.Any());
	}
}

internal sealed class CreateTodoListHandler : IRequestHandler<CreateTodoList, ErrorOr<TodoListDto>>
{
	private readonly ITodoListRepository _todoListRepository;

	public CreateTodoListHandler(ITodoListRepository todoListRepository)
		=> _todoListRepository = todoListRepository;

	public async Task<ErrorOr<TodoListDto>> Handle(CreateTodoList request, CancellationToken cancellationToken)
	{
		TodoList todoList = TodoList.Create(request.Title);

		request
			.Items?
			.ToList()
			.ForEach(i => 
			{
				TodoItem todoItem = TodoItem.Create(i.Name, i.Description, i.DueDate, todoList.Id);
				todoList.Items.Add(todoItem);
			});

		await _todoListRepository.AddAsync(todoList, cancellationToken);

		return todoList.Adapt<TodoListDto>();
	}
}
