namespace SleekFlow.Todo.Infrastructure.Configurations;

public class TokenConfiguration
{
	public const string Section = "TokenConfiguration";

	public string ValidAudience { get; init; } = null!;
	public int ValidForSeconds { get; init; }
}