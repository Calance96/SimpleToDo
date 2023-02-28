namespace SleekFlow.Todo.Api.Contracts.Responses;

public sealed record Error(string Message, string? Code);