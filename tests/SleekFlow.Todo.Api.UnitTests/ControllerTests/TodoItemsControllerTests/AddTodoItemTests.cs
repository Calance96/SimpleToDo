using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using SleekFlow.Todo.Api.Contracts.Requests.TodoItems;
using SleekFlow.Todo.Api.Contracts.Responses;
using SleekFlow.Todo.Api.Controllers;
using SleekFlow.Todo.Api.UnitTests.Extensions;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.TodoItems.Commands;
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Domain.Enums;

namespace SleekFlow.Todo.Api.UnitTests.ControllerTests.TodoItemsControllerTests;

public sealed class AddTodoItemTests
{
	private readonly Mock<IMediator> _mediatorMock;
	private readonly TodoItemsController _controller;

	public AddTodoItemTests()
	{
		_mediatorMock = new();
		_controller = new TodoItemsController(_mediatorMock.Object);
	}

	[Fact]
	public async Task AddTodoItem_ValidRequest_Returns200WithTodoItem()
	{
		// Arrange
		AddTodoItemRequest request = GetAddTodoItemRequest();

		ApiResponse<TodoItemDto> expectedResponse = ApiResponse<TodoItemDto>.Success(
			new()
			{
				Id = Guid.NewGuid(),
				Name = request.Name,
				Description = request.Description,
				DueDate = request.DueDate,
				Status = TodoItemStatus.Pending.ToString()
			});

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse.Data!);

		// Act
		IActionResult response = await _controller.AddTodoItem(request, new CancellationToken());

		// Assert
		response.ShouldBeOkObjectResult(typeof(ApiResponse<TodoItemDto>), expectedResponse);
	}

	[Fact]
	public async Task AddTodoItem_RequestFailedValidation_Returns400WithErrorMessage()
	{
		// Arrange
		ErrorOr.Error error = ErrorOr.Error.Validation(
			Errors.GeneralErrors.GeneralValidationErrorCode,
			"Some validation error message.");

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<AddTodoItem>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.AddTodoItem(GetAddTodoItemRequest(), new CancellationToken());

		// Assert
		response.ShouldBeBadRequestObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task AddTodoItem_NonExistentListId_Returns404WithErrorMessage()
	{
		// Arrange
		AddTodoItemRequest request = GetAddTodoItemRequest();
		ErrorOr.Error error = Errors.TodoListErrors.NotFound(request.ListId);

		BaseApiResponse expectedResponse = BaseApiResponse.Failure(error);

		_mediatorMock
			.Setup(x => x.Send(request.ToCommand(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(error);

		// Act
		IActionResult response = await _controller.AddTodoItem(request, new CancellationToken());

		// Assert
		response.ShouldBeNotFoundObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	[Fact]
	public async Task AddTodoItem_RequestUnexpectedFailure_Returns500()
	{
		// Arrange
		BaseApiResponse expectedResponse = BaseApiResponse.Failure(Errors.GeneralErrors.GeneralUnexpectedError);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<AddTodoItem>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Errors.GeneralErrors.GeneralUnexpectedError);

		// Act
		IActionResult response = await _controller.AddTodoItem(GetAddTodoItemRequest(), new CancellationToken());

		// Assert
		response.ShouldBeInternalServerErrorObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	private static AddTodoItemRequest GetAddTodoItemRequest()
		=> new(
			Guid.NewGuid(),
			"Item 1 Name",
			"Item 1 Description",
			null);
}