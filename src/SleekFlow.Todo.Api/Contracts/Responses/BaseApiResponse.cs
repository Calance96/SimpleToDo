namespace SleekFlow.Todo.Api.Contracts.Responses;

public class BaseApiResponse
{
	public bool IsSuccess { get; protected set; }
	public IEnumerable<Error> Errors => _errors;

	protected readonly List<Error> _errors = new();

	protected BaseApiResponse(bool isSuccess, IEnumerable<Error>? errors = null)
	{
		IsSuccess = isSuccess;

		if (errors is not null)
		{
			_errors.AddRange(errors);
		}
	}

	public static BaseApiResponse Success()
		=> new(true, null);

	public static BaseApiResponse Failure()
		=> new(true, null);

	public static BaseApiResponse Failure(Error error)
	{
		BaseApiResponse apiResponse = new(false, default);
		apiResponse.AddError(error);

		return apiResponse;
	}

	public static BaseApiResponse Failure(ErrorOr.Error error)
		=> Failure(new Error(error.Description, error.Code));

	public static BaseApiResponse Failure(IEnumerable<Error>? errors)
		=> new(false, errors);

	public static BaseApiResponse Failure(IEnumerable<ErrorOr.Error>? errors)
		=> Failure(errors?.Select(e => new Error(e.Description, e.Code)));

	public void AddError(Error error)
		=> _errors.Add(error);

	public void AddError(ErrorOr.Error error)
		=> AddError(new Error(error.Description, error.Code));
}