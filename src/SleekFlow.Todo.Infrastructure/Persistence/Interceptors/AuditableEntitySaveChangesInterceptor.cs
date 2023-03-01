using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Domain.Common;

namespace SleekFlow.Todo.Infrastructure.Persistence.Interceptors;

internal sealed class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
	private readonly IClock _clock;
	private readonly ICurrentUserService _currentUserService;

	public AuditableEntitySaveChangesInterceptor(
		ICurrentUserService currentUserService, 
		IClock clock)
	{
		_clock = clock;
		_currentUserService = currentUserService;
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		SetEntitiesMetadata(eventData.Context);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private void SetEntitiesMetadata(DbContext? context)
	{
		if (context is null)
		{
			return;
		}

		string userId = _currentUserService.UserId ?? "test-user";

		foreach (EntityEntry<AuditableEntity> entityEntry in context.ChangeTracker.Entries<AuditableEntity>())
		{
			if (entityEntry.State is EntityState.Added)
			{
				entityEntry.Entity.CreatedAt = _clock.Now;
				entityEntry.Entity.CreatedBy = userId;
			}
			else if (entityEntry.State is EntityState.Modified)
			{
				entityEntry.Entity.UpdatedAt = _clock.Now;
				entityEntry.Entity.UpdatedBy = userId;
			}
		}
	}
}