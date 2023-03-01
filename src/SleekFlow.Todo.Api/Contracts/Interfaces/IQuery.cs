namespace SleekFlow.Todo.Api.Contracts.Interfaces;

internal interface IQuery<TQuery>
	where TQuery : class
{
	TQuery ToQuery();
}