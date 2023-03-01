namespace SleekFlow.Todo.Application.Common.Interfaces;

public interface IClock
{
	public DateTimeOffset Now { get; }
}