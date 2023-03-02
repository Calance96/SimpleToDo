using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Requests.TodoLists;
using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Api.Controllers;
using SleekFlow.Todo.Api.UnitTests.Extensions;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.TodoLists.Commands;

namespace SleekFlow.Todo.Api.UnitTests.ControllerTests.TodoListsControllerTests;

public sealed class UpdateTodoListTests
{
	private readonly Mock<IMediator> _mediatorMock;
	private readonly TodoListsController _controller;

	public UpdateTodoListTests()
	{
		_mediatorMock = new();
		_controller = new TodoListsController(_mediatorMock.Object);
	}

	[Fact]
	public async Task UpdateTodoList_ValidRequest_Returns200WithTodoList()
	{
		// Arrange
		UpdateTodoListRequest request = GetUpdateTodoListRequest();

		BaseApiResponse expectedResponse = BaseApiResponse.Success();

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Unit.Value);

		// Act
		IActionResult response = await _controller.UpdateTodoList(request, new CancellationToken());

		// Assert
		response.ShouldBeOkObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task UpdateTodoList_RequestFailedValidation_Returns400WithErrorMessage()
	{
		// Arrange
		ErrorOr.Error error = ErrorOr.Error.Validation(
			Errors.GeneralErrors.GeneralValidationErrorCode,
			"Some validation error message.");

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<UpdateTodoList>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.UpdateTodoList(GetUpdateTodoListRequest(), new CancellationToken());

		// Assert
		response.ShouldBeBadRequestObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task UpdateTodoList_NonExistentItemId_Returns404WithErrorMessage()
	{
		// Arrange
		UpdateTodoListRequest request = GetUpdateTodoListRequest();
		ErrorOr.Error error = Errors.TodoListErrors.NotFound(request.ListId);

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.UpdateTodoList(request, new CancellationToken());

		// Assert
		response.ShouldBeNotFoundObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task UpdateTodoList_RequestUnexpectedFailure_Returns500()
	{
		// Arrange
		BaseApiResponse expectedResponse = BaseApiResponse.Failure(Errors.GeneralErrors.GeneralUnexpectedError);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<UpdateTodoList>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Errors.GeneralErrors.GeneralUnexpectedError);

		// Act
		IActionResult response = await _controller.UpdateTodoList(GetUpdateTodoListRequest(), new CancellationToken());

		// Assert
		response.ShouldBeInternalServerErrorObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	private static UpdateTodoListRequest GetUpdateTodoListRequest()
		=> new()
		{
			ListId = Guid.NewGuid(),
			Body = new()
			{
				Title = "Updated title"
			}
		};
}