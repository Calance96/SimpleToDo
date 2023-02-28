using ErrorOr;

namespace SleekFlow.Todo.Application.Common.Exceptions;

public static class Errors
{
	public const string ItemNotFound = "not_found";

	public static class GeneralErrors
	{
		public const string GeneralValidationErrorCode = "validation_error";
		public static Error GeneralUnexpectedError => Error.Unexpected("general_error", "One or more errors has occurred while processing the request.");
	}

	public static class TodoListErrors
	{
		public static Error NotFound(Guid listId) => Error.NotFound(ItemNotFound, $"Todo list of ID '{listId}' not found.");
	}

	public static class TodoItemErrors
	{
		public static Error NotFound(Guid itemId) => Error.NotFound(ItemNotFound, $"Todo item of ID '{itemId}' not found.");
	}
}