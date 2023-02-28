namespace SleekFlow.Todo.Domain.Common;

public abstract class AuditableEntity
{
	public string CreatedBy { get; set; } = null!;
	public DateTimeOffset CreatedAt { get; set; }

	public string? UpdatedBy { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
}