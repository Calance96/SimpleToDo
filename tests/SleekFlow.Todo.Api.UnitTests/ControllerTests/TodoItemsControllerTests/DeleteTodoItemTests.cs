using Microsoft.AspNetCore.Mvc;
using SleekFlow.Todo.Api.Contracts.Requests.TodoItems;
using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Api.Controllers;
using SleekFlow.Todo.Api.UnitTests.Extensions;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.TodoItems.Commands;

namespace SleekFlow.Todo.Api.UnitTests.ControllerTests.TodoItemsControllerTests;

public sealed class DeleteTodoItemTests
{
	private readonly Mock<IMediator> _mediatorMock;
	private readonly TodoItemsController _controller;

	public DeleteTodoItemTests()
	{
		_mediatorMock = new();
		_controller = new TodoItemsController(_mediatorMock.Object);
	}

	[Fact]
	public async Task DeleteTodoItem_ValidRequest_Returns200WithTodoItem()
	{
		// Arrange
		DeleteTodoItemRequest request = GetDeleteTodoItemRequest();

		BaseApiResponse expectedResponse = BaseApiResponse.Success();

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Unit.Value);

		// Act
		IActionResult response = await _controller.DeleteTodoItem(request, new CancellationToken());

		// Assert
		response.ShouldBeOkObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task DeleteTodoItem_RequestFailedValidation_Returns400WithErrorMessage()
	{
		// Arrange
		ErrorOr.Error error = ErrorOr.Error.Validation(
			Errors.GeneralErrors.GeneralValidationErrorCode,
			"Some validation error message.");

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<DeleteTodoItem>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.DeleteTodoItem(GetDeleteTodoItemRequest(), new CancellationToken());

		// Assert
		response.ShouldBeBadRequestObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task DeleteTodoItem_NonExistentItemId_Returns404WithErrorMessage()
	{
		// Arrange
		DeleteTodoItemRequest request = GetDeleteTodoItemRequest();
		ErrorOr.Error error = Errors.TodoItemErrors.NotFound(request.ItemId);

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.DeleteTodoItem(request, new CancellationToken());

		// Assert
		response.ShouldBeNotFoundObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task DeleteTodoItem_RequestUnexpectedFailure_Returns500()
	{
		// Arrange
		BaseApiResponse expectedResponse = BaseApiResponse.Failure(Errors.GeneralErrors.GeneralUnexpectedError);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<DeleteTodoItem>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Errors.GeneralErrors.GeneralUnexpectedError);

		// Act
		IActionResult response = await _controller.DeleteTodoItem(GetDeleteTodoItemRequest(), new CancellationToken());

		// Assert
		response.ShouldBeInternalServerErrorObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	private static DeleteTodoItemRequest GetDeleteTodoItemRequest()
		=> new(Guid.NewGuid());
}