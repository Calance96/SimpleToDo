namespace SleekFlow.Todo.Api.Contracts.Responses;

public class ApiResponse
{
	public bool IsSuccess { get; private set; }
	public object? Data { get; private set; }
	public IEnumerable<Error> Errors => _errors;

	private readonly List<Error> _errors = new();

	private ApiResponse(bool isSuccess, object? data, IEnumerable<Error>? errors = null)
	{
		IsSuccess = isSuccess;
		Data = data;

		if (errors is not null)
		{
			_errors.AddRange(errors);
		}
	}

	public static ApiResponse Success(object? data)
		=> new(true, data);

	public static ApiResponse Failure()
		=> new(false, null);

	public static ApiResponse Failure(Error error)
	{
		ApiResponse apiResponse = new(false, null);
		apiResponse.AddError(error);

		return apiResponse;
	}

	public static ApiResponse Failure(ErrorOr.Error error)
		=> Failure(new Error(error.Description, error.Code));

	public static ApiResponse Failure(IEnumerable<Error>? errors)
		=> new(false, null, errors);

	public static ApiResponse Failure(IEnumerable<ErrorOr.Error>? errors)
		=> Failure(errors?.Select(e => new Error(e.Description, e.Code)));

	public void AddError(Error error)
		=> _errors.Add(error);

	public void AddError(ErrorOr.Error error)
		=> AddError(new Error(error.Description, error.Code));
}