using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleekFlow.Todo.Domain.Constants;
using SleekFlow.Todo.Domain.Entities;
using SleekFlow.Todo.Infrastructure.Extensions;

namespace SleekFlow.Todo.Infrastructure.Persistence.EntityConfigurations;

internal sealed class TodoListEntityConfiguration : IEntityTypeConfiguration<TodoList>
{
	public void Configure(EntityTypeBuilder<TodoList> builder)
	{
		builder.ConfigureGuidKey(x => x.Id, "PK_TodoLists");

		builder.ConfigureAuditableProperties();

		builder.Property(x => x.Title)
			.HasColumnType("NVARCHAR")
			.HasMaxLength(DomainConstants.TodoList.TitleMaxLength);

		builder.HasMany(x => x.Items)
			.WithOne()
			.HasConstraintName("FK_TodoItem_TodoList")
			.HasForeignKey("ListId");
	}
}