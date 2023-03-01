namespace SleekFlow.Todo.Api.Contracts.Responses;

public class ApiResponse<T> : BaseApiResponse
{
	public T? Data { get; private set; }

	private ApiResponse(bool isSuccess, T? data, IEnumerable<Error>? errors = null) : base(isSuccess, errors)
		=> Data = data;

	public static ApiResponse<T> Success(T? data)
		=> new(true, data);

	public static new ApiResponse<T> Failure()
		=> new(false, default);

	public static new ApiResponse<T> Failure(Error error)
	{
		ApiResponse<T> apiResponse = new(false, default);
		apiResponse.AddError(error);

		return apiResponse;
	}

	public static new ApiResponse<T> Failure(ErrorOr.Error error)
		=> Failure(new Error(error.Description, error.Code));

	public static new ApiResponse<T> Failure(IEnumerable<Error>? errors)
		=> new(false, default, errors);

	public static new ApiResponse<T> Failure(IEnumerable<ErrorOr.Error>? errors)
		=> Failure(errors?.Select(e => new Error(e.Description, e.Code)));
}