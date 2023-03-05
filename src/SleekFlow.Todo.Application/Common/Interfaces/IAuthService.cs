using ErrorOr;

namespace SleekFlow.Todo.Application.Common.Interfaces;

public interface IAuthService
{
	Task<ErrorOr<string>> CreateUserAsync(string username, string password, CancellationToken cancellationToken);
	Task<ErrorOr<string>> VerifyCredentialsAsync(string username, string password, CancellationToken cancellationToken);
}