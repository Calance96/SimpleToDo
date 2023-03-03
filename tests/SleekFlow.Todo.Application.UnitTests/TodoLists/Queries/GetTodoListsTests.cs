using Mapster;
using SleekFlow.Todo.Application.Common.Enums;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Application.TodoLists.Dtos;
using SleekFlow.Todo.Application.TodoLists.Queries;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Application.UnitTests.TodoLists.Queries;

public class GetTodoListsTests
{
    private readonly Mock<ITodoListRepository> _todoListRepository;

	private readonly GetAllTodoListsHandler _handler;

    public GetTodoListsTests()
    {
        _todoListRepository = new Mock<ITodoListRepository>();

        _handler = new(_todoListRepository.Object);
    }

    [Fact]
    public async Task Handle_OnSuccess_ReturnsTodoLists()
    {
        // Arrange
        List<TodoList> sampleData = GetTodoListsSampleData();
        List<TodoListDto> expected = sampleData.Adapt<List<TodoListDto>>();

		_todoListRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sampleData);

        // Act
        ErrorOr<List<TodoListDto>> response = await _handler.Handle(new GetAllTodoLists(null, null), new CancellationToken());

        // Assert
        response.IsError.ShouldBeFalse();
        response.Value.ShouldBeEquivalentTo(expected);
    }

	[Fact]
	public async Task Handle_SortTitleDesc_ReturnsTodoListsInCorrectOrder()
	{
		// Arrange
		List<TodoList> sampleData = GetTodoListsSampleData(5);
		List<TodoListDto> expected = sampleData
			.Adapt<List<TodoListDto>>()
			.OrderByDescending(x => x.Title)
			.ToList();

		_todoListRepository
			.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(sampleData);

		// Act
		ErrorOr<List<TodoListDto>> response = await _handler.Handle(
			new GetAllTodoLists(TodoListSortableField.Title.ToString(), SortOrder.Desc.ToString()), 
			new CancellationToken());

		// Assert
		response.IsError.ShouldBeFalse();
		response.Value.ShouldBeEquivalentTo(expected);
	}

	[Fact]
	public async Task Handle_SortDateAsc_ReturnsTodoListsInCorrectOrder()
	{
		// Arrange
		List<TodoList> sampleData = GetTodoListsSampleData(5);
		List<TodoListDto> expected = sampleData
			.Adapt<List<TodoListDto>>()
			.OrderBy(x => x.CreatedAt)
			.ToList();

		_todoListRepository
			.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(sampleData);

		// Act
		ErrorOr<List<TodoListDto>> response = await _handler.Handle(
			new GetAllTodoLists(TodoListSortableField.Date.ToString(), SortOrder.Asc.ToString()),
			new CancellationToken());

		// Assert
		response.IsError.ShouldBeFalse();
		response.Value.ShouldBeEquivalentTo(expected);
	}

	[Fact]
	public async Task Handle_OnNoData_ReturnsEmptyList()
	{
		// Arrange
		List<TodoListDto> expected = new();

		_todoListRepository
			.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<TodoList>());

		// Act
		ErrorOr<List<TodoListDto>> response = await _handler.Handle(new GetAllTodoLists(null, null), new CancellationToken());

		// Assert
		response.IsError.ShouldBeFalse();
		response.Value.ShouldBe(expected);
	}

	private static List<TodoList> GetTodoListsSampleData(int size = 1)
		=> Enumerable.Range(1, size)
			.Select(i =>
            {
                TodoList todoList = TodoList.Create($"title {i}");
				todoList.CreatedAt = DateTimeOffset.UtcNow.AddSeconds(new Random().Next(10));
				todoList.CreatedBy = "some user";

                todoList.Items.Add(TodoItem.Create($"item {i}", $"item description {i}", null, todoList.Id));

                return todoList;
            })
			.ToList();
}