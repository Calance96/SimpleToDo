using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using SleekFlow.Todo.Api.Contracts.Requests.TodoLists;
using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Api.Controllers;
using SleekFlow.Todo.Api.UnitTests.Extensions;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.TodoLists.Commands;

namespace SleekFlow.Todo.Api.UnitTests.ControllerTests.TodoListsControllerTests;

public sealed class DeleteTodoListTests
{
	private readonly Mock<IMediator> _mediatorMock;
	private readonly TodoListsController _controller;

	public DeleteTodoListTests()
	{
		_mediatorMock = new();
		_controller = new TodoListsController(_mediatorMock.Object);
	}

	[Fact]
	public async Task DeleteTodoList_ValidRequest_Returns200WithTodoList()
	{
		// Arrange
		DeleteTodoListRequest request = GetDeleteTodoListRequest();

		BaseApiResponse expectedResponse = BaseApiResponse.Success();

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Unit.Value);

		// Act
		IActionResult response = await _controller.DeleteTodoList(request, new CancellationToken());

		// Assert
		response.ShouldBeOkObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task DeleteTodoList_RequestFailedValidation_Returns400WithErrorMessage()
	{
		// Arrange
		ErrorOr.Error error = ErrorOr.Error.Validation(
			Errors.GeneralErrors.GeneralValidationErrorCode,
			"Some validation error message.");

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<DeleteTodoList>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.DeleteTodoList(GetDeleteTodoListRequest(), new CancellationToken());

		// Assert
		response.ShouldBeBadRequestObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task DeleteTodoList_NonExistentItemId_Returns404WithErrorMessage()
	{
		// Arrange
		DeleteTodoListRequest request = GetDeleteTodoListRequest();
		ErrorOr.Error error = Errors.TodoListErrors.NotFound(request.ListId);

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.DeleteTodoList(request, new CancellationToken());

		// Assert
		response.ShouldBeNotFoundObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task DeleteTodoList_RequestUnexpectedFailure_Returns500()
	{
		// Arrange
		BaseApiResponse expectedResponse = BaseApiResponse.Failure(Errors.GeneralErrors.GeneralUnexpectedError);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<DeleteTodoList>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Errors.GeneralErrors.GeneralUnexpectedError);

		// Act
		IActionResult response = await _controller.DeleteTodoList(GetDeleteTodoListRequest(), new CancellationToken());

		// Assert
		response.ShouldBeInternalServerErrorObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	private static DeleteTodoListRequest GetDeleteTodoListRequest()
		=> new(Guid.NewGuid());
}