using Mapster;
using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Requests.TodoLists;
using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Api.Controllers;
using SleekFlow.Todo.Api.UnitTests.Extensions;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Application.TodoLists.Commands;
using SleekFlow.Todo.Application.TodoLists.Dtos;

namespace SleekFlow.Todo.Api.UnitTests.ControllerTests.TodoListsControllerTests;

public sealed class CreateTodoListTests
{
	private readonly Mock<IMediator> _mediatorMock;
	private readonly TodoListsController _controller;

	public CreateTodoListTests()
	{
		_mediatorMock = new();
		_controller = new TodoListsController(_mediatorMock.Object);
	}

	[Fact]
	public async Task CreateTodoList_ValidRequest_Returns200WithTodoList()
	{
		// Arrange
		CreateTodoListRequest request = GetCreateTodoListRequest();

		ApiResponse<TodoListDto> expectedResponse = ApiResponse<TodoListDto>.Success(
			new TodoListDto
			{
				Id = Guid.NewGuid(),
				Title = request.Title,
				Items = request.Items!.Adapt<List<TodoItemDto>>(),
				CreatedBy = "someone",
				CreatedAt = DateTimeOffset.UtcNow
			});

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse.Data!);

		// Act
		IActionResult response = await _controller.CreateTodoList(request, new CancellationToken());

		// Assert
		response.ShouldBeOkObjectResult(typeof(ApiResponse<TodoListDto>), expectedResponse);
	}

	[Fact]
	public async Task CreateTodoList_RequestFailedValidation_Returns400WithErrorMessage()
	{
		// Arrange
		ErrorOr.Error error = ErrorOr.Error.Validation(
			Errors.GeneralErrors.GeneralValidationErrorCode,
			"Some validation error message.");

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<CreateTodoList>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.CreateTodoList(GetCreateTodoListRequest(), new CancellationToken());

		// Assert
		response.ShouldBeBadRequestObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task CreateTodoList_RequestUnexpectedFailure_Returns500()
	{
		// Arrange
		BaseApiResponse expectedResponse = BaseApiResponse.Failure(Errors.GeneralErrors.GeneralUnexpectedError);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<CreateTodoList>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Errors.GeneralErrors.GeneralUnexpectedError);

		// Act
		IActionResult response = await _controller.CreateTodoList(GetCreateTodoListRequest(), new CancellationToken());

		// Assert
		response.ShouldBeInternalServerErrorObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	private static CreateTodoListRequest GetCreateTodoListRequest()
		=> new(
			"List title",
			new List<CreateTodoItemDto>
			{
				new("item 1 name", "item 2 description", null)
			});
}