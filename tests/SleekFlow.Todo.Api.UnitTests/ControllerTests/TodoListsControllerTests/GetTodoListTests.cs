using Azure.Core;
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
using SleekFlow.Todo.Application.TodoItems.Dtos;
using SleekFlow.Todo.Application.TodoLists.Dtos;
using SleekFlow.Todo.Application.TodoLists.Queries;
using SleekFlow.Todo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekFlow.Todo.Api.UnitTests.ControllerTests.TodoListsControllerTests;

public sealed class GetTodoListTests
{
	private readonly Mock<IMediator> _mediatorMock;
	private readonly TodoListsController _controller;

	public GetTodoListTests()
	{
		_mediatorMock = new();
		_controller = new TodoListsController(_mediatorMock.Object);
	}

	[Fact]
	public async Task GetTodoLists_HasData_Returns200WithData()
	{
		// Arrange
		ApiResponse<List<TodoListDto>> expectedResponse = ApiResponse<List<TodoListDto>>.Success(GetTodoListsSampleData());

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<GetAllTodoLists>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse.Data!);

		// Act
		IActionResult response = await _controller.GetTodoLists(new CancellationToken());

		// Assert
		response.ShouldBeOkObjectResult(typeof(ApiResponse<List<TodoListDto>>), expectedResponse);
	}

	[Fact]
	public async Task GetTodoLists_HasNoData_Returns200WithEmptyData()
	{
		// Arrange
		ApiResponse<List<TodoListDto>> expectedResponse = ApiResponse<List<TodoListDto>>.Success(new List<TodoListDto>());

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<GetAllTodoLists>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedResponse.Data!);

		// Act
		IActionResult response = await _controller.GetTodoLists(new CancellationToken());

		// Assert
		response.ShouldBeOkObjectResult(typeof(ApiResponse<List<TodoListDto>>), expectedResponse);
	}

	[Fact]
	public async Task GetTodoLists_RequestUnexpectedFailure_Returns500()
	{
		// Arrange
		BaseApiResponse expectedResponse = BaseApiResponse.Failure(Errors.GeneralErrors.GeneralUnexpectedError);

		_mediatorMock
			.Setup(x => x.Send(It.IsAny<GetAllTodoLists>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Errors.GeneralErrors.GeneralUnexpectedError);

		// Act
		IActionResult response = await _controller.GetTodoLists(new CancellationToken());

		// Assert
		response.ShouldBeInternalServerErrorObjectResult(typeof(BaseApiResponse), expectedResponse);
	}

	private static List<TodoListDto> GetTodoListsSampleData(int size = 1)
		=> Enumerable.Range(1, size)
			.Select(i => new TodoListDto
			{
				Id = Guid.NewGuid(),
				Title = $"List title {i}",
				CreatedBy = "Some user",
				CreatedAt = DateTimeOffset.UtcNow,
				Items = new List<TodoItemDto>()
				{
					new()
					{
						Id = Guid.NewGuid(),
						Name = Guid.NewGuid().ToString("N"),
						Description = Guid.NewGuid().ToString("N"),
						DueDate = null,
						Status = TodoItemStatus.Pending.ToString()
					}
				}
			})
			.ToList();
}
