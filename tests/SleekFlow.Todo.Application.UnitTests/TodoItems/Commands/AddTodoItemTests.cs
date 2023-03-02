using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Commands;
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Application.UnitTests.TodoItems.Commands;

public sealed class AddTodoItemTests
{
	private readonly Mock<ITodoItemRepository> _todoItemRepository;
	private readonly Mock<ITodoListRepository> _todoListRepository;

	private readonly AddTodoItemHandler _handler;

	public AddTodoItemTests()
	{
		_todoItemRepository = new();
		_todoListRepository = new();

		_handler = new AddTodoItemHandler(_todoListRepository.Object, _todoItemRepository.Object);
	}

	[Fact]
	public async Task Handle_OnSuccess_ReturnsCreatedTodoItem()
	{
		// Arrange
		AddTodoItem request = new(
			Guid.NewGuid(),
			new("name", "description", DateTimeOffset.Now.AddDays(7)));

		TodoItemDto expected = new()
		{
			Name = request.Item.Name,
			Description = request.Item.Description,
			DueDate = request.Item.DueDate,
			Status = TodoItemStatus.Pending.ToString()
		};

		_todoListRepository
			.Setup(x => x.GetByIdAsync(request.ListId, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new TodoList());

		_todoItemRepository
			.Setup(x => x.AddAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
			.Callback<TodoItem, CancellationToken>((item, token) => expected.Id = item.Id)
			.Returns(Task.CompletedTask);

		// Act
		ErrorOr<TodoItemDto> response = await _handler.Handle(request, new CancellationToken());

		// Assert
		response.IsError.ShouldBeFalse();
		response.Value.ShouldBeEquivalentTo(expected);
	}
}