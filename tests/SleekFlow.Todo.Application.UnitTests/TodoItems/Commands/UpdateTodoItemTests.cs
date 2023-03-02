using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Commands;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Application.UnitTests.TodoItems.Commands;

public sealed class UpdateTodoItemTests
{
	private readonly Mock<ITodoItemRepository> _todoItemRepository;

	private readonly UpdateTodoItemHandler _handler;

	public UpdateTodoItemTests()
	{
		_todoItemRepository = new();

		_handler = new(_todoItemRepository.Object);
	}

	[Fact]
	public async Task Handle_OnSuccess_ReturnsUnit()
	{
		// Arrange
		UpdateTodoItem request = new(
			Guid.NewGuid(),
			"name",
			"description",
			TodoItemStatus.Done,
			DateTimeOffset.UtcNow.AddDays(7));

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
		UpdateTodoItem request = new(
			Guid.NewGuid(),
			"name",
			"description",
			TodoItemStatus.Done,
			DateTimeOffset.UtcNow.AddDays(7));

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