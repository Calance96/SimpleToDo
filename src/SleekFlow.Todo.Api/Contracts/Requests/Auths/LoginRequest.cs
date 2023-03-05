using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.Auths.Commands;

namespace SleekFlow.Todo.Api.Contracts.Requests.Auths;

public sealed record LoginRequest(
	string UserName,
	string Password) : ICommand<UserLogin>
{
	public UserLogin ToCommand()
		=> new(UserName, Password);
}
