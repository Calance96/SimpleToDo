namespace SleekFlow.Todo.Api.Contracts.Interfaces;

internal interface ICommand<TCommand>
	where TCommand : class
{
	TCommand ToCommand();
}