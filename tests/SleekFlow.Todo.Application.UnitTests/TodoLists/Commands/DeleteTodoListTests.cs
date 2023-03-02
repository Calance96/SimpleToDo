using ErrorOr;
using MediatR;
using Moq;
using Shouldly;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoLists.Commands;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.UnitTests.TodoLists.Commands;

public sealed class DeleteTodoListTests
{
	private readonly Mock<ITodoListRepository> _todoListRepository;

	private readonly DeleteTodoListHandler _handler;

	public DeleteTodoListTests()
	{
		_todoListRepository = new();

		_handler = new(_todoListRepository.Object);
	}

	[Fact]
	public async Task Handle_OnSuccess_ReturnsUnit()
	{
		// Arrange
		DeleteTodoList request = new(Guid.NewGuid());

		_todoListRepository
			.Setup(x => x.GetByIdAsync(request.ListId, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new TodoList());

		// Act
		ErrorOr<Unit> response = await _handler.Handle(request, new CancellationToken());

		// Assert
		response.IsError.ShouldBeFalse();
		response.Value.ShouldBe(Unit.Value);
	}

	[Fact]
	public async Task Handle_TodoItemNotExist_ReturnsNotFoundError()
	{
		// Arrange
		DeleteTodoList request = new(Guid.NewGuid());

		_todoListRepository
			.Setup(x => x.GetByIdAsync(request.ListId, It.IsAny<CancellationToken>()))
			.ReturnsAsync(default(TodoList));

		// Act
		ErrorOr<Unit> response = await _handler.Handle(request, new CancellationToken());

		// Assert
		response.IsError.ShouldBeTrue();
		response.FirstError.ShouldBeEquivalentTo(Errors.TodoListErrors.NotFound(request.ListId));
	}
}