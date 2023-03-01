using Microsoft.EntityFrameworkCore;
using SleekFlow.Todo.Application.Common.Interfaces;

namespace SleekFlow.Todo.Infrastructure.Repositories;

internal class GenericRepository<TEntity> : IGenericRepository<TEntity>
	where TEntity : class
{
	private readonly DbContext _dbContext;
	private readonly DbSet<TEntity> _dbSet;

	public GenericRepository(DbContext dbContext)
	{
		_dbContext = dbContext;
		_dbSet = dbContext.Set<TEntity>();
	}

	public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
	{
		_dbSet.Add(entity);

		return _dbContext.SaveChangesAsync(cancellationToken);
	}

	public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
	{
		_dbSet.Remove(entity);

		return _dbContext.SaveChangesAsync(cancellationToken);
	}

	public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
	{
		_dbSet.Update(entity);

		return _dbContext.SaveChangesAsync(cancellationToken);
	}
}