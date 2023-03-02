using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Commands;
using SleekFlow.Todo.Domain.Entities;

namespace SleekFlow.Todo.Application.UnitTests.TodoItems.Commands;

public sealed class DeleteTodoItemTests
{
	private readonly Mock<ITodoItemRepository> _todoItemRepository;

	private readonly DeleteTodoItemHandler _handler;

	public DeleteTodoItemTests()
	{
		_todoItemRepository = new();

		_handler = new(_todoItemRepository.Object);
	}

	[Fact]
	public async Task Handle_OnSuccess_ReturnsUnit()
	{
		// Arrange
		DeleteTodoItem request = new(Guid.NewGuid());

		_todoItemRepository
			.Setup(x => x.GetByIdAsync(request.ItemId, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new TodoItem());

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
		DeleteTodoItem request = new(Guid.NewGuid());

		_todoItemRepository
			.Setup(x => x.GetByIdAsync(request.ItemId, It.IsAny<CancellationToken>()))
			.ReturnsAsync(default(TodoItem));

		// Act
		ErrorOr<Unit> response = await _handler.Handle(request, new CancellationToken());

		// Assert
		response.IsError.ShouldBeTrue();
		response.FirstError.ShouldBeEquivalentTo(Errors.TodoItemErrors.NotFound(request.ItemId));
	}
}