using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleekFlow.Todo.Domain.Common;
using SleekFlow.Todo.Domain.Constants;
using System.Linq.Expressions;

namespace SleekFlow.Todo.Infrastructure.Extensions;

internal static class EntityConfigurationExtensions
{
	public static void ConfigureAuditableProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
		where TEntity : AuditableEntity
	{
		builder.Property(e => e.CreatedBy)
			.HasColumnType("VARCHAR")
			.HasMaxLength(DomainConstants.User.UserIdLength);

		builder.Property(e => e.CreatedAt)
			.HasColumnType("DATETIME");

		builder.Property(e => e.UpdatedBy)
			.HasColumnType("VARCHAR")
			.HasMaxLength(DomainConstants.User.UserIdLength);

		builder.Property(e => e.UpdatedAt)
			.HasColumnType("DATETIME");
	}

	public static void ConfigureGuidKey<TEntity>(
		this EntityTypeBuilder<TEntity> builder,
		Expression<Func<TEntity, object?>> keySelector,
		string constraintName)
		where TEntity : class
	{
		builder.HasKey(keySelector)
			.HasName(constraintName)
			.IsClustered();

		builder.Property(keySelector)
			.HasColumnType("VARCHAR")
			.HasMaxLength(DomainConstants.User.UserIdLength)
			.ValueGeneratedNever();
	}
}