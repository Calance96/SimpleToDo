using ErrorOr;
using Moq;
using Shouldly;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Application.TodoLists.Commands;
using SleekFlow.Todo.Application.TodoLists.Dtos;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Application.UnitTests.TodoLists.Commands;

public sealed class CreateTodoListTests
{
	private readonly Mock<ITodoListRepository> _todoListRepository;

	private readonly CreateTodoListHandler _handler;

    public CreateTodoListTests()
    {
        _todoListRepository = new();

        _handler = new(_todoListRepository.Object);
    }

    [Fact]
    public async Task Handle_OnSuccess_ReturnsCreatedTodoList()
    {
        // Arrange
        CreateTodoList request = new(
            "title",
            new List<CreateTodoItemDto>
            {
                new("name", "description", DateTimeOffset.UtcNow.AddDays(7))
            });

        TodoListDto expected = new()
        {
            Title = request.Title,
            Items = new List<TodoItemDto>
            {
                new()
                {
                    Name = request.Items!.First().Name,
                    Description =  request.Items!.First().Description,
                    DueDate = request.Items!.First().DueDate,
                    Status = TodoItemStatus.Pending.ToString()
                }
            }
        };

        _todoListRepository
            .Setup(x => x.AddAsync(It.IsAny<TodoList>(), It.IsAny<CancellationToken>()))
            .Callback<TodoList, CancellationToken>((item, token) =>
            {
                expected.Id = item.Id;
                expected.CreatedBy = item.CreatedBy;
                expected.CreatedAt = item.CreatedAt;

                expected.Items.First().Id = item.Items.First().Id;
            })
            .Returns(Task.CompletedTask);

        // Act
        ErrorOr<TodoListDto> response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.IsError.ShouldBeFalse();
        response.Value.ShouldBeEquivalentTo(expected);
    }
}