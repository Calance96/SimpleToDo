using FluentValidation;
using SleekFlow.Todo.Domain.Constants;

namespace SleekFlow.Todo.Application.TodoLists.Dtos;

public sealed record CreateTodoItemListDto(
    string Name,
    string Description,
    DateTimeOffset? DueDate);