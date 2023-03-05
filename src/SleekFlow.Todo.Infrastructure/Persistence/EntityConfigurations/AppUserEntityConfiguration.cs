using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SleekFlow.Todo.Infrastructure.Extensions;
using SleekFlow.Todo.Infrastructure.Identities;

namespace SleekFlow.Todo.Infrastructure.Persistence.EntityConfigurations;

internal sealed class AppUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
{
	public void Configure(EntityTypeBuilder<AppUser> builder)
	{
		builder.ConfigureGuidKey(x => x.Id, "PK_AppUsers");

		builder.ConfigureAuditableProperties();

		builder.Property(x => x.UserName)
			.HasColumnType("NVARCHAR")
			.HasMaxLength(256);

		builder.Property(x => x.PasswordHash)
			.HasColumnType("NVARCHAR(MAX)");
	}
}