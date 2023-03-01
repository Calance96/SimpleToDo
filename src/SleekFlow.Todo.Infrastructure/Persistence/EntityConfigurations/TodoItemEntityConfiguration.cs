using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleekFlow.Todo.Domain.Constants;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Domain.Enums;
using SleekFlow.Todo.Infrastructure.Extensions;

namespace SleekFlow.Todo.Infrastructure.Persistence.EntityConfigurations;

internal sealed class TodoItemEntityConfiguration : IEntityTypeConfiguration<TodoItem>
{
	public void Configure(EntityTypeBuilder<TodoItem> builder)
	{
		builder.ConfigureGuidKey(x => x.Id, "PK_TodoItems");

		builder.ConfigureAuditableProperties();

		builder.Property(x => x.Name)
			.HasColumnType("NVARCHAR")
			.HasMaxLength(DomainConstants.TodoItem.NameMaxLength);

		builder.Property(x => x.Description)
			.HasColumnType("NVARCHAR")
			.HasMaxLength(DomainConstants.TodoItem.DescriptionMaxLength);

		builder.Property(x => x.DueDate)
			.HasColumnType("DATETIME");

		builder.Property(x => x.Status)
			.HasColumnType("INTEGER")
			.HasConversion(
				value => (int)value,
				dbValue => Enum.IsDefined(typeof(TodoItemStatus), dbValue) ? (TodoItemStatus)dbValue : TodoItemStatus.Pending);
	}
}