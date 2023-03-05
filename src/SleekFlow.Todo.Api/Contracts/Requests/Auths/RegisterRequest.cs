using SleekFlow.Todo.Api.Contracts.Interfaces;
using SleekFlow.Todo.Application.Auths.Commands;

namespace SleekFlow.Todo.Api.Contracts.Requests.Auths;

public sealed record RegisterRequest(
	string UserName,
	string Password) : ICommand<UserRegister>
{
	public UserRegister ToCommand()
		=> new(UserName, Password);
}
